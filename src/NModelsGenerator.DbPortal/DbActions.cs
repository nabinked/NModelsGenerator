using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using DbPortal;
using NModelsGenerator.Common;

namespace NModelsGenerator.DbPortal
{
    public class DbActions
    {
        private readonly string _connectionString;
        private readonly DbTypes _dbType;

        public DbActions(string connectionString, DbTypes dbType)
        {
            _connectionString = connectionString;
            _dbType = dbType;
        }

        private IDbConnection GetConnection()
        {
            return ConnectionFactory.CreateConnection(_connectionString, _dbType);
        }

        public List<DbSchema> GetAllSchemas(string schemaNames)
        {

            List<DbSchema> schemas = new List<DbSchema>();
            using (var con = GetConnection())
            {

                schemas = string.IsNullOrWhiteSpace(schemaNames) ? con.Query<DbSchema>(SqlStatements.GetAllSchemas(_dbType)).ToList() : SchemaNamesToDbSchema(schemaNames, schemas);
                foreach (var schema in schemas)
                {
                    //Entities
                    schema.Entities = GetAllEntities(schema).ToList();
                    //Columns
                    foreach (var entity in schema.Entities)
                    {
                        entity.Columns = GetAllColumns(entity);
                    }
                }
            }
            return schemas;

        }

        private IEnumerable<IDbEntity> GetAllEntities(DbSchema schema)
        {
            var entities = new List<IDbEntity>();
            entities.AddRange(GetAllTables(schema));
            entities.AddRange(GetAllViews(schema));
            return entities;

        }

        private List<DbSchema> SchemaNamesToDbSchema(string schemaNames, List<DbSchema> schemas)
        {
            var schemaList = schemaNames.Split(',');
            foreach (var s in schemaList)
            {
                schemas.Add(new DbSchema()
                {
                    SchemaName = s.Trim()
                });
            }
            return schemas;
        }

        private List<IDbEntity> GetAllViews(DbSchema schema, IDbConnection conn = null)
        {
            List<IDbEntity> views;
            using (var con = conn ?? GetConnection())
            {
                views = con.Query<DbEntity>(SqlStatements.GetAllViews(_dbType), new { DbName = con.Database, Schema = schema.SchemaName }).ToList<IDbEntity>();
            }
            foreach (var dbEntity in views)
            {
                dbEntity.IsView = true;
            }
            return views;
        }

        public List<IDbEntity> GetAllTables(DbSchema schema, IDbConnection conn = null)
        {
            using (var con = conn ?? GetConnection())
            {
                return con.Query<DbEntity>(SqlStatements.GetAllTables(_dbType), new { DbName = con.Database, Schema = schema.SchemaName }).ToList<IDbEntity>();
            }

        }

        public List<DbColumn> GetAllColumns(IDbEntity entity, IDbConnection conn = null)
        {
            using (var con = conn ?? GetConnection())
            {
                return con.Query<DbColumn>(SqlStatements.GetAllColumns(_dbType), new { DbName = con.Database, Schema = entity.TableSchema, entity.TableName }).ToList();
            }

        }




    }
}