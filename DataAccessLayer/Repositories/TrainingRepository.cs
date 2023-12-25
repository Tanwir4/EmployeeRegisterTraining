using DataAccessLayer.DBConnection;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using EmployeeTrainingRegistrationServices.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace DataAccessLayer.Repositories
{
    public class TrainingRepository : ITrainingRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer;

        public TrainingRepository(IDataAccessLayer dataAccessLayer) {
            _dataAccessLayer = dataAccessLayer;
        }
        public List<Training> GetAll()
        {
            List<Training> training = new List<Training>();
            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                string sql = $"SELECT * FROM TrainingDetails";
                using (SqlCommand command = new SqlCommand(sql, sqlConnection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Training trainingItem = new Training();
                                foreach (var property in typeof(Training).GetProperties())
                                {
                                    string columnName = property.Name;
                                    if (columnName != null && columnName != "")
                                    {
                                        var value = reader[columnName] == DBNull.Value
                                            ? null
                                            : reader[columnName];
                                        property.SetValue(trainingItem, value);
                                    }
                                }
                                training.Add(trainingItem);
                            }
                        }
                    }
                }
            }
            return training;
        }
        public List<Training> GetTrainingById(int id)
        {
            List<Training> trainingList = new List<Training>();
            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                const string SQL = "SELECT * FROM TrainingDetails WHERE TrainingID=@TrainingID";
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                new SqlParameter("@TrainingID", SqlDbType.Int) { Value = id }
                };
                SqlDataReader reader = _dataAccessLayer.GetDataWithConditions(SQL, parameters);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Training trainingItem = new Training();
                        foreach (var property in typeof(Training).GetProperties())
                        {
                            string columnName = property.Name;
                            if (columnName != null && columnName != "")
                            {
                                var value = reader[columnName] == DBNull.Value
                                    ? null
                                    : reader[columnName];
                                property.SetValue(trainingItem, value);
                            }
                        }
                        trainingList.Add(trainingItem);
                    }
                }
            }
            return trainingList;
        }

        public bool UpdateTraining(Training training)
        {
            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                string sql = $@"Update TrainingDetails 
                                SET Title=@Title
                                WHERE TrainingID=@TrainingID";
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                       new SqlParameter("@Title", SqlDbType.VarChar, 100) { Value = training.Title },
                       //new SqlParameter("@StartDate", SqlDbType.Date) { Value = training.StartDate },
                       new SqlParameter("@TrainingID", SqlDbType.Int) { Value = training.TrainingID }
                   };
                int numberOfRowsAffected =  _dataAccessLayer.InsertData(sql, parameters);
                return (numberOfRowsAffected > 0);
            }
        }
    }
}
