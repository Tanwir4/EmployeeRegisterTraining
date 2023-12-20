using DataAccessLayer.Models;
using System.Collections.Generic;

namespace DataAccessLayer.Repositories.IRepositories
{
    public interface IApplicationRepository
    {
        bool saveApplication(int trainingId, byte[] fileData);
        List<UserApplication> GetApplicationDetailsByUserId();
    }
}
