using Challenge_MVC_Store.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Challenge_MVC_Store.Data.Repositories.Products
{
    public class ProductRepository : Repository<Order>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        // Option 1: Add include to existing method
        public new async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.OrderProducts)
                .ThenInclude(op => op.Order)
                .ThenInclude(o => o.Customer)
                .ToListAsync();
        }

        public IQueryable<Product> GetQueryable()
        {
            return _context.Products.AsQueryable();
        }
    }
}