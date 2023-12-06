using DataAccessLayer.DBConnection;
using DataAccessLayer.Repositories.IRepositories;
using EmployeeTrainingRegistrationServices.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class TrainingRepository : ITrainingRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer;
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
    }
}
