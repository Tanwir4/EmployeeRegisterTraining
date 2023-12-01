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
                    //   logger.Log("Connected Successfully");
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
        public DataTable GetData(string sql, List<SqlParameter> parameters)
        {
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sql, Connection))
            {
                cmd.Parameters.AddRange(parameters.ToArray());
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                return dt;
            }
        }
    }
}
