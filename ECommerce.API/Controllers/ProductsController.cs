using ECommerce.API.Attributes;
using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTO_S.Products;
using ECommerce.Application.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    
    public class ProductsController (IProductServices productServices): ApiBaseController
    {       
        [HttpGet]
        [Authorize]
        [RedisCache(5000)]
        public async Task<ActionResult<PaginatedResult<ProductDto>>> GetAllProducts ([FromQuery]ProudectQueryParams  queryParams  , CancellationToken ct)
        {
            var Products = await productServices.GetAllProductsAsync (queryParams, ct);
            return ToActionResult < PaginatedResult<ProductDto>>(Products) ;

        }
        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<BrandDto>>> GetAllBrands(CancellationToken ct)
        {
            var Brands = await productServices.GetAllProductBrandsAsync();
            var result = ToActionResult(Brands);
            return result;

        }
        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<TypeDto>>> GetAllTypes(CancellationToken ct)
        {
            var Types = await productServices.GetAllProductTypsAsync();
            var result = ToActionResult(Types);
            return result;

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int Id ,CancellationToken ct)
        {
            var Product = await productServices.GetByIdAsync(Id );
            var result = ToActionResult(Product);
            return result;


        }

    }
}
