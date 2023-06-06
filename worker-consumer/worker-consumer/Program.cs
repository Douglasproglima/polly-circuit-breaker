using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using worker_consumer.Resilience;

namespace worker_consumer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton(CircuitBreaker.CreatePolicy());
                    services.AddHostedService<Worker>();
                });

    }
}
