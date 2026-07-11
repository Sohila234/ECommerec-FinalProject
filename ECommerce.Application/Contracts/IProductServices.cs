using ECommerce.Application.Common;
using ECommerce.Application.DTO_S.Products;
using ECommerce.Application.Params;
using ECommerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Contracts
{
    public interface IProductServices
    {
        Task<Result<PaginatedResult<ProductDto>>> GetAllProductsAsync(ProudectQueryParams queryParams , CancellationToken ct = default);
        Task<Result<IReadOnlyList<BrandDto>>> GetAllProductBrandsAsync(CancellationToken ct = default);
        Task<Result<IReadOnlyList<TypeDto>>> GetAllProductTypsAsync( CancellationToken ct = default);

        Task<Result<ProductDto>> GetByIdAsync (int Id , CancellationToken ct = default);



    }
}
