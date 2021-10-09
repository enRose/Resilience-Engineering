using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using api.Configs;
using Microsoft.Extensions.Options;
using Polly.Contrib.Simmy;
using Polly.Contrib.Simmy.Outcomes;
using api.Entities;

namespace api.Services
{
    public interface ICoreBankingService
    {
        Task<IEnumerable<Account>> GetAccounts(string userId);
        Task<bool> ApplyFor(PersonalLoan personalLoan);
    }

    public class CoreBankingService : ICoreBankingService
    {
        private readonly HttpClient _client;
        private readonly CoreBankingApiSettings settings;

        public CoreBankingService(
            HttpClient client,
            IOptions<CoreBankingApiSettings> options)
        {
            settings = options.Value;
            _client = client;
        }

        public async Task<IEnumerable<Account>> GetAccounts(string userId)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                settings.HostUri);

            request.Headers.Add("Accept", "application/vnd.github.v3+json");

            request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

            var response = await _client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();

                return await JsonSerializer.DeserializeAsync
                    <IEnumerable<Account>>(responseStream);
            }

            return Array.Empty<Account>();
        }

        public async Task<bool> ApplyFor(PersonalLoan personalLoan)
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
                $"\n{JsonSerializer.Serialize(personalLoan)}\n"
                )
            );

            return true;
        }
    }
}