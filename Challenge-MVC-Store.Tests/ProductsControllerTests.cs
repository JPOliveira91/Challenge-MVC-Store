using Challenge_MVC_Store.API.Controllers;
using Challenge_MVC_Store.API.UseCase;
using Challenge_MVC_Store.Data.Models;
using Challenge_MVC_Store.Data.Repositories.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Text.Json;

namespace Challenge_MVC_Store.Tests
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mockRepository = new Mock<IProductRepository>();

            DefaultHttpContext httpContext = new();

            ControllerContext controllerContext = new()
            {
                HttpContext = httpContext
            };

            ProductsUseCase productsUseCase = new ProductsUseCase(_mockRepository.Object);

            _controller = new ProductsController(productsUseCase)
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
                new Product { Id = 1, Name = "Produto A", Price = 10.00m },
                new Product { Id = 2, Name = "Produto B", Price = 20.00m },
                new Product { Id = 3, Name = "Produto C", Price = 30.00m }
            ];

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

            // Act
            OkObjectResult? result = (await _controller.GetProducts(null, 1, 2)).Result as OkObjectResult;

            // Assert
            Assert.NotNull(result);

            IEnumerable<ProductDto>? returnedProducts = result.Value as IEnumerable<ProductDto>;
            Assert.NotNull(returnedProducts);
            Assert.Equal(2, returnedProducts.Count());
        }

        [Fact]
        public async Task GetProducts_ReturnsFilteredProducts_WhenIdIsProvided()
        {
            // Arrange
            List<Product> products =
            [
                new Product { Id = 1, Name = "Produto A", Price = 10.00m },
                new Product { Id = 2, Name = "Produto B", Price = 20.00m }
            ];

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

            // Act
            OkObjectResult? result = (await _controller.GetProducts(1, 1, 10)).Result as OkObjectResult;

            // Assert
            Assert.NotNull(result);

            IEnumerable<ProductDto>? returnedProducts = result.Value as IEnumerable<ProductDto>;
            Assert.NotNull(returnedProducts);
            Assert.Single(returnedProducts);
            Assert.Equal(1, returnedProducts.First().Id);
        }

        [Fact]
        public async Task GetProducts_ReturnsEmptyList_WhenNoProductsExist()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetQueryable()).Returns(Enumerable.Empty<Product>().AsQueryable());

            // Act
            OkObjectResult? result = (await _controller.GetProducts(1, 1, 10)).Result as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            IEnumerable<ProductDto>? returnedProducts = result.Value as IEnumerable<ProductDto>;
            Assert.NotNull(returnedProducts);
            Assert.Empty(returnedProducts);
        }

        [Fact]
        public async Task GetProducts_ReturnsBadRequest_WhenPageIsInvalid()
        {
            // Act
            ActionResult<IEnumerable<ProductDto>> result = await _controller.GetProducts(null, -1, 10);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetProducts_ReturnsBadRequest_WhenPageSizeIsInvalid()
        {
            // Act
            ActionResult<IEnumerable<ProductDto>> result = await _controller.GetProducts(null, 1, 0);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetProducts_MapsProductToProductDtoCorrectly()
        {
            // Arrange
            List<Product> products =
            [
                new Product
                {
                    Id = 1,
                    Name = "Product A",
                    Price = 10.00m,
                    OrderProducts =
                    [
                        new() {
                            Order = new Order
                            {
                                Id = 1,
                                Date = DateTime.Now,
                                CustomerId = 1,
                                Customer = new Customer
                                {
                                    Id = 1,
                                    Name = "André Matos",
                                    Email = "amatos@teste.com"
                                }
                            },
                            Quantity = 2,
                            UnitPrice = 10.00m
                        }
                    ]
                }
            ];

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

            // Act
            OkObjectResult? result = (await _controller.GetProducts(1, 1, 10)).Result as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            IEnumerable<ProductDto>? returnedProducts = result.Value as IEnumerable<ProductDto>;
            Assert.NotNull(returnedProducts);
            ProductDto productDto = returnedProducts.First();
            Assert.Equal(1, productDto.Id);
            Assert.Equal("Product A", productDto.Name);
            Assert.Equal(10.00m, productDto.Price);
            Assert.Single(productDto.Orders);
            OrderDto orderDto = productDto.Orders.First();
            Assert.Equal(1, orderDto.Id);
            Assert.Equal(2, orderDto.Quantity);
            Assert.Equal(10.00m, orderDto.UnitPrice);
            Assert.Equal(1, orderDto.CustomerId);
            Assert.Equal("André Matos", orderDto.CustomerName);
            Assert.Equal("amatos@teste.com", orderDto.CustomerEmail);
        }

        [Fact]
        public async Task GetProducts_SetsPaginationHeaders()
        {
            // Arrange
            List<Product> products =
            [
                new Product { Id = 1, Name = "Produto A", Price = 10.00m },
                new Product { Id = 2, Name = "Produto B", Price = 20.00m },
                new Product { Id = 3, Name = "Produto C", Price = 30.00m }
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