using Challenge_MVC_Store.Data.Models;

namespace Challenge_MVC_Store.Data.Repositories.Products
{
    public interface IProductRepository
    {
        IQueryable<Product> GetQueryable();

        Task<IEnumerable<Product>> GetAllAsync();
    }
}