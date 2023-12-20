using DataAccessLayer.DBConnection;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

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
        public bool saveApplication(int trainingId, byte[] fileData)
        {
            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                string sql = $@"INSERT INTO ApplicationDetails (ApplicationDate,Statuss,ManagerApproval,Attachment,TrainingID,UserID,DeclineReason) 
                                              VALUES(@CurrentDate,'Pending',0,@FileData,@TrainingId,@UserId,'Processing')";
                DateTime currentDate = DateTime.Today;
                int userId = _userRepository.GetUserId();

                List<SqlParameter> parameters = new List<SqlParameter>
                      {
                          new SqlParameter("@CurrentDate", SqlDbType.Date) { Value = currentDate },
                          new SqlParameter("@FileData", SqlDbType.VarBinary) { Value = fileData },
                          new SqlParameter("@TrainingId", SqlDbType.Int) { Value = trainingId },
                          new SqlParameter("@UserId", SqlDbType.Int) { Value = userId },
                      };
                return (_dataAccessLayer.InsertData(sql, parameters) > 0);
            }
        }

        public List<UserApplication> GetApplicationDetailsByUserId()
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
                int userId = _userRepository.GetUserId();

                List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@UserID", SqlDbType.Int) { Value = userId }
        };

                SqlDataReader reader = _dataAccessLayer.GetDataWithConditions(SQL, parameters);

                if (reader.HasRows)
                {
                    while (reader.Read())
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

    }
}
