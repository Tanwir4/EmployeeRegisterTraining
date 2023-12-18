using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
namespace DataAccessLayer.DBConnection
{
    public class DataAccessLayer : IDataAccessLayer
    {
        public SqlConnection Connection { get; private set; }
        public SqlConnection CreateConnection()
        {
            try
            {
                var connectionString = ConfigurationManager.AppSettings["DefaultConnectionString"];
                if (!string.IsNullOrEmpty(connectionString))
                {
                    Connection = new SqlConnection(connectionString);
                    Connection.Open();
                    return Connection;
                }
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Unable to find the connection string", exception);
            }
            throw new ApplicationException("Unable to find the connection string");
        }
        public void Dispose()
        {
            if (Connection != null && Connection.State.Equals(ConnectionState.Open))
            {
                Connection.Close();
                Connection.Dispose();
            }
        }
        public SqlDataReader GetDataWithConditions(string sql, List<SqlParameter> parameters)
        {
            using (SqlCommand cmd = new SqlCommand(sql))
            {
                cmd.Connection= Connection;
                cmd.Parameters.AddRange(parameters.ToArray());
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
        }
        public int InsertData(string sql, List<SqlParameter> parameters)
        {
            using (SqlCommand cmd = new SqlCommand(sql))
            {
                cmd.Connection = Connection;
                cmd.Parameters.AddRange(parameters.ToArray());
                return cmd.ExecuteNonQuery();
            }
        }
        public List<string> GetAll(string sql)
        {
            List<string> resultList = new List<string>();

            using (SqlCommand cmd = new SqlCommand(sql))
            {
                cmd.Connection = Connection;
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        string result = reader[0].ToString();
                        resultList.Add(result);
                    }
                }
            }
            return resultList;
        }
    }
}
