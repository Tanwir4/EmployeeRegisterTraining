using DataAccessLayer.DBConnection;
using DataAccessLayer.Repositories.IRepositories;
using EmployeeTrainingRegistrationServices.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class TrainingRepository : ITrainingRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer;
        private readonly IUserRepository _userRepository;
        public TrainingRepository(IDataAccessLayer dataAccessLayer, IUserRepository userRepository)
        {
            _dataAccessLayer = dataAccessLayer;
            _userRepository = userRepository;
        }
        public async Task<bool> IsTrainingAppliedAsync(int trainingId)
        {
            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                string sql = $@"
                           SELECT 1
                            FROM ApplicationDetails AD
                            JOIN UserDetails UD ON UD.UserID = AD.UserID
                            JOIN TrainingDetails TD ON AD.TrainingID = TD.TrainingID
                            WHERE UD.UserID = @UserID
                            AND TD.TrainingID = @TrainingID;";
                int userId =await _userRepository.GetUserIdAsync();
                List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@TrainingID", SqlDbType.Int) { Value = trainingId },
            new SqlParameter("@UserID", SqlDbType.Int) { Value = userId }

        };
                SqlDataReader getData =await _dataAccessLayer.GetDataWithConditionsAsync(sql, parameters);
                return (getData.HasRows);
            }
             
        }
        public async Task<bool> IsTrainingExpired(int trainingId)
        {
            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                string sql = $@"
                        SELECT 1
                        FROM TrainingDetails
                        WHERE Deadline > GETDATE() AND IsActive=@IsActive AND TrainingID=@TrainingID;";
                List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@IsActive", SqlDbType.Int) { Value = 1 },
            new SqlParameter("@TrainingID", SqlDbType.Int) { Value = trainingId }
        };
                SqlDataReader getData = await _dataAccessLayer.GetDataWithConditionsAsync(sql, parameters);
                return (getData.HasRows);
            }

        }
        public int GetNewTrainingId(Training training, Department department)
        {
            int trainingId = 0;

            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                string SQL = $@"DECLARE @DeptPriority INT
        SET @DeptPriority = (SELECT DepartmentID FROM Department WHERE DepartmentName = @DepartmentName)
        INSERT INTO TrainingDetails (Title, StartDate, Threshold, Description, Deadline, DepartmentPriority) 
        VALUES (@Title, @StartDate, @Threshold, @Description, @Deadline, @DeptPriority);
        SET @TrainingId = SCOPE_IDENTITY();";

                List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@Title", SqlDbType.VarChar, 100) { Value = training.Title },
            new SqlParameter("@StartDate", SqlDbType.Date) { Value = training.StartDate },
            new SqlParameter("@Threshold", SqlDbType.Int) { Value = training.Threshold },
            new SqlParameter("@Description", SqlDbType.VarChar, 100) { Value = training.Description },
            new SqlParameter("@Deadline", SqlDbType.Date) { Value = training.Deadline },
            new SqlParameter("@DepartmentName", SqlDbType.VarChar, 40) { Value = department.DepartmentName },
            new SqlParameter("@TrainingId", SqlDbType.Int) { Direction = ParameterDirection.Output }
        };
                int numberOfRowsAffected = _dataAccessLayer.InsertData(SQL, parameters);
                if (parameters[6].Value != DBNull.Value)
                {
                    trainingId = Convert.ToInt32(parameters[6].Value);
                }
            }

            return trainingId;
        }
        public bool AddTraining(Training training, Department department)
        {
            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                    int trainingId= GetNewTrainingId(training,department);
                    List<int> prerequisiteIds = new List<int>();
                    string selectPrerequisiteIdSql = "SELECT PreRequisiteID FROM PreRequisite WHERE PreRequisite = @PrerequisiteName;";
                    foreach (string prerequisite in training.PreRequisite)
                    {
                        using (SqlCommand cmd = new SqlCommand(selectPrerequisiteIdSql, sqlConnection))
                        {
                            cmd.Parameters.AddWithValue("@PrerequisiteName", prerequisite);
                            int prerequisiteId = Convert.ToInt32(cmd.ExecuteScalar());
                            prerequisiteIds.Add(prerequisiteId);
                        }
                    }
                    string insertPrerequisitesSql = @"
                INSERT INTO TrainingPreRequisite (PreRequisiteID,TrainingID) 
                VALUES (@PreRequisiteID,@TrainingID);";

                    foreach (int prerequisiteId in prerequisiteIds)
                    {
                        using (SqlCommand cmd = new SqlCommand(insertPrerequisitesSql, sqlConnection))
                        {
                            cmd.Parameters.AddWithValue("@TrainingID", trainingId);
                            cmd.Parameters.AddWithValue("@PrerequisiteID", prerequisiteId);
                             cmd.ExecuteNonQuery();
                        }
                    }

                    return true; 
            }
        }

        public async Task<bool> DeleteTrainingAsync(int id)
        {
            bool IsTrainingEnrolled = await DoesTrainingExistInEnrolledAsync(id);
            if (!IsTrainingEnrolled)
            {
                using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
                {
                    string sql = $@"Update TrainingDetails 
                            SET IsActive=0
                            WHERE TrainingID=@TrainingID";

                    List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@TrainingID", SqlDbType.Int) { Value = id }
            };

                    int numberOfRowsAffected = await _dataAccessLayer.InsertDataAsync(sql, parameters);
                    return (numberOfRowsAffected > 0);
                }
            }
            else
            {
                return true;
            }
        }
        public async Task<bool> DoesTrainingExistInEnrolledAsync(int id)
        {
            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                string sql = $@"select 1 from ApplicationDetails
                                inner join UserDetails on UserDetails.UserID=ApplicationDetails.UserID
                                inner join Enrollment on Enrollment.ApplicationID=ApplicationDetails.ApplicationID
                                where ApplicationDetails.TrainingID=@TrainingID";
                List<SqlParameter> parameters = new List<SqlParameter>
                      {
                             new SqlParameter("@TrainingID", SqlDbType.Int) { Value = id }
                         };
                SqlDataReader reader = await _dataAccessLayer.GetDataWithConditionsAsync(sql, parameters);
                if (reader.HasRows) { return true; }
                else { return false; }
            }
        }
        public async Task<List<Training>> GetAllForAdminAsync()
        {
            List<Training> trainingList = new List<Training>();

            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                string sql = $@"SELECT *
                                FROM TrainingDetails
                                WHERE IsActive = 1";

                using (SqlCommand command = new SqlCommand(sql, sqlConnection))
                {
                    using (SqlDataReader reader =await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Training trainingItem = new Training
                            {
                                TrainingID = (int)reader["TrainingID"],
                                Title = reader["Title"] == DBNull.Value ? null : (string)reader["Title"],
                                Description = reader["Description"] == DBNull.Value ? null : (string)reader["Description"]
                            };

                            trainingList.Add(trainingItem);
                        }
                    }
                }
            }

            return trainingList;
        }
        public async Task<List<Training>> GetAllForEmployeeAsync()
        {
            List<Training> trainingList = new List<Training>();

            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                string sql = $@"SELECT *
                                FROM TrainingDetails
                                WHERE IsActive = 1 AND Deadline >= GETDATE();";

                using (SqlCommand command = new SqlCommand(sql, sqlConnection))
                {
                    using (SqlDataReader reader =await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Training trainingItem = new Training
                            {
                                TrainingID = (int)reader["TrainingID"],
                                Title = reader["Title"] == DBNull.Value ? null : (string)reader["Title"],
                                Description = reader["Description"] == DBNull.Value ? null : (string)reader["Description"]
                            };

                            trainingList.Add(trainingItem);
                        }
                    }
                }
            }

            return trainingList;
        }

        public async Task<List<string>> GetPrerequisitesByTrainingIdAsync(int trainingID)
        {
            List<string> preRequisites = new List<string>();
            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                string sql = @"
            SELECT 
                pr.PreRequisite
            FROM TrainingPreRequisite tp
            INNER JOIN TrainingDetails td ON tp.TrainingID = td.TrainingID
            INNER JOIN PreRequisite pr ON tp.PreRequisiteID = pr.PreRequisiteID
            WHERE td.TrainingID = @TrainingID;";

                List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@TrainingID", SqlDbType.Int) { Value = trainingID }
        };

                SqlDataReader reader =await _dataAccessLayer.GetDataWithConditionsAsync(sql, parameters);

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        string preRequisite = (string)reader["PreRequisite"];
                        preRequisites.Add(preRequisite);
                    }
                }
            }

            return preRequisites;
        }
        public async Task<List<string>> GetAllPreRequisitesAsync()
        {
            List<string> preRequisites = new List<string>();
            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                string sql = @"SELECT PreRequisite FROM PreRequisite";

                using (SqlCommand command = new SqlCommand(sql, sqlConnection))
                {
                    using (SqlDataReader reader =await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string preRequisite = (string)reader["PreRequisite"];
                            preRequisites.Add(preRequisite);

                        }
                    }
                }
            }
            return preRequisites;
        }
        public async Task<Training> GetTrainingByIdAsync(int id)
        {
            Training trainingItem = new Training();
            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                 string sql = $@"SELECT * FROM TrainingDetails 
                                INNER JOIN Department ON Department.DepartmentID=TrainingDetails.DepartmentPriority
                                WHERE TrainingID=@TrainingID";
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                new SqlParameter("@TrainingID", SqlDbType.Int) { Value = id }
                };
                SqlDataReader reader =await _dataAccessLayer.GetDataWithConditionsAsync(sql, parameters);
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {

                        trainingItem.TrainingID = (int)reader["TrainingID"];
                        trainingItem.Title = reader["Title"] == DBNull.Value ? null : (string)reader["Title"];
                        trainingItem.Description = reader["Description"] == DBNull.Value ? null : (string)reader["Description"];
                        trainingItem.StartDate = (DateTime)reader["StartDate"];
                        trainingItem.Deadline = (DateTime)reader["Deadline"];
                            trainingItem.Threshold = (int)reader["Threshold"];
                        trainingItem.DepartmentName = (string)reader["DepartmentName"];
                    }
                }
                return trainingItem;
            }
        }
        public async Task<bool> UpdateTrainingAsync(Training training, Department department, List<string> checkedPrerequisites)
        {
            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                string sql = $@"DECLARE @DeptPriority INT
                        SET @DeptPriority=(SELECT DepartmentID FROM Department WHERE DepartmentName=@DepartmentName)
                        UPDATE TrainingDetails 
                        SET Title=@Title, 
                            StartDate=@StartDate,
                            Threshold=@Threshold, 
                            Description=@PreRequisite, 
                            Deadline=@Deadline,
                            DepartmentPriority=@DeptPriority
                        WHERE TrainingID=@TrainingID;
                        DELETE FROM TrainingPreRequisite WHERE TrainingID=@TrainingID;
                        INSERT INTO TrainingPreRequisite (TrainingID, PreRequisiteID)
                        VALUES ";

                // Add values for prerequisites
                for (int i = 0; i < training.PreRequisite.Count; i++)
                {
                    sql += $"(@TrainingID, (SELECT PreRequisiteID FROM PreRequisite WHERE PreRequisite = @PreReq{i}))";

                    // Add comma for all but the last value
                    if (i < training.PreRequisite.Count - 1)
                    {
                        sql += ",";
                    }
                }

                List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@Title", SqlDbType.VarChar, 100) { Value = training.Title },
            new SqlParameter("@StartDate", SqlDbType.Date) { Value = training.StartDate },
            new SqlParameter("@Deadline", SqlDbType.Date) { Value = training.Deadline },
            new SqlParameter("@Threshold", SqlDbType.Int) { Value = training.Threshold },
            new SqlParameter("@TrainingID", SqlDbType.Int) { Value = training.TrainingID },
            new SqlParameter("@PreRequisite", SqlDbType.VarChar, 100) { Value = training.Description },
            new SqlParameter("@DepartmentName", SqlDbType.VarChar, 40) { Value = department.DepartmentName }
        };

                // Add parameters for prerequisites
                for (int i = 0; i < training.PreRequisite.Count; i++)
                {
                    parameters.Add(new SqlParameter($"@PreReq{i}", SqlDbType.VarChar, 100) { Value = training.PreRequisite[i] });
                }

                int numberOfRowsAffected =await _dataAccessLayer.InsertDataAsync(sql, parameters);
                return (numberOfRowsAffected > 0);
            }
        }
    }
}
