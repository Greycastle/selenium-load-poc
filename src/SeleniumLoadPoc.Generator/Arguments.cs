using System;
using System.Collections.Generic;
using System.Linq;

namespace SeleniumLoadPoc.Generator
{
    public class Arguments
    {
        private Arguments(int clients, string dockerName, string dockerArgs)
        {
            ClientCount = clients;
            DockerName = dockerName;
            DockerArgs = dockerArgs;
            Valid = true;
        }

        private Arguments()
        {
            Valid = false;
        }

        public static Arguments Parse(IEnumerable<string> arguments)
        {
            var args = arguments.ToList();
            if (args.Count != 4 && args.Count != 6)
            {
                WriteError("Invalid number of arguments");
                return new Arguments();
            }

            var dockerArgs = TryParseDockerArgs(args);

            if (ParseClients(args, out int clients) && ParseDockerName(args, out string dockerName))
            {
                return new Arguments(clients, dockerName, dockerArgs);
            }

            return new Arguments();
        }

        private static string TryParseDockerArgs(List<string> args)
        {
            var argumentPosition = args.IndexOf("--docker-args");
            if (argumentPosition == -1)
            {
                return null;
            }

            return args[argumentPosition + 1].Trim('"', '\'');
        }

        private static bool ParseDockerName(List<string> args, out string dockerName)
        {
            var argumentPosition = args.IndexOf("--docker-name");
            if (argumentPosition == -1)
            {
                dockerName = null;
                return false;
            }

            dockerName = args[argumentPosition + 1];
            return true;
        }

        private static bool ParseClients(List<string> args, out int clients)
        {
            var argumentPosition = args.IndexOf("--clients");
            if (argumentPosition == -1)
            {
                clients = 0;
                return false;
            }

            clients = int.Parse(args[argumentPosition + 1]);
            return true;
        }

        private static void WriteError(string message)
        {
            Console.WriteLine($"Error: {message}");
            Console.WriteLine("Usage: dotnet run --clients <number> --docker-name <name> [--docker-args '<args>']");
        }

        public int ClientCount { get; }
        public bool Valid { get; }

        public string DockerName { get; }
        public string DockerArgs { get; }
    }
}
