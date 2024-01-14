using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using EmployeeTrainingRegistrationServices.Security;
using EmployeeTrainingRegistrationServices.Services;
using Moq;
namespace EmployeeTrainingRegistrationTest
{
    [TestFixture]
    public class TestLoginService
    {
        private Mock<IUserRepository> _stubUserRepository;
        private LoginService _loginService;

        [SetUp]
        public void Setup()
        {
            var salt = PasswordHashing.GenerateSalt();
            List<Account> accounts = new List<Account>()
            {

                new Account()
                {
                    UserAccountId = 1,
                    Email = "nasweerah@gmail.com",
                    Password = "fakepassword123",
                    HashedPassword =PasswordHashing.ComputeStringToSha256Hash("fakepassword123",salt),
                    Salt = salt
                }
            };

            _stubUserRepository = new Mock<IUserRepository>();
            _stubUserRepository
                .Setup(iUserRepository=> iUserRepository.AuthenticateAsync(It.IsAny<Account>()))
                .ReturnsAsync((Account acc)=>
                {
                    return accounts.Where(account => account.Email == acc.Email).FirstOrDefault();

                });
            _loginService = new LoginService(_stubUserRepository.Object);

        }

        [Test]
        [TestCase("nasweerah@gmail.com", "fakepassword123",ExpectedResult =true)]
        public async Task<bool> IsAuthenticatedAsyncTest_AuthenticatedUser_ShouldGetAuthenticated(string email,string password)
        {
            //Arrange

            Account loginAccount = new Account()
            {
                Email = email,
                Password = password
            };

            //Act
            bool IsAuthenticated =await _loginService.IsAuthenticatedAsync(loginAccount);

            //Assert
            return IsAuthenticated;

        }

        [Test]
        [TestCase("nasweerah@gmail.com", "fakepassword12345", ExpectedResult = false)]
        public async Task<bool> IsAuthenticatedAsyncTest_AuthenticatedUser_ShouldNotGetAuthenticated(string email, string password)
        {
            //Arrange

            Account loginAccount = new Account()
            {
                Email = email,
                Password = password
            };

            //Act
            bool IsAuthenticated = await _loginService.IsAuthenticatedAsync(loginAccount);

            //Assert
            return IsAuthenticated;

        }




    }
}
