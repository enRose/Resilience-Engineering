using System;
using System.Linq;
using System.Threading.Tasks;
using api.Services;
using api.Entities;
using api.Helpers;
using api.ViewModels;

namespace api.Services
{
    public interface IAppService
    {
        Task<PersonalLoanVm> GetApp();
        Task<bool> SubmitApp();
        Task<bool> AgreeToRetry();
        Task<bool> ConsumerRetry(PersonalLoanVm app);
        Task<bool> NotificationConsent();
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
                Accounts = accounts?.Select(a => new AccountVm { Name = a.Name })
            };
        }

        public async Task<bool> SubmitApp()
        {
            var app = await GetApp();

            return await coreBankingService.ApplyFor(Convert(app));
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
            return await coreBankingService.ApplyFor(Convert(app));
        }

        private PersonalLoan Convert(PersonalLoanVm dest)
            => new PersonalLoan
            {
                App = new App { Data = dest?.App?.Data },
                Accounts = dest?.Accounts?.Select(a => new Account { Name = a.Name })
            };

        public Task<bool> NotificationConsent()
        {
            throw new NotImplementedException();
        }
    }
}
