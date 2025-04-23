using Challenge_MVC_Store.API.UseCase;
using Challenge_MVC_Store.Data.Models;
using Challenge_MVC_Store.Data.Repositories.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace Challenge_MVC_Store.API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsUseCase _productsUseCase;

        public ProductsController(IProductsUseCase productsUseCase)
        {
            _productsUseCase = productsUseCase;
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
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts([FromQuery] int? id = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1)
            {
                return BadRequest("Número da página necessita ser maior que 0.");
            }

            if (pageSize < 1)
            {
                return BadRequest("Tamanho da página necessita ser maior que 0.");
            }

            try
            {
                (int totalCount, int totalPages, List<ProductDto> result) = await _productsUseCase.GetPagedProductsList(id, page, pageSize);

                // Additional validation for the returned data
                if (page > 1 && page > totalPages)
                {
                    return BadRequest($"Página {page} requisitada execede o total de {totalPages} páginas.");
                }

                var paginationMetadata = new
                {
                    TotalCount = totalCount,
                    PageSize = pageSize,
                    CurrentPage = page,
                    TotalPages = totalPages,
                    HasPrevious = page > 1,
                    HasNext = page < totalPages
                };

                Response.Headers.Append("X-Pagination",
                    JsonSerializer.Serialize(paginationMetadata));

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
    }
}