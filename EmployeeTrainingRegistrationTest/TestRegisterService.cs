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
           .Setup(iUserRepository => iUserRepository.IsEmailUniqueAsync(It.IsAny<string>()))
           .ReturnsAsync((string email) =>
           {
               bool isExist = users.Any(u => u.Email == email);
               if (!isExist)
               {
                   return true;
               }
               return false;
           });

            _stubUserRepository
            .Setup(iUserRepository => iUserRepository.IsNicUniqueAsync(It.IsAny<string>()))
            .ReturnsAsync((string nic) =>
            {
                bool isExist = users.Any(u => u.NationalIdentityCard == nic);
                if (!isExist)
                {
                    return true;
                }
                return false;
            });


            _stubUserRepository
            .Setup(iUserRepository => iUserRepository.IsMobileNumberUniqueAsync(It.IsAny<string>()))
            .ReturnsAsync((string mobNum) =>
            {
                bool isExist = users.Any(u => u.MobileNumber == mobNum);
                if (!isExist)
                {
                    return true;
                }
                return false;
            });

            _stubUserRepository
             .Setup(iUserRepository => iUserRepository.RegisterAsync(It.IsAny<User>()))
             .ReturnsAsync(true);

            _registerService = new RegisterService(_stubUserRepository.Object);
            _accountService = new AccountService(_stubUserRepository.Object);

        }
        
        [TestCase("Tanwir", "Lollmohamud","57919528","L04060176543218","Rushmee Toolsee","Product and Technology","nasweerah.4@gmail.com","1234", ExpectedResult = true)]
        public async Task<bool> IsRegisteredAsync_ValidDetails_SuccessfulRegistration(string firstName,string lastName,string mobNumber,string nic,string managerName,string dept,string email, string password)
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
            bool IsEmailUnique =await _accountService.IsEmailUniqueAsync(registerUser.Email);
            bool IsNicUnique =await _accountService.IsNicUniqueAsync(registerUser.NationalIdentityCard);
            bool IsMobileNumberUnique = await _accountService.IsMobileNumberUniqueAsync(registerUser.MobileNumber);
            bool IsRegistered = await _registerService.IsRegisteredAsync(registerUser);
            //Assert
            return (IsRegistered && IsEmailUnique && IsNicUnique && IsMobileNumberUnique);
        }

        [TestCase("Tanwir", "Lollmohamud", "57919528", "Y07050154367543", "Rushmee Toolsee", "Product and Technology", "yang@gmail.com", "1234", ExpectedResult = false)]
        [TestCase("Tanwir", "Lollmohamud", "57772294", "L04060176543218", "Rushmee Toolsee", "Product and Technology", "yang@gmail.com", "1234", ExpectedResult = false)]
        [TestCase("Tanwir", "Lollmohamud", "57772294", "Y07050154367543", "Rushmee Toolsee", "Product and Technology", "nasweerah.4@gmail.com", "1234", ExpectedResult = false)]
        public async Task<bool> IsRegisteredAsync_InvalidInput_UnsuccessfulRegistration(string firstName, string lastName, string mobNumber, string nic, string managerName, string dept, string email, string password)
        {
            //Arrange
            User registerUser = new User()
            {
                FirstName = firstName,
                LastName = lastName,
                MobileNumber = mobNumber,
                NationalIdentityCard = nic,
                ManagerName = managerName,
                DepartmentName = dept,
                Email = email,
                Password = password
            };
            //Act
            bool IsEmailUnique = await _accountService.IsEmailUniqueAsync(registerUser.Email);
            bool IsNicUnique = await _accountService.IsNicUniqueAsync(registerUser.NationalIdentityCard);
            bool IsMobileNumberUnique = await _accountService.IsMobileNumberUniqueAsync(registerUser.MobileNumber);
            bool IsRegistered = await _registerService.IsRegisteredAsync(registerUser);
            //Assert
            return (IsRegistered && IsEmailUnique && IsNicUnique && IsMobileNumberUnique);
        }

    }
}
