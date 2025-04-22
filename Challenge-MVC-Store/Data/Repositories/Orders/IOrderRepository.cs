using Challenge_MVC_Store.Data.Models;

namespace Challenge_MVC_Store.Data.Repositories.Orders
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order?> GetByIdWithProductsAsync(int id);

        Task IncludeProductToOrderAsync(int orderId, int productId, int quantity);
    }
}