using System;
using System.IO;
using Microsoft.Extensions.CommandLineUtils;
using NModelsGenerator.Common;

namespace NModelsGenerator.Cli
{
    class Program
    {
        static int Main(string[] args)
        {

            var options = CommandLineOptions.Parse(args);

            if (options?.Command == null)
            {
                // RootCommand will have printed help
                return 1;
            }

            if (options.Command.NModelsGenerator != null)
            {
                options.Command.NModelsGenerator.OnLogEvent += CommandOnOnLogEvent;
            }
            return options.Command.Run();

        }

        private static void CommandOnOnLogEvent(string logText)
        {
            Console.WriteLine(logText);
        }
    }
}
