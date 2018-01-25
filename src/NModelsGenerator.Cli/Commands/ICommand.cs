using NModelsGenerator.Common;

namespace NModelsGenerator.Cli.Commands
{
    public interface ICommand
    {
        int Run();
        INModelsGenerator NModelsGenerator { get; set; }
    }
}