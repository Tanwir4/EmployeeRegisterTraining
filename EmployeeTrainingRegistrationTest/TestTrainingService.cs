using DataAccessLayer.Repositories.IRepositories;
using EmployeeTrainingRegistrationServices.Entities;
using EmployeeTrainingRegistrationServices.Services;
using Moq;
namespace EmployeeTrainingRegistrationTest
{
    public class TestTrainingService
    {
        private Mock<ITrainingRepository> _stubTrainingRepository;
        private TrainingService _trainingService;

        [SetUp]
        public void Setup()
        {
            // Arrange
            List<Training> trainingList = new List<Training>()
        {
            new Training()
            {
                TrainingID = 1,
                Title = "SQL Training",
                Description = "SQL Training Description",
                StartDate = DateTime.Now,
                Deadline = DateTime.Now.AddMonths(1),
                Threshold = 10,
                DepartmentName = "Product and Technology"
            }
        };

            _stubTrainingRepository = new Mock<ITrainingRepository>();
            _stubTrainingRepository
                .Setup(iTrainingRepository => iTrainingRepository.GetTrainingByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int trainingId) =>
                {
                    return trainingList.FirstOrDefault(training => training.TrainingID == trainingId);
                });

            _trainingService = new TrainingService(_stubTrainingRepository.Object);
        }

        [TestCase(2, ExpectedResult =null)]
        public async Task<Training> GetTrainingByIdAsync_InvalidId_ReturnsNull(int trainingId)
        {
            // Act
            var result = await _trainingService.GetAllTrainingByIdAsync(trainingId);

            // Assert
           return result;
        }

        [TestCase(1)]
        public async Task GetTrainingByIdAsync_ValidId_ReturnsTraining(int trainingId)
        {
            // Act
            var result = await _trainingService.GetAllTrainingByIdAsync(trainingId);

            //Assert
            Assert.IsInstanceOf<Training>(result);
        }

    }
}
