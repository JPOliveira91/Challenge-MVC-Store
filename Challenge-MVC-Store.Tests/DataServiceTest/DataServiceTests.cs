using Challenge_MVC_Store.Services.LogService;
using Moq;
using Challenge_MVC_Store.Services.DataService;

namespace Challenge_MVC_Store.Tests.DataServiceTest
{
    public class DataServiceTests
    {
        [Fact]
        public async Task ProcessDataAsync_ShouldCallLogServiceOnce()
        {
            // Arrange
            Mock<ILogService> mockLogService = new();
            DataService dataService = new(mockLogService.Object);

            // Act
            await dataService.ProcessDataAsync();

            // Assert
            mockLogService.Verify(x => x.Log(It.IsAny<string>()), Times.Once());
        }
    }
}