using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.CommandLineUtils;
using NModelsGenerator.Cli.Commands;
using NModelsGenerator.Common;

namespace NModelsGenerator.Cli.CommandConfigurations
{
    public static class RunCommandConfiguration
    {
        public static void Configure(CommandLineApplication command, CommandLineOptions options)
        {
            command.Description = "Run the generator.";
            command.HelpOption("-?|-h|--help");

            var configArgument = command.Argument("config",
                "Config file path");

            command.OnExecute(() =>
            {
                options.Command = new RunCommand(configArgument, options);

                return 0;
            });

        }
    }
}
