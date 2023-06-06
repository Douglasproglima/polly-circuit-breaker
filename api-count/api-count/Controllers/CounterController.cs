using api_count.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace api_count.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CounterController : ControllerBase
    {
        private static readonly Counter _COUNTER = new Counter();
        private readonly ILogger<CounterController> _logger;
        private readonly IConfiguration _configuration;

        public CounterController(ILogger<CounterController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public CounterResult Get()
        {
            int valorAtualContador;
            lock (_COUNTER)
            {
                _COUNTER.Incrementar();
                valorAtualContador = _COUNTER.ValorAtual;
            }

            if (valorAtualContador % 4 == 0)
            {
                _logger.LogError("Simulando falha...");
                throw new Exception("Simulação de falha!");
            }

            _logger.LogInformation($"Contador - Valor atual: {valorAtualContador}");

            return new()
            {
                CurrentValue = valorAtualContador,
                Locale = _COUNTER.Local,
                Kernel = _COUNTER.Kernel,
                TargetFwk= _COUNTER.TargetFramework,
                FixeMessage = "Teste",
                VariableMessage = _configuration["MensagemVariavel"]
            };
        }
    }
}