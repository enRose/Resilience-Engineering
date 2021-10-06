using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using api.Configs;
using Microsoft.Extensions.Options;
using retry.Entities;

namespace api.Services
{
    public interface ICoreBankingService
    {
        Task<IEnumerable<Account>> GetAccounts(string userId);
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
    }
}