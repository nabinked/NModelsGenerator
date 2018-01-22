using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.CommandLineUtils;
using NModelsGenerator.Cli.CommandConfigurations;
using NModelsGenerator.Cli.Commands;

namespace NModelsGenerator.Cli
{
    public class CommandLineOptions
    {

        public static CommandLineOptions Parse(string[] args)
        {
            var options = new CommandLineOptions();

            var app = new CommandLineApplication();

            var isQuietOption = app.Option("--extra-quiet|-q",
                                           "Instruct the ninja to do its best to be even more quiet",
                                           CommandOptionType.NoValue);

            RootCommandConfiguration.Configure(app, options);

            options.IsQuiet = isQuietOption.HasValue();

            var result = app.Execute(args);

            if (result != 0 || options.Command == null)
            {
                Console.Error.WriteLine("Fail");
                return null;
            }

            return options;
        }

        public ICommand Command { get; set; }
        public bool IsQuiet { get; set; }

    }
}
