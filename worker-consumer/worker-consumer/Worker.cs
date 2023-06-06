using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using worker_consumer.Models;
using Polly.CircuitBreaker;

namespace worker_consumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly AsyncCircuitBreakerPolicy _circuitBreaker;

        public Worker(ILogger<Worker> logger, IConfiguration configuration, AsyncCircuitBreakerPolicy circuitBreaker)
        {
            _logger = logger;
            _configuration = configuration;
            _circuitBreaker = circuitBreaker;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var httpClient = new HttpClient();
            var urlApiContagem = _configuration["UrlApiCounter"];

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var resultado = await _circuitBreaker.ExecuteAsync<CounterResult>(() =>
                    {
                        return httpClient.GetFromJsonAsync<CounterResult>(urlApiContagem);
                    });

                    _logger.LogInformation($"* {DateTime.Now:HH:mm:ss} * " +
                        $"Circuito = {_circuitBreaker.CircuitState} | " +
                        $"Contador = {resultado.CurrentValue} | " +
                        $"Mensagem = {resultado.VariableMessage}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"# {DateTime.Now:HH:mm:ss} # " +
                        $"Circuito = {_circuitBreaker.CircuitState} | " +
                        $"Falha ao invocar a API: {ex.GetType().FullName} | {ex.Message}");
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
