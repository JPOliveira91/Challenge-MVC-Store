using Challenge_MVC_Store.Data.Models;
using Challenge_MVC_Store.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace Challenge_MVC_Store.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IRepository<Product> _productRepository;

        public ProductsController(IRepository<Product> ProductRepository)
        {
            _productRepository = ProductRepository;
        }

        // GET: api/Products
        /// <summary>
        /// Obtém uma lista paginada de produtos
        /// </summary>
        /// <param name="id">Filtro por id do produto (opcional)</param>
        /// <param name="page">Número da página (padrão: 1)</param>
        /// <param name="pageSize">Itens por página (padrão: 10)</param>
        /// <returns>Lista de produtos</returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Obtém uma lista paginada de produtos")]
        [SwaggerResponse(200, "Lista de produtos retornada com sucesso", typeof(IEnumerable<Product>))]
        [SwaggerResponse(400, "Parâmetros inválidos")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] int? id = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            (int totalCount, int totalPages, List<Product> result) = await GetPagedProductsList(id, page, pageSize);

            string response = JsonSerializer.Serialize(new
            {
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = page,
                TotalPages = totalPages
            });

            Response.Headers.Append("X-Pagination", response);

            return Ok(result);
        }

        private async Task<(int totalCount, int totalPages, List<Product> result)> GetPagedProductsList(int? id, int page, int pageSize)
        {
            IEnumerable<Product> products = await _productRepository.GetAllAsync();

            if (id != null)
            {
                products = products.Where(p => p.Id == id);
            }

            int totalCount = products.Count();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            List<Product> result = products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(product => product.Name)
                .ToList();

            return (totalCount, totalPages, result);
        }
    }
}