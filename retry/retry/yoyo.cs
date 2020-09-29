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

namespace retry
{
    public interface IYoyo
    {
        Task<bool> ConsumerRetryWriteGitHubBranch(IEnumerable<GitHubBranch> gitHubBranches);
        Task<IEnumerable<GitHubBranch>> GetGitHubBranch();
        Task<bool> WriteGitHubBranch();
        Task<bool> AgreeToRetry();   
    }

    public class Yoyo : IYoyo
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IKafkaProducer kafkaProducer;
        
        public Yoyo(IHttpClientFactory h, IKafkaProducer k)
        {
            _httpClientFactory = h;
            kafkaProducer = k;
        }

        public async Task<bool> WriteGitHubBranch()
        {
            Logger.LogThreadInfo("Start Yoyo WriteGitHubBranch()");

            var githubBrances = await GetGitHubBranch();

            return await WriteWithChaos(githubBrances);
        }

        public async Task<bool> AgreeToRetry()
        {
            var githubBrances = await GetGitHubBranch();

            kafkaProducer.Produce(Guid.NewGuid().ToString(), githubBrances);

            return true;
        }

        public async Task<bool> ConsumerRetryWriteGitHubBranch(
            IEnumerable<GitHubBranch> gitHubBranches)
        {
            Logger.LogThreadInfo("Start Yoyo ConsumerRetryWriteGitHubBranch()");

            return await WriteWithChaos(gitHubBranches);
        }

        public async Task<bool> WriteWithChaos(IEnumerable<GitHubBranch> gitHubBranches)
        {
            var fault = new IOException();

            var chaos = MonkeyPolicy
                .InjectExceptionAsync(with =>
                    with.Fault(fault)
                    .InjectionRate(1)
                    .Enabled()
                );

            var path = Path.Combine(Environment
                .GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "Projects/try-stuff/retry/gitHubBranches.txt");

            using StreamWriter outputFile = new StreamWriter(path, true);

            await chaos.ExecuteAsync(async () =>
                await outputFile.WriteAsync(
                $"\n{JsonSerializer.Serialize(gitHubBranches)}\n"
                )
            );

            return true;
        }

        public async Task<IEnumerable<GitHubBranch>> GetGitHubBranch()
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

                return await JsonSerializer.DeserializeAsync
                    <IEnumerable<GitHubBranch>>(responseStream);
            }
            else
            {
                return Array.Empty<GitHubBranch>();
            }
        }
    }

    public class GitHubBranch
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
