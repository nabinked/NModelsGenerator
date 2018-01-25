using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.CommandLineUtils;
using NModelsGenerator.Common;
using NModelsGenerator.Core;

namespace NModelsGenerator.Cli.Commands
{
    class RootCommand : ICommand
    {
        private readonly CommandLineApplication _app;

        public RootCommand(CommandLineApplication app)
        {
            _app = app;
            NModelsGenerator = new Generator();
        }

        public int Run()
        {
            _app.ShowHelp();

            return 1;
        }

        public INModelsGenerator NModelsGenerator { get; set; }
    }
}
