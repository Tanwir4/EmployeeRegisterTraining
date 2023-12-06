﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DBConnection
{
    public interface IDataAccessLayer : IDisposable
    {
        SqlConnection Connection { get; }
        string Connect();
        void Disconnect();
        SqlDataReader GetData(string sql, List<SqlParameter> parameters);
        int InsertData(string sql, List<SqlParameter> parameters);
        List<string> GetAll(string sql);
        SqlConnection CreateConnection();
    }
}
