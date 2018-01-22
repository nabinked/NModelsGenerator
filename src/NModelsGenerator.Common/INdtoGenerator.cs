using System.Collections.Generic;

namespace NModelsGenerator.Common
{
    public interface INModelsGenerator
    {
        int Run(Config config);
        int Init();
        IEnumerable<string> GetAddedItems();
        string GetConfigFilePath();
        IEnumerable<string> GetRemovedItems();
        event LogEvent OnLogEvent;
    }
}
