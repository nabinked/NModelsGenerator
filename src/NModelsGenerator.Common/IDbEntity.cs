using System.Collections.Generic;

namespace NModelsGenerator.Common
{
    public interface IDbEntity
    {
        List<DbColumn> Columns { get; set; }
        string TableCatalog { get; set; }
        string TableName { get; set; }
        string TableSchema { get; set; }
        string TableType { get; set; }
        bool IsView { get; set; }
    }
}