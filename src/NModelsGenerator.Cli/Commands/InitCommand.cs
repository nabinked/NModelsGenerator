using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NModelsGenerator.Common;
using NModelsGenerator.Core;

namespace NModelsGenerator.Cli.Commands
{
    public class InitCommand : ICommand
    {
        private readonly CommandLineOptions _options;

        public InitCommand(CommandLineOptions options)
        {
            _options = options;
        }

        public int Run()
        {
            var currentDirectory = GetCurrentDirectory();
            var generator = new Generator(new DirectoryInfo(currentDirectory));
            return generator.Init();
        }

        private string GetCurrentDirectory()
        {
            return Directory.GetCurrentDirectory();
        }
    }
}
