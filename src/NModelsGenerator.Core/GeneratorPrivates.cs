using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using DbPortal;
using Newtonsoft.Json;
using NModelsGenerator.Common;

namespace NModelsGenerator.Core
{
    partial class Generator
    {
        private readonly IDictionary<string, string> _variables;
        private Config _config;
        private readonly DirectoryInfo _projectFolder;
        private const string ViewFolderName = "Views";
        private readonly IList<string> _addedItems;
        private readonly IList<string> _removedItems;

        private void GenerateClasses(IEnumerable<DbSchema> schemas)
        {
            foreach (var schema in schemas)
            {
                Log($"Generating models for schema {schema}");
                _variables[Variables.SchemaName] = schema.SchemaName;
                GenerateModels(schema.Entities);
            }
        }
        private void GenerateModels(IList<IDbEntity> entities)
        {
            //Generate Entities
            foreach (var entity in entities)
            {
                GenerateModelInProject(entity);
            }
        }

        private void GenerateModelInProject(IDbEntity entity)
        {
            //Generate Entities
            Log($"Creating class for {entity.TableName}");

            //Set the variables
            _variables[Variables.TableName] = entity.TableName;

            var nameSpace = GetNameSpaceName(entity);
            var className = GetClassName(entity.TableName);
            var classGenerator = new ClassWriter(nameSpace, className, baseTypeList: _config.BaseClassTypes,
                                                    imports: _config.Imports, classAttributes: _config.ClassAttributes,
                                                    variables: _variables, baseClassProperties: _config.BaseClassProperties);

            var filePath = Path.Combine(GetFolderPathForEntity(entity), className + ".cs");
            classGenerator.Start(entity.Columns, filePath);
            _addedItems.Add(filePath);

        }
        private string GetFolderPathForEntity(IDbEntity entity)
        {
            var schemaFolderName = entity.TableSchema.ToPasacalCase();
            var folderPathForEntity = _projectFolder;
            if (_config.SchemaBasedFolders)
            {
                folderPathForEntity = Directory.CreateDirectory(Path.Combine(_projectFolder.FullName, schemaFolderName));
            }
            if (entity.IsView)
            {
                folderPathForEntity = Directory.CreateDirectory(Path.Combine(folderPathForEntity.FullName, ViewFolderName));
            }
            return folderPathForEntity.FullName;

        }
        private string GetClassName(string tableName)
        {
            return
                new PluralizationServiceInstance().Singularize(tableName.ToPasacalCase());
        }
        private string GetNameSpaceName(IDbEntity entity)
        {
            var dotSeperatedNamespaceList = new List<string>();
            if (!string.IsNullOrWhiteSpace(_config.RootNameSpace))
            {
                dotSeperatedNamespaceList.Add(_config.RootNameSpace.ReplaceVariables(_variables));
            }
            if (!string.IsNullOrWhiteSpace(_config.NameSpaceSuffix))
            {
                dotSeperatedNamespaceList.Add(_config.RootNameSpace.ReplaceVariables(_variables));
            }
            if (entity.IsView)
            {
                dotSeperatedNamespaceList.Add("Views");

            }
            return string.Join(".", dotSeperatedNamespaceList);
        }

        private static string AddConfigJsonFile(string projectRootFolder, string configFileName)
        {
            var configFilePath = Path.Combine(projectRootFolder, configFileName);
            using (var streamWriter = File.CreateText(configFilePath))
            {
                var serializer = new JsonSerializer();
                dynamic obj = new ExpandoObject();
                obj.MyClassAttribute = "Class attributes in object notations";
                serializer.Serialize(streamWriter, new Config()
                {
                    ConnectionString = "Connection String to your database",
                    ClassAttributes = obj,
                    DbType = DbTypes.PostgreSql,
                    BaseClassTypes = new List<string>() { "List", "of", "BaseClasses", "and", "interfaces" },
                    Imports = new List<string>() { "List", "of", "using directives" }
                });
            }
            return configFilePath;
        }


        private void Log(string logtext)
        {
            OnLogEvent?.Invoke(logtext);
        }
    }
}
