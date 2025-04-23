using Challenge_MVC_Store.Controllers;
using Challenge_MVC_Store.Data.Models;
using Challenge_MVC_Store.Data.Repositories.Customers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;

namespace Challenge_MVC_Store.Tests
{
    public class CustomersControllerTests
    {
        private readonly Mock<ILogger<CustomersController>> _mockLogger;
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly CustomersController _controller;

        public CustomersControllerTests()
        {
            _mockLogger = new Mock<ILogger<CustomersController>>();
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _controller = new CustomersController(_mockLogger.Object, _mockCustomerRepository.Object);
            _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), new Mock<ITempDataProvider>().Object);
        }

        [Fact]
        public void Create_Get_ReturnsViewResult()
        {
            // Act
            IActionResult result = _controller.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Create_Post_RedirectsToCreate_WhenModelStateIsValid()
        {
            // Arrange
            Customer customer = new Customer
            {
                Name = "Neymar Júnior",
                Email = "njunior@teste.com"
            };

            _mockCustomerRepository.Setup(repo => repo.CreateAsync(customer)).Returns(Task.CompletedTask);
            _mockCustomerRepository.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            IActionResult result = await _controller.Create(customer);

            // Assert
            RedirectToActionResult redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Create", redirectResult.ActionName);
            Assert.Equal("Cliente salvo com sucesso!", _controller.TempData["SuccessMessage"]);
        }

        [Theory]
        [InlineData(null, "email@test.com", "O nome é obrigatório.")]
        [InlineData("Edson Arantes", null, "O email é obrigatório.")]
        [InlineData("Edson Arantes", "invalid-email", "Por favor, insira um email válido.")]
        public async Task Create_Post_ReturnsViewResult_WhenModelStateIsInvalid(string? name, string? email, string errorMessage)
        {
            // Arrange
            Customer customer = new Customer
            {
                Name = name,
                Email = email
            };

            // Act
            IActionResult result = await _controller.Create(customer);

            // Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(customer, viewResult.Model);
            Assert.Equal(errorMessage, _controller.TempData["ErrorMessage"]);
        }

        [Fact]
        public async Task Create_Post_ReturnsViewResult_WhenExceptionIsThrown()
        {
            // Arrange
            Customer customer = new Customer
            {
                Name = "Drew Brees",
                Email = "dbrees@teste.com"
            };

            _mockCustomerRepository.Setup(repo => repo.CreateAsync(customer)).ThrowsAsync(new Exception());

            // Act
            IActionResult result = await _controller.Create(customer);

            // Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(customer, viewResult.Model);
            Assert.Equal("Erro ao salvar o cliente.", _controller.TempData["ErrorMessage"]);
        }
    }
}