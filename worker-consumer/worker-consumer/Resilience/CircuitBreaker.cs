﻿using Polly;
using Polly.CircuitBreaker;
using System;

namespace worker_consumer.Resilience
{
    public static class CircuitBreaker
    {
        private static readonly int
            _NUMBER_ERRORS = 5,
            _INTERVALS_REQUEST = 10;
            

        public static AsyncCircuitBreakerPolicy CreatePolicy()
        {
            return Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(_NUMBER_ERRORS, TimeSpan.FromSeconds(_INTERVALS_REQUEST),
                    onBreak: (_, _) =>
                    {
                        ShowCircuitState("Open (onBreak)", ConsoleColor.Red);
                    },
                    onReset: () =>
                    {
                        ShowCircuitState("Closed (onReset)", ConsoleColor.Green);
                    },
                    onHalfOpen: () =>
                    {
                        ShowCircuitState("Half Open (onHalfOpen)", ConsoleColor.Yellow);
                    });
        }

        private static void ShowCircuitState(string descStatus, ConsoleColor backgroundColor)
        {
            var previousBackgroundColor = Console.BackgroundColor;
            var previousForegroundColor = Console.ForegroundColor;

            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = ConsoleColor.Black;

            Console.Out.WriteLine($" ***** Estado do Circuito: {descStatus} **** ");

            Console.BackgroundColor = previousBackgroundColor;
            Console.ForegroundColor = previousForegroundColor;
        }
    }
}
