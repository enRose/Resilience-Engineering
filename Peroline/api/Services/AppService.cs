using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using api.Services;
using AutoMapper;
using Polly.Contrib.Simmy;
using Polly.Contrib.Simmy.Outcomes;
using retry.Helpers;
using retry.ViewModels;

namespace retry.Services
{
    public interface IAppService
    {
        Task<PersonalLoanVm> GetApp();
        Task<bool> SubmitApp();
        Task<bool> AgreeToRetry();
        Task<bool> ConsumerRetry(PersonalLoanVm app);
    }

    public class AppService : IAppService
    {
        private DataContext _dbContext;
        private readonly IKafkaProducer kafkaProducer;
        private readonly ICoreBankingService coreBankingService;
        
        public AppService(
            DataContext context,
            IKafkaProducer k,
            CoreBankingService c)
        {
            _dbContext = context;
            kafkaProducer = k;
            coreBankingService = c;
        }

        public async Task<PersonalLoanVm> GetApp()
        {
            var pl = _dbContext.PersonalLoans.SingleOrDefault(x => x.Id == 1);

            var accounts = await coreBankingService.GetAccounts(userId: "1");

            return new PersonalLoanVm
            {
                App = new AppVm { Data = pl?.App?.Data },
                Accounts = accounts?.Select(a => new AccountVm { Name = a.Name})
            };
        }

        public async Task<bool> SubmitApp()
        {
            var app = await GetApp();

            return await InvokeDownstreamAPI(app);
        }

        public async Task<bool> AgreeToRetry()
        {
            var app = await GetApp();

            kafkaProducer.Produce(Guid.NewGuid().ToString(), app);

            return true;
        }

        public async Task<bool> ConsumerRetry(
            PersonalLoanVm app)
        {
            return await InvokeDownstreamAPI(app);
        }

        public async Task<bool> InvokeDownstreamAPI(PersonalLoanVm app)
        {
            var fault = new IOException();

            var chaos = MonkeyPolicy
                .InjectExceptionAsync(with =>
                    with.Fault(fault)
                    .InjectionRate(1)
                    .Enabled()
                );

            var path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Environment.CurrentDirectory + @"/downstream.txt");

            using StreamWriter outputFile = new StreamWriter(path, true);

            await chaos.ExecuteAsync(async () =>
                await outputFile.WriteAsync(
                $"\n{JsonSerializer.Serialize(app)}\n"
                )
            );

            return true;
        }    
    }
}
