using Challenge_MVC_Store.Controllers;
using Challenge_MVC_Store.Data.Models;
using Challenge_MVC_Store.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Text.Json;

namespace Challenge_MVC_Store.Tests
{
    public class ProductsControllerTests
    {
        private readonly Mock<IRepository<Product>> _mockRepository;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mockRepository = new Mock<IRepository<Product>>();

            // Create mock HTTP context
            DefaultHttpContext httpContext = new();

            // Create controller context
            ControllerContext controllerContext = new()
            {
                HttpContext = httpContext
            };

            _controller = new ProductsController(_mockRepository.Object)
            {
                ControllerContext = controllerContext
            };
        }

        [Fact]
        public async Task GetProducts_ReturnsPaginatedProducts_WhenNoIdIsProvided()
        {
            // Arrange
            List<Product> products =
            [
                new Product { Id = 1, Name = "Product A", Price = 10.00m },
                new Product { Id = 2, Name = "Product B", Price = 20.00m },
                new Product { Id = 3, Name = "Product C", Price = 30.00m }
            ];

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

            // Act
            OkObjectResult? result = (await _controller.GetProducts(null, 1, 2)).Result as OkObjectResult;

            // Assert
            Assert.NotNull(result);

            IEnumerable<Product>? returnedProducts = result.Value as IEnumerable<Product>;
            Assert.NotNull(returnedProducts);
            Assert.Equal(2, returnedProducts.Count());
        }

        [Fact]
        public async Task GetProducts_ReturnsFilteredProducts_WhenIdIsProvided()
        {
            // Arrange
            List<Product> products =
            [
                new Product { Id = 1, Name = "Product A", Price = 10.00m },
                new Product { Id = 2, Name = "Product B", Price = 20.00m }
            ];

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

            // Act
            OkObjectResult? result = (await _controller.GetProducts(1, 1, 10)).Result as OkObjectResult;

            // Assert
            Assert.NotNull(result);

            IEnumerable<Product>? returnedProducts = result.Value as IEnumerable<Product>;
            Assert.NotNull(returnedProducts);
            Assert.Single(returnedProducts);
            Assert.Equal(1, returnedProducts.First().Id);
        }

        [Fact]
        public async Task GetProducts_SetsPaginationHeaders()
        {
            // Arrange
            List<Product> products =
            [
                new Product { Id = 1, Name = "Product A", Price = 10.00m },
                new Product { Id = 2, Name = "Product B", Price = 20.00m },
                new Product { Id = 3, Name = "Product C", Price = 30.00m }
            ];

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

            // Act
            OkObjectResult? result = (await _controller.GetProducts(null, 1, 2)).Result as OkObjectResult;

            // Assert

            Assert.NotNull(result);

            Microsoft.Extensions.Primitives.StringValues paginationHeader = _controller.Response.Headers["X-Pagination"];
            Assert.False(string.IsNullOrEmpty(paginationHeader));

            PaginationMetadata? paginationMetadata = JsonSerializer.Deserialize<PaginationMetadata>(paginationHeader);
            Assert.NotNull(paginationMetadata);
            Assert.Equal(1, paginationMetadata.CurrentPage);
            Assert.Equal(2, paginationMetadata.PageSize);
            Assert.Equal(3, paginationMetadata.TotalCount);
            Assert.Equal(2, paginationMetadata.TotalPages);
        }
    }

    public class PaginationMetadata
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}