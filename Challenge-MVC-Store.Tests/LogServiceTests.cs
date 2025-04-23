using Challenge_MVC_Store.Services.LogService;

namespace Challenge_MVC_Store.Tests
{
    public class LogServiceTests
    {
        [Fact]
        public void Log_ShouldWriteMessageToConsole()
        {
            // Arrange
            LogService logService = new();
            string message = "Mensagem teste";
            string expectedOutput = $"Log - {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";

            using StringWriter stringWriter = new();
            Console.SetOut(stringWriter);

            // Act
            logService.Log(message);

            // Assert
            string output = stringWriter.ToString().Trim();
            Assert.StartsWith("Log - ", output);
            Assert.Contains(message, output);
        }
    }
}