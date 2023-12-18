namespace EmployeeTrainingRegistrationServices.Interfaces
{
    public interface IAccountService
    {
        int GetUserAccountId(string email);
        bool IsApplicationSubmitted(int trainingId);
    }
}
