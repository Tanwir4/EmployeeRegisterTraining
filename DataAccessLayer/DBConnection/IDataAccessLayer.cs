using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DataAccessLayer.DBConnection
{
    public interface IDataAccessLayer : IDisposable
    {
        SqlConnection Connection { get; }
        SqlConnection CreateConnection();
        List<string> GetAll(string sql);
        SqlDataReader GetDataWithConditions(string sql, List<SqlParameter> parameters);
        Task<int> InsertDataAsync(string sql, List<SqlParameter> parameters);
        int InsertData(string sql, List<SqlParameter> parameters);
    }
}
