﻿using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using DataAccessLayer.DBConnection;
using System.Net;
using System.Web;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer;
        public UserRepository(IDataAccessLayer layer)
        {
            _dataAccessLayer = layer;
        }

        public string GetRoleByEmail(string email)
        {
            string role = null;
            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                string SQL = $@"SELECT RoleName
                                FROM UserRole
                                INNER JOIN UserAccount ON UserAccount.RoleID = UserRole.RoleID
                                WHERE Email=@Email;";
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                new SqlParameter("@Email", SqlDbType.VarChar, 100) { Value = email }
                };
                using (SqlDataReader reader = _dataAccessLayer.GetDataWithConditions(SQL, parameters))
                {
                    if (reader.Read())
                    {
                        role = reader.GetString(reader.GetOrdinal("RoleName"));
                    }
                }
            }
            return role;
        }

        public bool Authenticate(Account user)
        {
            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                const string SQL = "SELECT 1 FROM UserAccount WHERE Email=@EmailAddress AND Passwordd=@Password";
                List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@EmailAddress", SqlDbType.VarChar, 100) { Value = user.Email },
                new SqlParameter("@Password", SqlDbType.VarChar, 100) { Value = user.Password }
            };
                SqlDataReader getData = _dataAccessLayer.GetDataWithConditions(SQL, parameters);
                return (getData.HasRows);
            }
        }
        public async Task<bool> Register(User user)
        {
            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
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
                int numberOfRowsAffected = await _dataAccessLayer.InsertDataAsync(sql, parameters);
                return (numberOfRowsAffected > 0);
            }
        }
        public int GetUserAccountIdByEmail(string email)
        {
            int userAccountId = -1;
            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                string SQL = $@"SELECT UserAccountID
                                FROM UserAccount
                                WHERE Email=@Email";
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                new SqlParameter("@Email", SqlDbType.VarChar, 100) { Value = email }
                };
                using (SqlDataReader reader = _dataAccessLayer.GetDataWithConditions(SQL, parameters))
                {
                    if (reader.Read())
                    {
                        userAccountId = reader.GetInt32(reader.GetOrdinal("UserAccountID"));
                    }
                }
            }
            return userAccountId;
        }

        public int GetUserId()
        {
            int userId = 0;

            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                string SQL = $@"SELECT UserID
                                FROM UserDetails
                                WHERE UserAccountID=@UserAccountId";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@UserAccountId", HttpContext.Current.Session["UserAccountId"]));
                using (SqlDataReader reader = _dataAccessLayer.GetDataWithConditions(SQL, parameters))
                {
                    if (reader.Read())
                    {
                        userId = reader.GetInt32(reader.GetOrdinal("UserID"));
                    }
                }
            }
            return userId;
        }
    }
}
