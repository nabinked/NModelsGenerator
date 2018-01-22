
using System.Collections.Generic;
using System.Dynamic;

namespace NModelsGenerator.Common
{
    public class Config
    {
        public string ConnectionString { get; set; }
        public DbTypes DbType { get; set; }

        public ExpandoObject ClassAttributes { get; set; }
        public List<string> BaseClassTypes { get; set; }
        public List<string> BaseClassProperties { get; set; }
        public List<string> Imports { get; set; }
        public string Schemas { get; set; }
        public string RootNameSpace { get; set; }
        public string NameSpaceSuffix { get; set; }
        public bool SchemaBasedFolders { get; set; }

        public static Config Defaults
        {
            get
            {
                dynamic obj = new ExpandoObject();
                obj.MyClassAttribute = "Class attributes in object notations";
                return new Config()
                {
                    ConnectionString = "Connection String to your database",
                    ClassAttributes = obj,
                    DbType = DbTypes.PostgreSql,
                    BaseClassTypes = new List<string>() { "List", "of", "BaseClasses", "and", "interfaces" },
                    BaseClassProperties = new List<string>() { "List", "of", "BaseClasses", "properties" },
                    Imports = new List<string>() { "List", "of", "using directives" },
                    SchemaBasedFolders = false,
                    NameSpaceSuffix = "A string for namespace suffix",
                    Schemas = "Comma separated string for specifying schemas to include. If empty all schemas included"
                };
            }
        }
    }
}
