using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using EmployeeTrainingRegistrationServices.Security;
using EmployeeTrainingRegistrationServices.Services;
using Moq;
namespace EmployeeTrainingRegistrationTest
{
    [TestFixture]
    public class TestRegisterService
    {
        private Mock<IUserRepository> _stubUserRepository;
        private RegisterService _registerService;
        private AccountService _accountService;

        [SetUp]
        public void Setup()
        {
            var salt = PasswordHashing.GenerateSalt();
            List<User> users = new List<User>()
            {

                new User()
                {
                    UserId = 1,
                    FirstName="Andy",
                    LastName="Yang",
                    MobileNumber="57772294",
                    NationalIdentityCard="Y07050154367543",
                    ManagerName="Rushmee Toolsee",
                    DepartmentName="Product and Technology",
                    Email = "yang@gmail.com",
                    Password = "fakepassword123",
                    HashedPassword =PasswordHashing.ComputeStringToSha256Hash("fakepassword123",salt),
                    Salt = salt
                }
            };

            _stubUserRepository = new Mock<IUserRepository>();

            _stubUserRepository
            .Setup(iUserRepository => iUserRepository.IsEmailUniqueAsync("yang@gmail.com"))
            .Returns(false);

            _stubUserRepository
            .Setup(iUserRepository => iUserRepository.IsEmailUniqueAsync(It.Is<string>(email => email != "yang@gmail.com")))
            .Returns(true);

            _stubUserRepository
            .Setup(iUserRepository => iUserRepository.IsNicUniqueAsync("Y07050154367543"))
            .ReturnsAsync(false);

            _stubUserRepository
            .Setup(iUserRepository => iUserRepository.IsNicUniqueAsync(It.Is<string>(nic => nic != "Y07050154367543")))
            .ReturnsAsync(true);

            _stubUserRepository
            .Setup(iUserRepository => iUserRepository.IsMobileNumberUniqueAsync("57772294"))
            .ReturnsAsync(false);

            _stubUserRepository
            .Setup(iUserRepository => iUserRepository.IsMobileNumberUniqueAsync(It.Is<string>(mobNumber => mobNumber != "57772294")))
            .ReturnsAsync(true);

            _stubUserRepository
             .Setup(iUserRepository => iUserRepository.RegisterAsync(It.IsAny<User>()))
             .ReturnsAsync(true);

            _registerService = new RegisterService(_stubUserRepository.Object);
            _accountService = new AccountService(_stubUserRepository.Object);

        }
        [Test]
        [TestCase("Tanwir", "Lollmohamud","57919528","L04060176543218","Rushmee Toolsee","Product and Technology","nasweerah.4@gmail.com","1234", ExpectedResult = true)]
        [TestCase("Tanwir", "Lollmohamud", "57919528", "Y07050154367543", "Rushmee Toolsee", "Product and Technology", "yang@gmail.com", "1234", ExpectedResult = false)]
        [TestCase("Tanwir", "Lollmohamud", "57772294", "L04060176543218", "Rushmee Toolsee", "Product and Technology", "yang@gmail.com", "1234", ExpectedResult = false)]
        [TestCase("Tanwir", "Lollmohamud", "57772294", "Y07050154367543", "Rushmee Toolsee", "Product and Technology", "nasweerah.4@gmail.com", "1234", ExpectedResult = false)]
        public async Task<bool> IsRegisteredAsyncTest(string firstName,string lastName,string mobNumber,string nic,string managerName,string dept,string email, string password)
        {
            //Arrange

            User registerUser = new User()
            {
                FirstName=firstName,
                LastName=lastName,
                MobileNumber = mobNumber,
                NationalIdentityCard=nic,
                ManagerName=managerName,
                DepartmentName=dept,
                Email = email,
                Password = password
            };

            //Act
            bool IsRegistered = await _registerService.IsRegisteredAsync(registerUser);
            bool IsEmailUnique = _accountService.IsEmailUniqueAsync(registerUser.Email);
            bool IsNicUnique =await _accountService.IsNicUniqueAsync(registerUser.NationalIdentityCard);
            bool IsMobileNumberUnique = await _accountService.IsMobileNumberUniqueAsync(registerUser.MobileNumber);

            //Assert
            return (IsRegistered && IsEmailUnique && IsNicUnique && IsMobileNumberUnique);

        }

    }
}
