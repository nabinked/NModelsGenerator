using NModelsGenerator.Common;

namespace DbPortal
{
    public class SqlStatements
    {
        /// <summary>
        /// returns the query to get schemas
        /// </summary>
        /// <param name="dbTypes">database type</param>
        /// <returns></returns>
        public static string GetAllSchemas(DbTypes dbTypes)
        {
            switch (dbTypes)
            {
                case DbTypes.PostgreSql:
                    return @"SELECT schema_name as SchemaName FROM information_schema.schemata WHERE schema_name NOT IN ('pg_toast', 'pg_temp_1', 'pg_toast_temp_1', 'pg_catalog', 'public', 'information_schema');";
                case DbTypes.MySql:
                    return "";
                case DbTypes.MsSql:
                    return @"SELECT SCHEMA_NAME as SchemaName FROM INFORMATION_SCHEMA.SCHEMATA";
                default:
                    return "";
            }
        }

        public static string GetAllTables(DbTypes dbTypes)
        {
            switch (dbTypes)
            {
                case DbTypes.PostgreSql:
                    return @"SELECT table_catalog as TableCatalog, table_schema as TableSchema, table_name as TableName, table_type as TableType FROM information_schema.tables WHERE table_type = 'BASE TABLE' AND table_catalog = @DbName AND table_schema = @Schema;";
                case DbTypes.MySql:
                    return @"";
                case DbTypes.MsSql:
                    return @"SELECT Table_Catalog as TableCatalog, Table_Schema as TableSchema, Table_Name as TableName, Table_Type as TableType FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' AND TABLE_SCHEMA = @Schema AND TABLE_CATALOG = @DbName;";
                default:
                    return "";
            }
        }

        public static string GetAllViews(DbTypes dbTypes)
        {
            switch (dbTypes)
            {
                case DbTypes.PostgreSql:
                    return @"SELECT table_catalog as TableCatalog, table_schema as TableSchema, table_name as TableName, view_definition as ViewDefinition FROM information_schema.views WHERE table_catalog = @DbName AND table_schema = @Schema;";
                case DbTypes.MySql:
                    return @"";
                case DbTypes.MsSql:
                    return @"SELECT table_catalog as TableCatalog, table_schema as TableSchema, table_name as TableName, view_definition as ViewDefinition FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = @Schema AND TABLE_CATALOG = @DbName;";
                default:
                    return "";
            }
        }

        public static string GetAllColumns(DbTypes dbTypes)
        {
            switch (dbTypes)
            {
                case DbTypes.PostgreSql:
                    return @"SELECT    column_name as ColumnName, 
                                                        is_nullable as IsNullable, 
                                                        data_type as DataType, 
                                                        character_maximum_length as CharacterMaxLength
                                                        FROM information_schema.columns WHERE table_catalog = @DbName AND table_schema = @Schema AND table_name = @TableName;";
                case DbTypes.MySql:
                    return @"";
                case DbTypes.MsSql:
                    return @"SELECT    column_name as ColumnName, 
                                                        is_nullable as IsNullable, 
                                                        data_type as DataType, 
                                                        character_maximum_length as CharacterMaxLength
                                                        FROM information_schema.columns WHERE table_catalog = @DbName AND table_schema = @Schema AND table_name = @TableName;";
                default:
                    return "";
            }
        }
    }
}
