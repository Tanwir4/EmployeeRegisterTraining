
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using EmployeeTrainingRegistrationServices.Services;
using Moq;

namespace EmployeeTrainingRegistrationTest
{
    [TestFixture]
    public class TestAccountService
    {
        private Mock<IUserRepository> _stubUserRepository;
        private AccountService _accountService;

        [SetUp]
        public void Setup()
        {
            List<Account> accounts = new List<Account>()
            {

                new Account()
                {
                    UserAccountId = 1,
                    Email = "nasweerah@gmail.com",
                    Password = "fakepassword123"
                }
            };

            _stubUserRepository = new Mock<IUserRepository>();

            _stubUserRepository
            .Setup(iUserRepository => iUserRepository.GetUserAccountIdByEmailAsync(It.IsAny<String>()))
            .ReturnsAsync((Account acc) =>
            {
                return accounts.Where(account => account.Email == acc.Email).FirstOrDefault().UserAccountId;

            });


            _accountService = new AccountService(_stubUserRepository.Object);
        }


    }
}
