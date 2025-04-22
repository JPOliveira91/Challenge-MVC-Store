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

        public async Task IncludeProductToOrderAsync(int orderId, int produtoId, int quantity)
        {
            Product? product = await _context.Products.FindAsync(produtoId);
            if (product == null) throw new ArgumentException("Produto não encontrado");

            Order? order = await _context.Orders.FindAsync(orderId);
            if (order == null) throw new ArgumentException("Pedido não encontrado");

            OrderProduct orderProduct = new()
            {
                OrderId = orderId,
                ProductId = produtoId,
                Quantity = quantity,
                UnitPrice = product.Price
            };

            _context.OrderProducts.Add(orderProduct);

            await _context.SaveChangesAsync();
        }
    }
}