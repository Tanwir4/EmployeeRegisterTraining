using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DBConnection
{
    public interface IDataAccessLayer
    {
        SqlConnection Connection { get; }
        string Connect();
        void Disconnect();
        DataTable GetData(string sql, List<SqlParameter> parameters);
    }
}
