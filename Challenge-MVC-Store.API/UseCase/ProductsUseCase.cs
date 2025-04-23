using Challenge_MVC_Store.Data.Models;
using Challenge_MVC_Store.Data.Repositories.Products;

namespace Challenge_MVC_Store.API.UseCase
{
    public class ProductsUseCase : IProductsUseCase
    {
        private readonly IProductRepository _productRepository;

        public ProductsUseCase(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<(int totalCount, int totalPages, List<ProductDto> result)> GetPagedProductsList(int? id, int page, int pageSize)
        {
            IEnumerable<Product> products = await _productRepository.GetAllAsync();

            if (id != null)
            {
                products = products.Where(p => p.Id == id);
            }

            int totalCount = products.Count();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            List<ProductDto> result = [.. products
                .OrderBy(product => product.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => MapToProductDto(p))];

            return (totalCount, totalPages, result);
        }

        // Separate mapping function for better maintainability
        private static ProductDto MapToProductDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Orders = [.. product.OrderProducts
                    .Select(op => new OrderDto
                    {
                        Id = op.Order.Id,
                        Date = op.Order.Date,
                        Quantity = op.Quantity,
                        UnitPrice = op.UnitPrice,
                        CustomerId = op.Order.CustomerId,
                        CustomerName = op.Order.Customer.Name,
                        CustomerEmail = op.Order.Customer.Email
                    })]
            };
        }
    }
}