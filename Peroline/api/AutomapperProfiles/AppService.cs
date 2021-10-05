using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Polly.Contrib.Simmy;
using Polly.Contrib.Simmy.Outcomes;
using retry.ViewModels;

namespace retry.Services
{
    public interface IAppService
    {
        Task<PersonalLoanViewModel> GetApp();
        Task<bool> SubmitApp();
        Task<bool> AgreeToRetry();
        Task<bool> ConsumerRetry(PersonalLoanViewModel app);
    }

    public class AppService : IAppService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IKafkaProducer kafkaProducer;
        
        public AppService(IHttpClientFactory h, IKafkaProducer k)
        {
            _httpClientFactory = h;
            kafkaProducer = k;
        }

        public async Task<PersonalLoanViewModel> GetApp()
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                "https://api.github.com/repos/aspnet/AspNetCore.Docs/branches");

            request.Headers.Add("Accept", "application/vnd.github.v3+json");

            request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

            var client = _httpClientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();

                var r =  await JsonSerializer.DeserializeAsync
                    <IEnumerable<Content>>(responseStream);

                return new PersonalLoanViewModel()
                {
                    App = r
                };
            }
            
            return new PersonalLoanViewModel
            {
                App = Array.Empty<Content>()
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
            PersonalLoanViewModel app)
        {
            return await InvokeDownstreamAPI(app);
        }

        public async Task<bool> InvokeDownstreamAPI(PersonalLoanViewModel app)
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
