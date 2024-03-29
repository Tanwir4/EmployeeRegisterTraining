﻿using DataAccessLayer.DBConnection;
using DataAccessLayer.DTO;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;


namespace DataAccessLayer.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer;
        private readonly IUserRepository _userRepository;

        public ApplicationRepository(IDataAccessLayer dataAccessLayer, IUserRepository userRepository)
        {
            _dataAccessLayer = dataAccessLayer;
            _userRepository = userRepository;
        }

        public async Task<List<int>> GetAttachmentsByApplicationIdAsync(int applicationId)
        {
            List<int> attachments = new List<int>();

            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                string sql = @"
                       SELECT AttachmentID
                        FROM Attachment
                        INNER JOIN ApplicationDetails ON ApplicationDetails.ApplicationID = Attachment.ApplicationID
                        WHERE Attachment.ApplicationID = @ApplicationId;
                            ";

                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@ApplicationId", applicationId)
                };

                using (SqlDataReader reader =await _dataAccessLayer.GetDataWithConditionsAsync(sql, parameters))
                {
                    while (await reader.ReadAsync())
                    {

                        byte attachmentId = (byte)reader["AttachmentID"];
                        attachments.Add(attachmentId);

                    }
                }

                return attachments;
            }
        }
        public async Task<byte[]> GetAttachmentsByIdAsync(int attachmentId)
        {

            byte[] fileData = null;
            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                string sql = @"
                       SELECT Attachment
                        FROM Attachment
                        WHERE AttachmentID=@AttachmentID;
                            ";

                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@AttachmentID", attachmentId)
                };

                using (SqlDataReader reader =await _dataAccessLayer.GetDataWithConditionsAsync(sql, parameters))
                {
                    while (await reader.ReadAsync())
                    {


                       fileData = reader["Attachment"] == DBNull.Value ? null : (byte[])reader["Attachment"];
                      
                    }
                }

                return fileData;
            }
        }

     

        public async Task<EmailDTO> GetManagerApprovalDetailsAsync(int applicationId)
        {
            EmailDTO managerApprovalDetails = new EmailDTO();
            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                string sql = @"
                            SELECT
                             A.UserID AS ApplicantUserID,
                             CONCAT(U1.FirstName,' ',U1.LastName) AS ApplicantName, 
                             CONCAT(U2.FirstName,' ',U2.LastName) AS ManagertName, 
                             UA.Email AS ApplicantEmail,
                             U2.UserID AS ManagerUserID,
                             UM.Email AS ManagerEmail,
                             T.Title 
                             FROM
                             ApplicationDetails A
                             INNER JOIN UserDetails U1 ON A.UserID = U1.UserID
                             INNER JOIN UserAccount UA ON U1.UserAccountID = UA.UserAccountID
                             LEFT JOIN UserDetails U2 ON U1.ManagerUserID = U2.UserID
                             LEFT JOIN UserAccount UM ON U2.UserID = UM.UserAccountID
                             INNER JOIN TrainingDetails T ON A.TrainingID=T.TrainingID
                             WHERE A.ApplicationID = @ApplicationId;
                                                            ";

                List<SqlParameter> parameters = new List<SqlParameter>
    {
        new SqlParameter("@ApplicationId", applicationId)
    };

                using (SqlDataReader reader =await _dataAccessLayer.GetDataWithConditionsAsync(sql, parameters))
                {
                    if (await reader.ReadAsync())
                    {
                        managerApprovalDetails = new EmailDTO
                        {
                            ApplicantName = (string)reader["ApplicantName"],
                            ManagerName = (string)reader["ManagertName"],
                            TrainingTitle = reader.GetString(reader.GetOrdinal("Title")),
                            EmployeeEmail = reader.GetString(reader.GetOrdinal("ApplicantEmail"))

                        };
                    }
                }

                return managerApprovalDetails;
            }
        }

        public async Task<bool> DeclineApplicationAsync(string name, string title, string declineReason)
        {
            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                string sql = $@"UPDATE ApplicationDetails
                            SET DeclineReason=@DeclineReason,
                            Statuss=@Declined
                        WHERE UserID IN (
                            SELECT UserDetails.UserID
                            FROM UserDetails
                            INNER JOIN ApplicationDetails ON UserDetails.UserID = ApplicationDetails.UserID
                            WHERE CONCAT(UserDetails.FirstName, ' ', UserDetails.LastName)=@FullName
                        )
						AND TrainingID IN(
						SELECT TrainingDetails.TrainingID
                            FROM ApplicationDetails                          
                            INNER JOIN TrainingDetails ON TrainingDetails.TrainingID = ApplicationDetails.TrainingID
                            WHERE TrainingDetails.Title = @TrainingTitle
						);";
                List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@FullName", SqlDbType.VarChar, 100) { Value = name },
            new SqlParameter("@TrainingTitle", SqlDbType.VarChar, 100) { Value = title },
            new SqlParameter("@DeclineReason", SqlDbType.VarChar, 100) { Value = declineReason },
             new SqlParameter("@Declined", SqlDbType.VarChar, 100) { Value = "Declined" }
        };
                int numberOfRowsAffected =await _dataAccessLayer.InsertDataAsync(sql, parameters);
                return (numberOfRowsAffected > 0);
            }
        }

        public async Task<string> ApproveApplicationAsync(string name, string title)
        {
            string status = null; 
            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                string updateSql = $@"UPDATE ApplicationDetails
                        SET ManagerApproval = @ManagerApproval,
                        Statuss=@Approved
                        WHERE UserID IN (
                            SELECT UserDetails.UserID
                            FROM UserDetails
                            INNER JOIN ApplicationDetails ON UserDetails.UserID = ApplicationDetails.UserID
                            WHERE CONCAT(UserDetails.FirstName, ' ', UserDetails.LastName)=@FullName
                        )
						AND TrainingID IN(
						SELECT TrainingDetails.TrainingID
                            FROM ApplicationDetails                          
                            INNER JOIN TrainingDetails ON TrainingDetails.TrainingID = ApplicationDetails.TrainingID
                            WHERE TrainingDetails.Title = @TrainingTitle
						);";

                string selectSql = @"SELECT Statuss
                        FROM UserDetails
                        INNER JOIN ApplicationDetails ON UserDetails.UserID = ApplicationDetails.UserID
                        INNER JOIN TrainingDetails ON TrainingDetails.TrainingID = ApplicationDetails.TrainingID
                        WHERE CONCAT(UserDetails.FirstName, ' ', UserDetails.LastName)=@FullName
                        AND TrainingDetails.Title=@TrainingTitle";

                List<SqlParameter> parametersUpdate = new List<SqlParameter>
        {
            new SqlParameter("@FullName", SqlDbType.VarChar, 100) { Value = name },
            new SqlParameter("@TrainingTitle", SqlDbType.VarChar, 100) { Value = title },
            new SqlParameter("@Approved", SqlDbType.VarChar, 100) { Value = "Approved" },
            new SqlParameter("@ManagerApproval", SqlDbType.Int) { Value = 1 }
        };

                List<SqlParameter> parametersSelect = new List<SqlParameter>
        {
            new SqlParameter("@FullName", SqlDbType.VarChar, 100) { Value = name },
            new SqlParameter("@TrainingTitle", SqlDbType.VarChar, 100) { Value = title }
        };

                int numberOfRowsAffected =await _dataAccessLayer.InsertDataAsync(updateSql, parametersUpdate);

                using (SqlDataReader reader =await _dataAccessLayer.GetDataWithConditionsAsync(selectSql, parametersSelect))
                {
                    if (reader.HasRows)
                    {
                        await reader.ReadAsync(); 
                        status = (string)reader["Statuss"];
                    }
                }
            }

            return status;
        }

        public async Task<List<ManagerApplicationDTO>> GetApplicationsByManagerIdAsync()
        {
            List<ManagerApplicationDTO> applicationList = new List<ManagerApplicationDTO>();
            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                string sql = $@"
                                    SELECT CONCAT(UserDetails.FirstName, ' ', UserDetails.LastName) AS FullName, TrainingDetails.Title,ApplicationDetails.Statuss,ApplicationDetails.ApplicationID
                                    FROM UserDetails
                                    INNER JOIN ApplicationDetails ON UserDetails.UserID = ApplicationDetails.UserID
                                    INNER JOIN TrainingDetails ON TrainingDetails.TrainingID = ApplicationDetails.TrainingID
                                    WHERE UserDetails.ManagerUserID = @ManagerID AND Statuss=@Status;";
                int managerId =await _userRepository.GetUserIdAsync();

                List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@ManagerID", SqlDbType.Int) { Value = managerId },
            new SqlParameter("@Status", SqlDbType.VarChar,100) { Value = "Pending" }
        };

                SqlDataReader reader =await _dataAccessLayer.GetDataWithConditionsAsync(sql, parameters);

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        ManagerApplicationDTO applicationDetailsItem = new ManagerApplicationDTO
                        {
                            TrainingTitle = reader["Title"] == DBNull.Value ? null : (string)reader["Title"],
                            ApplicationStatus = reader["Statuss"] == DBNull.Value ? null : (string)reader["Statuss"],
                            ApplicantName = reader["FullName"] == DBNull.Value ? null : (string)reader["FullName"],
                            ApplicationID = (int)reader["ApplicationID"]
                        };

                        applicationList.Add(applicationDetailsItem);
                    }
                }
            }
            return applicationList;

        }

        public async  Task<List<UserApplication>> GetApplicationDetailsByUserIdAsync()
        {
            List<UserApplication> trainingDetailsList = new List<UserApplication>();
            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                const string SQL = @"
            SELECT TrainingDetails.Title, ApplicationDetails.Statuss
            FROM TrainingDetails
            INNER JOIN ApplicationDetails ON TrainingDetails.TrainingID = ApplicationDetails.TrainingID
            INNER JOIN UserDetails ON UserDetails.UserID = ApplicationDetails.UserID
            WHERE ApplicationDetails.UserID = @UserID";
                int userId =await _userRepository.GetUserIdAsync();

                List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@UserID", SqlDbType.Int) { Value = userId }
        };

                SqlDataReader reader =await _dataAccessLayer.GetDataWithConditionsAsync(SQL, parameters);

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        UserApplication trainingDetailsItem = new UserApplication
                        {
                            TrainingTitle = reader["Title"] == DBNull.Value ? null : (string)reader["Title"],
                            Status = reader["Statuss"] == DBNull.Value ? null : (string)reader["Statuss"],
                        };

                        trainingDetailsList.Add(trainingDetailsItem);
                    }
                }
            }

            return trainingDetailsList;
        }

        public async Task<bool> SaveApplicationAsync(int trainingId, List<byte[]> fileDataList)
        {
            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {

                using (SqlTransaction transaction = sqlConnection.BeginTransaction())
                {
                    
                        // Insert application details
                        string applicationSql = $@"INSERT INTO ApplicationDetails (ApplicationDate, Statuss, ManagerApproval, TrainingID, UserID, DeclineReason) 
                                          VALUES (@CurrentDate, 'Pending', 0, @TrainingId, @UserId, @DeclineReason);
                                          SELECT SCOPE_IDENTITY();";

                        DateTime currentDate = DateTime.Today;
                        int userId =await _userRepository.GetUserIdAsync();

                        int applicationId;
                        using (SqlCommand cmdApplication = new SqlCommand(applicationSql, sqlConnection, transaction))
                        {
                            cmdApplication.Parameters.Add("@CurrentDate", SqlDbType.Date).Value = currentDate;
                            cmdApplication.Parameters.Add("@TrainingId", SqlDbType.Int).Value = trainingId;
                            cmdApplication.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                            cmdApplication.Parameters.Add("@DeclineReason", SqlDbType.VarChar,50).Value = "N/A";

                        applicationId = Convert.ToInt32(cmdApplication.ExecuteScalar());
                        }

                        // Insert each file data into Attachment table with the corresponding ApplicationID
                        string attachmentSql = "INSERT INTO Attachment (Attachment, ApplicationID) VALUES (@FileData, @ApplicationId);";

                        foreach (var fileData in fileDataList)
                        {
                            using (SqlCommand cmdAttachment = new SqlCommand(attachmentSql, sqlConnection, transaction))
                            {
                                cmdAttachment.Parameters.Add("@FileData", SqlDbType.VarBinary).Value = fileData;
                                cmdAttachment.Parameters.Add("@ApplicationId", SqlDbType.Int).Value = applicationId;
                                cmdAttachment.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                        return true;
                }
            }
        }
    }
}
