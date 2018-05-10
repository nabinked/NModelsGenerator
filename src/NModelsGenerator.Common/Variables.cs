using System.Collections.Generic;

namespace NModelsGenerator.Common
{
    public static class Variables
    {
        public const string TableName = "$tableName";
        public const string SchemaName = "$schemaName";

        public static string ReplaceVariables(this string value, IDictionary<string, string> variables)
        {
            foreach (var variable in variables)
            {
                value = value.Replace(variable.Key, variable.Value);
            }
            return value;
        }
    }
}
