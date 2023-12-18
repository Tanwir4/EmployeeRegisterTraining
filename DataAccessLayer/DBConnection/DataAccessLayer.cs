using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Threading.Tasks;
using DataAccessLayer.AppLogger;

namespace DataAccessLayer.DBConnection
{
    public class DataAccessLayer : IDataAccessLayer
    {
        private readonly ILogger _logger;
        public DataAccessLayer(ILogger logger)
        {
            _logger = logger;
        }
        public SqlConnection Connection { get; private set; }
        public SqlConnection CreateConnection()
        {
            try
            {
                var connectionString = ConfigurationManager.AppSettings["DefaultConnectionString"];
                if (string.IsNullOrEmpty(connectionString)) throw new ApplicationException("Connection string empty");              
                Connection = new SqlConnection(connectionString);
                Connection.Open();
                return Connection;               
            }
            catch (Exception exception)
            {
                _logger.LogError(exception);
                throw new ApplicationException("Unable to find the connection string", exception);
            }       
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
            try
            {
                using (SqlCommand cmd = new SqlCommand(sql))
                {
                    cmd.Connection = Connection;
                    cmd.Parameters.AddRange(parameters.ToArray());
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    return reader;
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception);
                throw;
            }
        }
        public async Task<int> InsertDataAsync(string sql, List<SqlParameter> parameters)
        {
            using (SqlCommand cmd = new SqlCommand(sql, Connection))
            {
                cmd.Parameters.AddRange(parameters.ToArray());
                var r=await  cmd.ExecuteNonQueryAsync();
                return r;
            }
        }
        public int InsertData(string sql, List<SqlParameter> parameters)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, Connection))
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception);
                throw;
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
