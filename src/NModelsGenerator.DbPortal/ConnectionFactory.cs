using System;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using NModelsGenerator.Common;
using Npgsql;

namespace DbPortal
{
    public class ConnectionFactory
    {

        public static IDbConnection CreateConnection(string connectionString, DbTypes dbType)
        {
            IDbConnection con = null;
            switch (dbType)
            {
                case DbTypes.PostgreSql:
                    con = new NpgsqlConnection(connectionString);
                    break;
                case DbTypes.MySql:
                    con = new MySqlConnection(connectionString);
                    break;
                case DbTypes.MsSql:
                    con = new SqlConnection(connectionString);
                    break;
            }
            try
            {
                con?.Open();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return con;


        }


    }
}
