using EtlService.Configuration;
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

        public ConsoleApplication(IConfiguration configuration)
        {
            this.configuration = configuration;
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
            var dummy = async (CancellationToken token) =>
            {
                while (!token.IsCancellationRequested)
                {
                    Console.WriteLine("Running");
                    await Task.Delay(2000);
                }
            };

            if (cancellationTokenSource != null && !cancellationTokenSource.IsCancellationRequested)
            {
                Console.WriteLine("Service has already started");
                return;
            }
            cancellationTokenSource = new CancellationTokenSource();
            Console.WriteLine("Service started");
            Task.Run(() => dummy(cancellationTokenSource.Token));
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
                "restart - restart service with updated configuration",
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
