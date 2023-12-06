using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using DataAccessLayer.DBConnection;
using EmployeeTrainingRegistrationServices.Entities;
using System.Diagnostics;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer;
        public UserRepository(IDataAccessLayer layer)
        {
            _dataAccessLayer = layer;
        }
        public bool Authenticate(Account user)
        {
            _dataAccessLayer.Connect();
            const string SQL = "SELECT 1 FROM UserAccount WHERE Email=@EmailAddress AND Passwordd=@Password";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@EmailAddress", SqlDbType.VarChar, 100) { Value = user.Email },
                new SqlParameter("@Password", SqlDbType.VarChar, 100) { Value = user.Password } //Change password varchar
            };
            SqlDataReader getData = _dataAccessLayer.GetData(SQL, parameters);
            return (getData.HasRows);
        }
        public bool Register(User user)
        {
            _dataAccessLayer.Connect();
            string sql = $@"
                          DECLARE @AccountTempID INT
                          DECLARE @managerId INT
                          DECLARE @deptId INT
                          INSERT INTO UserAccount (Email, Passwordd,RoleID) VALUES (@Email,@Passwordd,1)

                           SET @AccountTempID = SCOPE_IDENTITY()
                           SET @managerId=(SELECT ManagerID FROM ManagerDetails WHERE ManagerName=@ManagerName)
                           SET @deptId =(SELECT DepartmentID FROM Department WHERE DepartmentName=@DepartmentName)

                          INSERT INTO UserDetails (FirstName,LastName,MobileNumber,NationalIdentityCard,ManagerID,UserAccountID,DepartmentID)
                          VALUES (@FirstName,@LastName,@MobileNumber,@NationalIdentityCard,@managerId, @AccountTempID, @deptId)";
            List<SqlParameter> parameters = new List<SqlParameter>
                   {
                       new SqlParameter("@Email", SqlDbType.VarChar, 100) { Value = user.Email },
                       new SqlParameter("@Passwordd", SqlDbType.VarChar, 100) { Value = user.Password },
                       new SqlParameter("@FirstName", SqlDbType.VarChar, 50) { Value = user.FirstName },
                       new SqlParameter("@LastName", SqlDbType.VarChar, 100) { Value = user.LastName },
                       new SqlParameter("@MobileNumber", SqlDbType.VarChar, 20) { Value = user.MobileNumber },
                       new SqlParameter("@NationalIdentityCard", SqlDbType.VarChar, 15) { Value = user.NationalIdentityCard },
                       new SqlParameter("@ManagerName", SqlDbType.VarChar, 80) { Value = user.ManagerName },
                       new SqlParameter("@DepartmentName", SqlDbType.VarChar, 40) { Value = user.DepartmentName }

                   };
            int numberOfRowsAffected = _dataAccessLayer.InsertData(sql, parameters);
            return (numberOfRowsAffected>0);
        }
    }
}
