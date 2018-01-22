using System.Collections.Generic;
using System.Dynamic;

namespace NModelsGenerator.Common
{
    public class DbSchema
    {
        public List<IDbEntity> Entities { get; set; }
        public string Owner { get; set; }
        public string SchemaName { get; set; }
    }
}
