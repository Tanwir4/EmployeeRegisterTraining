using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using DataAccessLayer.DBConnection;
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


        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                string sql = $@"select 1 from UserAccount where Email=@Email";

                List<SqlParameter> parameters = new List<SqlParameter>
                   {
                       new SqlParameter("@Email", SqlDbType.VarChar, 100) { Value = email }
                   };
                SqlDataReader reader = await _dataAccessLayer.GetDataWithConditionsAsync(sql, parameters);
                return (reader.HasRows);
            }
        }

        public async Task<List<string>> GetAllManagersByDepartmentAsync(string department)
        {
            List<string> managerList = new List<string>();
            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                string sql = $@"select concat(FirstName,' ',LastName) As ManagerName 
                                from UserDetails
                                inner join Department on Department.DepartmentID=UserDetails.DepartmentID
                                where DepartmentName=@DepartmentName AND ManagerUserID IS NULL;
                                ";
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                new SqlParameter("@DepartmentName", SqlDbType.VarChar, 100) { Value = department }
                };
                using (SqlDataReader reader = await _dataAccessLayer.GetDataWithConditionsAsync(sql, parameters))
                {
                    string managerName;
                   while (await reader.ReadAsync())
                    {
                        managerName = reader.GetString(reader.GetOrdinal("ManagerName"));
                        managerList.Add(managerName);
                    }
                }
            }
            return managerList;
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

        public async Task<Account> AuthenticateAsync(Account account)
        {
            Account acc = new Account();
            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                const string SQL = "SELECT HashedPassword,Salt FROM UserAccount WHERE Email=@EmailAddress";
                List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@EmailAddress", SqlDbType.VarChar, 100) { Value = account.Email }
               
            };
                SqlDataReader getData =await _dataAccessLayer.GetDataWithConditionsAsync(SQL, parameters);

                if (await getData.ReadAsync())
                {
                    acc.HashedPassword = (byte[])getData["HashedPassword"];
                    acc.Salt = (byte[])getData["Salt"];
                }
                    return acc;
            }
        }
        public async Task<bool> RegisterAsync(User user)
        {
            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                string sql = $@"
                           DECLARE @AccountTempID INT
                           DECLARE @managerId INT
                           DECLARE @deptId INT
                           INSERT INTO UserAccount (Email, Passwordd,HashedPassword,Salt,RoleID) VALUES (@Email,@Passwordd,@HashedPassword,@Salt,1)

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
                       new SqlParameter("@DepartmentName", SqlDbType.VarChar, 40) { Value = user.DepartmentName },
                       new SqlParameter("@HashedPassword", SqlDbType.VarBinary) { Value = user.HashedPassword },
                        new SqlParameter("@Salt", SqlDbType.VarBinary) { Value = user.Salt }
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

        public async Task<string> GetManagerEmailByApplicantIDAsync()
        {
            string email = null;
            int userId = await GetUserIdAsync();
            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                string sql = $@"declare @ManagerUserID int
                                declare @ManagerUserAccountID int
                                declare @ManagerEmail varchar(max)
                                set @ManagerUserID=(select ManagerUserID from UserDetails where UserID=@UserID)
                                set @ManagerUserAccountID=(select UserAccountID from UserDetails where UserID=@ManagerUserID)
                                set @ManagerEmail=(select Email from UserAccount where UserAccountID=@ManagerUserAccountID)
                                select @ManagerEmail as ManagerEmail;";

                List<SqlParameter> parameters = new List<SqlParameter>
                {
                new SqlParameter("@UserID", SqlDbType.VarChar, 100) { Value = userId }
                };

                using (SqlDataReader reader = await _dataAccessLayer.GetDataWithConditionsAsync(sql, parameters))
                {
                    if (await reader.ReadAsync())
                    {
                        email = reader.GetString(reader.GetOrdinal("ManagerEmail"));
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
