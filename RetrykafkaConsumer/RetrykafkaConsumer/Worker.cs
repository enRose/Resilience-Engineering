using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace RetrykafkaConsumer
{
    // ref:
    // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-3.1&tabs=visual-studio-mac

    public class Worker : BackgroundService
    {
        public IServiceProvider Services { get; }

        private readonly ILogger<Worker> _logger;

        public Worker(
            IServiceProvider services,
            ILogger<Worker> logger)
        {
            Services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Logger.LogThreadInfo($"Worker ExecuteAsync()");

            using var scope = Services.CreateScope();

            var kafkaConsumer = scope
                .ServiceProvider
                .GetRequiredService<IKafkaConsumer>();

            await Task.Run(() => kafkaConsumer.Consume(stoppingToken));
        }
    }
}
