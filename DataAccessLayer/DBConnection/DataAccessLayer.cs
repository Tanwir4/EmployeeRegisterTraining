using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DataAccessLayer.DBConnection
{
    public class DataAccessLayer : IDataAccessLayer
    {
        public SqlConnection Connection { get; private set; }
        public string Connect()
        {
            try
            {
                var connectionString = ConfigurationManager.AppSettings["DefaultConnectionString"];
                if (!string.IsNullOrEmpty(connectionString))
                {
                    Connection = new SqlConnection(connectionString);
                    Connection.Open();
                    //logger.Log("Connected Successfully");
                }
            }
            catch (Exception ex)
            {
                //  logger.Log("Fail to connect");
                return "Unable to find the connection string " + ex.Message;
            }
            return "DB Connect: OK";
        }
        public void Disconnect()
        {
            if (Connection != null && Connection.State.Equals(ConnectionState.Open))
            {
                Connection.Close();
            }
        }
        public SqlDataReader GetData(string sql, List<SqlParameter> parameters)
        {
            using (SqlCommand cmd = new SqlCommand(sql))
            {
                cmd.Connection= Connection;
                cmd.Parameters.AddRange(parameters.ToArray());
                // Execute the query and return the SqlDataReader
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
                // Execute the query and return the SqlDataReader
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
                        // Assuming the data you want is in the first column of each row
                        string result = reader[0].ToString();
                        resultList.Add(result);
                    }
                }
            }
            return resultList;
        }

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

    }
}
