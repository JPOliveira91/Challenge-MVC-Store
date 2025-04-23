using Challenge_MVC_Store.Data.Models;

namespace Challenge_MVC_Store.API.UseCase
{
    public interface IProductsUseCase
    {
        Task<(int totalCount, int totalPages, List<ProductDto> result)> GetPagedProductsList(int? id, int page, int pageSize);
    }
}