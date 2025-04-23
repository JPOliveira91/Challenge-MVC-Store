using Challenge_MVC_Store.Data;
using Challenge_MVC_Store.Data.Models;
using Challenge_MVC_Store.Data.Repositories.Orders;
using Microsoft.EntityFrameworkCore;

namespace Challenge_MVC_Store.Tests
{
    public class OrderRepositoryTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public OrderRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetByIdWithProductsAsync_ReturnsOrderWithProducts_WhenOrderExists()
        {
            // Arrange
            await using ApplicationDbContext context = new(_dbContextOptions);
            OrderRepository repository = new(context);

            Product product = new() { Id = 1, Name = "Produto A", Price = 10.00m };
            Order order = new()
            {
                Id = 1,
                Date = DateTime.Now,
                CustomerId = 1,
                OrderProducts =
                [
                    new OrderProduct { ProductId = 1, Quantity = 2, UnitPrice = 10.00m, Product = product }
                ]
            };

            context.Products.Add(product);
            context.Orders.Add(order);
            await context.SaveChangesAsync();

            // Act
            Order? result = await repository.GetByIdWithProductsAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Single(result.OrderProducts);
            Assert.Equal("Produto A", result.OrderProducts.First().Product.Name);
        }

        [Fact]
        public async Task GetByIdWithProductsAsync_ReturnsNull_WhenOrderDoesNotExist()
        {
            // Arrange
            await using ApplicationDbContext context = new(_dbContextOptions);
            OrderRepository repository = new(context);

            // Act
            Order? result = await repository.GetByIdWithProductsAsync(999);

            // Assert
            Assert.Null(result);
        }
    }
}