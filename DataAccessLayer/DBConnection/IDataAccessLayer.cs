using System;
using System.Collections.Generic;
using System.Data.SqlClient;
namespace DataAccessLayer.DBConnection
{
    public interface IDataAccessLayer : IDisposable
    {
        SqlConnection Connection { get; }
        SqlConnection CreateConnection();
        List<string> GetAll(string sql);
        SqlDataReader GetDataWithConditions(string sql, List<SqlParameter> parameters);
        int InsertData(string sql, List<SqlParameter> parameters);
    }
}
