using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using DataAccessLayer.DBConnection;
using System.Web;
using System.Threading.Tasks;
using EmployeeTrainingRegistrationServices.Entities;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer;
        public UserRepository(IDataAccessLayer layer)
        {
            _dataAccessLayer = layer;
        }

        public async  Task<string> GetRoleByEmailAsync(string email)
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
                using (SqlDataReader reader =await _dataAccessLayer.GetDataWithConditionsAsync(SQL, parameters))
                {
                    if (await reader.ReadAsync())
                    {
                        role = reader.GetString(reader.GetOrdinal("RoleName"));
                    }
                }
            }
            return role;
        }

        public async Task<bool> AuthenticateAsync(Account user)
        {
            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                const string SQL = "SELECT 1 FROM UserAccount WHERE Email=@EmailAddress AND Passwordd=@Password";
                List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@EmailAddress", SqlDbType.VarChar, 100) { Value = user.Email },
                new SqlParameter("@Password", SqlDbType.VarChar, 100) { Value = user.Password }
            };
                SqlDataReader getData =await _dataAccessLayer.GetDataWithConditionsAsync(SQL, parameters);
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
                           SET @managerId=(SELECT UserID FROM UserDetails WHERE CONCAT(FirstName, ' ',LastName)=@ManagerName)
                           SET @deptId =(SELECT DepartmentID FROM Department WHERE DepartmentName=@DepartmentName)

                          INSERT INTO UserDetails (FirstName,LastName,MobileNumber,NationalIdentityCard,ManagerUserID,UserAccountID,DepartmentID)
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
        public async Task<int> GetUserAccountIdByEmailAsync(string email)
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
                using (SqlDataReader reader =await _dataAccessLayer.GetDataWithConditionsAsync(SQL, parameters))
                {
                    if (await reader.ReadAsync())
                    {
                        userAccountId = reader.GetInt32(reader.GetOrdinal("UserAccountID"));
                    }
                }
            }
            return userAccountId;
        }

        public async Task<string> GetManagerEmailByApplicantID()
        {
            string email = null;
            int userId =await GetUserIdAsync();
            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                string sql = $@"SELECT DISTINCT
                                UM.Email AS ManagerEmail
                                FROM
                                UserDetails U1
                                INNER JOIN UserAccount UA ON U1.UserID = UA.UserAccountID
                                LEFT JOIN UserDetails U2 ON U1.ManagerUserID = U2.UserID
                                LEFT JOIN UserAccount UM ON U2.UserID = UM.UserAccountID
                                WHERE
                                U1.UserID = @UserID;";

                List<SqlParameter> parameters = new List<SqlParameter>
                {
                new SqlParameter("@UserID", SqlDbType.VarChar, 100) { Value = userId }
                };

                using (SqlDataReader reader = await _dataAccessLayer.GetDataWithConditionsAsync(sql, parameters))
                {
                    if (await reader.ReadAsync())
                    {
                         email =  reader.GetString(reader.GetOrdinal("ManagerEmail"));
                    }
                }
               
            }
            return email;
        }



        public async Task<int> GetUserIdAsync()
        {
            int userId = 0;

            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                string SQL = $@"SELECT UserID
                                FROM UserDetails
                                WHERE UserAccountID=@UserAccountId";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@UserAccountId", HttpContext.Current.Session["UserAccountId"]));
                using (SqlDataReader reader =await _dataAccessLayer.GetDataWithConditionsAsync(SQL, parameters))
                {
                    if (await reader.ReadAsync())
                    {
                        userId = reader.GetInt32(reader.GetOrdinal("UserID"));
                    }
                }
            }
            return userId;
        }
    }
}
