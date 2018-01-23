using System;
using System.Collections.Generic;
using System.IO;
using DbPortal;
using NModelsGenerator.Common;
using NModelsGenerator.DbPortal;

namespace NModelsGenerator.Core
{
    public partial class Generator : INModelsGenerator
    {
        public event LogEvent OnLogEvent;

        public Generator(DirectoryInfo projectFolder)
        {
            _projectFolder = projectFolder;
            _variables = new Dictionary<string, string>();
            _addedItems = new List<string>();
            _removedItems = new List<string>();
        }

        public int Run(Config config)
        {
            _config = config;
            var dbActions = new DbActions(config.ConnectionString, config.DbType);
            Log("Getting schema information from database. This may take some time depending upon the schema size.");
            var schemas = dbActions.GetAllSchemas(config.Schemas);

            GenerateClasses(schemas);
            return 0;
        }

        public int Init()
        {
            try
            {
                AddConfigJsonFile(_projectFolder.FullName, Constants.ConfigFileName);
                return 0;
            }
            catch (Exception e)
            {
                return 1;
            }
        }

        public IEnumerable<string> GetAddedItems()
        {
            return _addedItems;
        }

        public string GetConfigFilePath()
        {
            return Path.Combine(_projectFolder.FullName, Constants.ConfigFileName);
        }

        public IEnumerable<string> GetRemovedItems()
        {
            return _removedItems;
        }
    }

}
