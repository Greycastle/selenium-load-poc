using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace SeleniumLoadPoc.Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            var arguments = Arguments.Parse(args);
            if (!arguments.Valid)
            {
                return;
            }

            Console.WriteLine($"Will generate load from {arguments.ClientCount} clients using container {arguments.DockerName}..");
            var loader = new Loader();
            loader.Generate(arguments.ClientCount, arguments.DockerName, arguments.DockerArgs);
        }
    }

    internal class Loader
    {
        public void Generate(int clients, string dockerName, string dockerArgs)
        {
            dockerArgs = dockerArgs ?? "";
            var processes = new List<Process>();
            for (int i = 0; i < clients; ++i)
            {
                var count = i + 1;
                var process = Process.Start("docker", $"run {dockerArgs} {dockerName}");
                Console.WriteLine($"Process {count} started!");
                processes.Add(process);
                process.Exited += (sender, args) =>
                {
                    if (process.ExitCode != 0)
                    {
                        Console.WriteLine($"Process {count} exited with code: " + process.ExitCode);
                    }
                    else
                    {
                        Console.WriteLine($"Process {count} finished!");
                    }

                };
            }

            while (processes.Any(p => !p.HasExited))
            {
                Thread.Sleep(1000);
            }
        }
    }
}
