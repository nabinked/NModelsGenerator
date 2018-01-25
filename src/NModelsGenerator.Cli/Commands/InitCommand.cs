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
            NModelsGenerator = new Generator();
        }

        public int Run()
        {
            return NModelsGenerator.Init();
        }

        public INModelsGenerator NModelsGenerator { get; set; }
    }
}
