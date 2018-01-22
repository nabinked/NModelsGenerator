using System;
using System.ComponentModel;

namespace NModelsGenerator.Common
{
    public class DbColumn
    {
        public string ColumnName { get; set; }

        public string DataType { get; set; }

        public bool IsRequired => IsNullable == "NO";

        public string IsNullable { get; set; }

        public int CharacterMaxLength { get; set; }

    }
}
