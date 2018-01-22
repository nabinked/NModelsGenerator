using System.Collections.Generic;

namespace NModelsGenerator.Common
{
    public class DbTable
    {
        public string TableCatalog { get; set; }
        public string TableSchema { get; set; }
        public string TableName { get; set; }
        public string TableType { get; set; }
        public List<DbColumn> Columns { get; set; }
    }
}
