﻿using EtlService.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtlService
{
    internal class ConsoleApplication
    {
        private readonly IConfiguration configuration;
        private CancellationTokenSource cancellationTokenSource;

        public ConsoleApplication()
        {
            configuration = new Configuration.Configuration("./config.json");
            configuration.CreateIfDoesntExistEmptyConfigFile();
        }
        public void Run()
        {
            PrintHelp();
            bool exit = false;
            while (!exit)
            {
                string inputString = GetInputString();
                if (inputString == "exit")
                {
                    exit = true;
                }
                else if (inputString == "start")
                {
                    Start();
                }
                else if(inputString == "stop")
                {
                    Stop();
                }
                else if(inputString == "reload")
                {
                    Reload();
                }
                else
                {
                    Console.Error.WriteLine("Invalid command");
                }
            }
        }
        private void Start()
        {
            bool result = configuration.ResetConfiguration();
            if (!result)
            {
                Console.WriteLine("Configuration laoding failed");
            }
            if (cancellationTokenSource != null && !cancellationTokenSource.IsCancellationRequested)
            {
                Console.WriteLine("Service has already started");
                return;
            }
            cancellationTokenSource = new CancellationTokenSource();
            var controller = new FlowController(configuration);
            Task.Run(() => controller.StartService(cancellationTokenSource.Token));
            Console.WriteLine("Service started");
        }

        private void Stop()
        {
            if(cancellationTokenSource != null && !cancellationTokenSource.IsCancellationRequested)
            {
                Console.WriteLine("Service stopped");
                cancellationTokenSource.Cancel();
            }
            else
            {
                Console.WriteLine("Service can't be stopped");
            }
        }

        private void Reload()
        {
            if (cancellationTokenSource == null || cancellationTokenSource.IsCancellationRequested)
            {
                Console.WriteLine("Service can't be reloaded");
                return;
            }
            Console.WriteLine("Service reloading");
            Stop();
            bool result = configuration.ResetConfiguration();
            if (result)
            {
                Console.WriteLine("Configuration was reloaded");
                Start();
            }
            else
            {
                Console.WriteLine("Configuration relaoding failed");
            }
        }

        private static string GetInputString()
        {
            string? input = Console.ReadLine();
            if (input is null)
            {
                return "";
            }
            return input;
        }

        private static void PrintHelp()
        {
            string[] helpString = new string[]
            {
                "Commands:",
                "start - start service",
                "reload - restart service with updated configuration",
                "stop - stop service",
                "exit - close application"
            };
            foreach (string line in helpString)
            {
                Console.WriteLine(line);
            }
        }
    }
}
