using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.CommandLineUtils;
using NModelsGenerator.Common;
using NModelsGenerator.Core;

namespace NModelsGenerator.Cli.Commands
{
    public class RunCommand : ICommand
    {
        private readonly CommandArgument _configArgument;
        private readonly CommandLineOptions _options;

        public RunCommand(CommandArgument configArgument, CommandLineOptions options)
        {
            _configArgument = configArgument;
            _options = options;
        }

        public int Run()
        {
            var currentDirectory = GetCurrentDirectory();
            var config = MiscUtils.GetConfig(_configArgument.Value ?? Path.Combine(currentDirectory, Constants.ConfigFileName));
            var generator = new Generator(new DirectoryInfo(currentDirectory));
            return generator.Run(config);
        }

        private string GetCurrentDirectory()
        {
            return Directory.GetCurrentDirectory();
        }
    }
}
