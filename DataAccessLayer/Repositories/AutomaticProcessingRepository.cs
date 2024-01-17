using DataAccessLayer.DBConnection;
using DataAccessLayer.DTO;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using EmployeeTrainingRegistrationServices.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class AutomaticProcessingRepository : IAutomaticProcessingRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer;
        public AutomaticProcessingRepository(IDataAccessLayer dataAccessLayer)
        {
            _dataAccessLayer = dataAccessLayer;
        }

        public async Task<List<EnrolledEmployeeForExportDTO>> GetSelectedEmployeeListAsync(int id)
        {
            List<EnrolledEmployeeForExportDTO> enrolledEmployeeList = new List<EnrolledEmployeeForExportDTO>();
            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                string sql = $@"SELECT
                                U1.FirstName,
                                U1.LastName,
                                UA.Email,
                                U2.FirstName AS ManagerFirstName,
                                U2.LastName AS ManagerLastName,
                                T.Title 
                                FROM
                                ApplicationDetails A
                                INNER JOIN UserDetails U1 ON A.UserID = U1.UserID
                                INNER JOIN UserAccount UA ON U1.UserAccountID = UA.UserAccountID
                                LEFT JOIN UserDetails U2 ON U1.ManagerUserID = U2.UserID
                                LEFT JOIN UserAccount UM ON U2.UserID = UM.UserAccountID
                                INNER JOIN TrainingDetails T ON A.TrainingID=T.TrainingID
                                INNER JOIN Enrollment E ON E.ApplicationID=A.ApplicationID
                                WHERE
                                T.TrainingID=@TrainingId AND T.Deadline = CONVERT(DATE, GETDATE());";

                List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@TrainingId", SqlDbType.Int) { Value = id}

        };
                using (SqlDataReader reader = await _dataAccessLayer.GetDataWithConditionsAsync(sql, parameters))
                {


                    while (await reader.ReadAsync())
                    {
                        EnrolledEmployeeForExportDTO enrolledEmployeeItem = new EnrolledEmployeeForExportDTO
                        {
                            TrainingTitle = (string)reader["Title"],
                            FirstName = (string)reader["FirstName"],
                            LastName = (string)reader["LastName"],
                            Email = (string)reader["Email"],
                            ManagerFirstName = (string)reader["ManagerFirstName"],
                            ManagerLastName = (string)reader["ManagerLastName"]
                        };
                        enrolledEmployeeList.Add(enrolledEmployeeItem);
                    }
                }
            }
                return enrolledEmployeeList;
        }

        public async Task<List<Training>> GetTrainingByDeadlineAsync()
        {

            List<Training> trainingList = new List<Training>();

            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                string sql = $@"SELECT *
                                FROM TrainingDetails
                                WHERE Deadline = CONVERT(DATE, GETDATE());";

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
                                StartDate=(DateTime)reader["StartDate"],
                                Threshold=(int)reader["Threshold"],
                                DepartmentPriority = (int)reader["DepartmentPriority"],
                                Deadline= (DateTime)reader["StartDate"]
                            };

                            trainingList.Add(trainingItem);
                        }
                    }
                }
            }

            return trainingList;
        }

        public async Task<List<EnrolledNotificationDTO>> ProcessApplicationAsync()
        {
            List<Training> trainingList =await GetTrainingByDeadlineAsync();
            List<EnrolledNotificationDTO> enrolledEmployee = new List<EnrolledNotificationDTO>();
            List<Application> filteredApplication = new List<Application>();
            List<Application> approvedApplicant= new List<Application>();
            foreach (Training training in trainingList)
            {
                using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
                {
                    string sql = $@"SELECT TOP (@Threshold)
                                 A.ApplicationID,
                                 U.UserID,
                                 U.FirstName,
                                 U.LastName,
                                 UA.Email,
                                 A.ApplicationDate,
                                 A.Statuss
                            FROM 
                                 ApplicationDetails A
                            INNER JOIN 
                                 UserDetails U ON A.UserID = U.UserID
                            INNER JOIN 
                                 UserAccount UA ON U.UserAccountID = UA.UserAccountID
                            WHERE 
                                 A.TrainingID = @TrainingID
                                 AND A.ManagerApproval = 1
                            ORDER BY 
                                 CASE 
                                     WHEN U.DepartmentID = @PriorityDept THEN 0
                                     ELSE 1
                                 END,
                                 A.ApplicationDate ASC;";

                    List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@TrainingID", SqlDbType.Int) { Value = training.TrainingID },
             new SqlParameter("@Threshold", SqlDbType.Int) { Value = training.Threshold },
             new SqlParameter("@PriorityDept", SqlDbType.Int) { Value = training.DepartmentPriority }


        };
                    using (SqlDataReader reader =await _dataAccessLayer.GetDataWithConditionsAsync(sql, parameters))
                    {


                        while (await reader.ReadAsync())
                        {
                            Application applicationItem = new Application
                            {
                                ApplicationId = (int)reader["ApplicationID"],
                                UserId = (int)reader["UserID"],
                                FirstName = (string)reader["FirstName"],
                                LastName = (string)reader["LastName"],
                                Email = (string)reader["Email"],
                                ApplicationDate = (DateTime)reader["ApplicationDate"],
                                Status = (string)reader["Statuss"]
                            };
                            filteredApplication.Add(applicationItem);
                        }
                    }

                     approvedApplicant = filteredApplication;
                }
            }
            foreach (Application application in approvedApplicant)
            {
                using (SqlConnection sqlConnection1 = _dataAccessLayer.CreateConnection())
                {
                    string approveApplicationSql = $@"UPDATE ApplicationDetails
                                        SET Statuss='Selected'
                                        WHERE ApplicationID=@ApplicationID";

                    List<SqlParameter> approvedApplicantParameters = new List<SqlParameter>
        {
            new SqlParameter("@ApplicationID", SqlDbType.Int) { Value = application.ApplicationId }
        };

                    int numberOfRowsAffected = await _dataAccessLayer.InsertDataAsync(approveApplicationSql, approvedApplicantParameters);


                    string enrolledEmployeeSql = $@"INSERT INTO Enrollment (ApplicationID) VALUES (@ApplicationId);";
                    List<SqlParameter> enrolledApplicantParameters = new List<SqlParameter>
        {
            new SqlParameter("@ApplicationId", SqlDbType.Int) { Value = application.ApplicationId }
        };
                    int numberOfRowsAffectedEnrolled = await _dataAccessLayer.InsertDataAsync(enrolledEmployeeSql, enrolledApplicantParameters);

                    string approvedEmployeeEmailSql = $@"SELECT Email,Title
                                                                FROM TrainingDetails
                                                                INNER JOIN ApplicationDetails ON TrainingDetails.TrainingID=ApplicationDetails.TrainingID
                                                                INNER JOIN UserDetails ON ApplicationDetails.UserID=UserDetails.UserID
                                                                INNER JOIN UserAccount ON UserDetails.UserAccountID=UserAccount.UserAccountID
                                                                WHERE ManagerApproval=1 AND ApplicationDetails.ApplicationID=@ApplicationID ";

                    List<SqlParameter> approvedEmployeeEmailParameters = new List<SqlParameter>
        {
            new SqlParameter("@ApplicationID", SqlDbType.Int) { Value = application.ApplicationId }
        };
                    using (SqlDataReader reader = await _dataAccessLayer.GetDataWithConditionsAsync(approvedEmployeeEmailSql, approvedEmployeeEmailParameters))
                    {
                        while (await reader.ReadAsync())
                        {
                            EnrolledNotificationDTO enrolledNotificationItem = new EnrolledNotificationDTO
                            {
                                Email = (string)reader["Email"],
                                Title = (string)reader["Title"],

                            };
                            enrolledEmployee.Add(enrolledNotificationItem);

                        }

                    }
                }
            }
            return enrolledEmployee;
        }
    }
}
