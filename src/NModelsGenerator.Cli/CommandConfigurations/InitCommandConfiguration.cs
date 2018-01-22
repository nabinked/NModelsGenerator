using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.CommandLineUtils;
using NModelsGenerator.Cli.Commands;
using NModelsGenerator.Common;

namespace NModelsGenerator.Cli.CommandConfigurations
{
    public static class InitCommandConfiguration
    {
        public static void Configure(CommandLineApplication command, CommandLineOptions options)
        {
            command.Description = "Initialize by adding a json config file to the project.";
            command.HelpOption("-?|-h|--help");

            command.OnExecute(() =>
            {
                options.Command = new InitCommand(options);

                return 0;
            });

        }
    }
}
