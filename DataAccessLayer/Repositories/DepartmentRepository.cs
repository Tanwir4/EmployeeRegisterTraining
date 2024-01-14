using DataAccessLayer.Repositories.IRepositories;
using EmployeeTrainingRegistrationServices.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DataAccessLayer.DBConnection;

namespace DataAccessLayer.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer;

        public DepartmentRepository(IDataAccessLayer layer)
        {
            _dataAccessLayer = layer;
        }
        public async Task<List<Department>> GetAllDepartmentNameAsync()
        {
            List<Department> departments = new List<Department>();
            using (SqlConnection sqlConnection = _dataAccessLayer.CreateConnection())
            {
                string sql = "SELECT DepartmentName from Department";
                using (SqlCommand command = new SqlCommand(sql, sqlConnection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                Department departmentItem = new Department
                                {
                                    DepartmentName = reader["DepartmentName"] == DBNull.Value ? null : (string)reader["DepartmentName"]
                                };

                                departments.Add(departmentItem);
                            }
                        }
                    }
                }
            }
            return departments;
        }

    }
}
