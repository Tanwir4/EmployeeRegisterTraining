using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.DBConnection;
using System.Diagnostics;
using EmployeeTrainingRegistrationServices.Entities;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer;
        string result;
        string sql;
        public UserRepository(IDataAccessLayer layer)
        {
            _dataAccessLayer = layer;
        }
        public bool GetUserLogin(UserAccount user)
        {
            try
            {
                result = _dataAccessLayer.Connect();
                sql = "SELECT * FROM TestLogin WHERE EmailAddress=@EmailAddress AND HashedPassword=@Password";

                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@EmailAddress", SqlDbType.VarChar, 100) { Value = user.Email },
                    new SqlParameter("@Password", SqlDbType.VarChar, 100) { Value = user.Password }
                };
                DataTable resultTable = _dataAccessLayer.GetData(sql, parameters);
                Debug.WriteLine(resultTable);
                Debug.WriteLine(sql);
                Debug.WriteLine(user.Email);
                Debug.WriteLine(user.Password);

                return (resultTable.Rows.Count > 0);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error");
                Console.WriteLine(e.Message);
                //throw e;
                return false;
            }



        }

         public bool AddUserAccount(UserDetails user, UserAccount acc, Department dept) {
            result = _dataAccessLayer.Connect();
            sql = $@"BEGIN TRANSACTION
                        DECLARE @AccountTempID INT
                        DECLARE @managerId INT
                        DECLARE @deptId INT
                        INSERT INTO UserAccount (Email, Passwordd,RoleID) VALUES (@Email,@Passwordd,1)
 
                         SET @AccountTempID = SCOPE_IDENTITY()
                         SET @managerId=(SELECT ManagerID FROM ManagerDetails WHERE ManagerName=@ManagerName)
                         SET @deptId =(SELECT DepartmentID FROM Department WHERE DepartmentName=@DepartmentName)
 
                        INSERT INTO UserDetails (FirstName,LastName,MobileNumber,NationalIdentityCard,ManagerID,UserAccountID,DepartmentID)
                        VALUES (@FirstName,@LastName,@MobileNumber,@NationalIdentityCard,@managerId, @AccountTempID, @deptId)
 
                     COMMIT TRANSACTION";
            List<SqlParameter> parameters = new List<SqlParameter>
                 {
                     new SqlParameter("@Email", SqlDbType.VarChar, 100) { Value = acc.Email },
                     new SqlParameter("@Passwordd", SqlDbType.VarChar, 100) { Value = acc.Password },
                     new SqlParameter("@FirstName", SqlDbType.VarChar, 50) { Value = user.FirstName },
                     new SqlParameter("@LastName", SqlDbType.VarChar, 100) { Value = user.LastName },
                     new SqlParameter("@MobileNumber", SqlDbType.VarChar, 20) { Value = user.MobileNumber },
                     new SqlParameter("@NationalIdentityCard", SqlDbType.VarChar, 15) { Value = user.NationalIdentityCard },
                     new SqlParameter("@ManagerName", SqlDbType.VarChar, 80) { Value = user.ManagerName },
                     new SqlParameter("@DepartmentName", SqlDbType.VarChar, 40) { Value = dept.DepartmentName }

                 };
            DataTable resultTable = _dataAccessLayer.GetData(sql, parameters);
            Debug.WriteLine(resultTable);
            Debug.WriteLine(sql);
            Debug.WriteLine(acc.Email);
            Debug.WriteLine(acc.Password);
            return (resultTable.Rows.Count > 0);

    }

   


    }
}
