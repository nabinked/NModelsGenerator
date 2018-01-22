using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.CommandLineUtils;

namespace NModelsGenerator.Cli.Commands
{
    class RootCommand : ICommand
    {
        private readonly CommandLineApplication _app;

        public RootCommand(CommandLineApplication app)
        {
            _app = app;
        }

        public int Run()
        {
            _app.ShowHelp();

            return 1;
        }
    }
}
