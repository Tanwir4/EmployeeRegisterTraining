using DataAccessLayer.Repositories.IRepositories;
using EmployeeTrainingRegistrationServices.Entities;
using EmployeeTrainingRegistrationServices.Services;
using Moq;

namespace EmployeeTrainingRegistrationTest
{
    [TestFixture]
    public class TestDepartmentService
    {
        private Mock<IDepartmentRepository> _stubDepartmentRepository;
        private DepartmentService _departmentService;

        [SetUp]
        public void Setup()
        {
            // Arrange
            List<Department> departmentList = new List<Department>()
        {
            new Department()
            {
                DepartmentId = 1,
                DepartmentName = "Product and Technology"
            }
        };

            _stubDepartmentRepository = new Mock<IDepartmentRepository>();
            _stubDepartmentRepository
                .Setup(iDepartmentRepository => iDepartmentRepository.GetAllDepartmentNameAsync())
                .ReturnsAsync(departmentList);

            _departmentService = new DepartmentService(_stubDepartmentRepository.Object);
        }

        [Test]
        public async Task GetAllDepartmentNameAsync_ReturnsAllDepartmentName()
        {
            // Act
            var result = await _departmentService.GetAllDepartmentNameAsync();

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
