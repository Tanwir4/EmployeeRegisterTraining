using DataAccessLayer.DBConnection;
using DataAccessLayer.DTO;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using EmployeeTrainingRegistrationServices.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
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
        public List<Training> GetTrainingByDeadline()
        {

            List<Training> trainingList = new List<Training>();

            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                string sql = $@"SELECT *
                                FROM TrainingDetails
                                WHERE Deadline = CONVERT(DATE, GETDATE());";

                using (SqlCommand command = new SqlCommand(sql, sqlConnection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
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

        public List<EnrolledNotificationDTO> ProcessApplication()
        {
            List<Training> trainingList = GetTrainingByDeadline();
            List<EnrolledNotificationDTO> enrolledEmployee = new List<EnrolledNotificationDTO>();
            List<Application> filteredApplication = new List<Application>();
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
                    using (SqlDataReader reader = _dataAccessLayer.GetDataWithConditions(sql, parameters))
                    {


                        while (reader.Read())
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

                    List<Application> approvedApplicant = filteredApplication;

                    foreach (Application application in approvedApplicant)
                    {
                        using (SqlConnection sqlConnection1 = _dataAccessLayer.CreateConnection())
                        {
                            string approveApplicationSql = $@"UPDATE ApplicationDetails
                                        SET Statuss='Approved'
                                        WHERE ApplicationID=@ApplicationID";

                            List<SqlParameter> approvedApplicantParameters = new List<SqlParameter>
        {
            new SqlParameter("@ApplicationID", SqlDbType.Int) { Value = application.ApplicationId }
        };

                            int numberOfRowsAffected = _dataAccessLayer.InsertData(approveApplicationSql, approvedApplicantParameters);

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
                            using (SqlDataReader reader = _dataAccessLayer.GetDataWithConditions(approvedEmployeeEmailSql, approvedEmployeeEmailParameters))
                            {
                                while (reader.Read())
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




                }

            }
            return enrolledEmployee;
        }
    }
}
