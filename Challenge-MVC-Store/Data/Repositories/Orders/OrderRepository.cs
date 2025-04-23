using Challenge_MVC_Store.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Challenge_MVC_Store.Data.Repositories.Orders
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Order?> GetByIdWithProductsAsync(int id)
        {
            return await _context.Orders
                .Include(p => p.OrderProducts)
                .ThenInclude(pp => pp.Product)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}