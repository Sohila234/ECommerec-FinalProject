using AutoMapper;
using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTO_S.Products;
using ECommerce.Application.Params;
using ECommerce.Application.Specifications;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services
{
    public class ProductServices : IProductServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ProductServices( IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<Result<IReadOnlyList<BrandDto>>> GetAllProductBrandsAsync(CancellationToken ct = default)
        {
            var Brands = await unitOfWork.GetRepository<ProductsBrand, int>().GetAllAsync(ct);
            var MappedBrand = mapper.Map<IReadOnlyList<ProductsBrand>,IReadOnlyList<BrandDto>>(Brands);
            return Result<IReadOnlyList<BrandDto>>.Ok(MappedBrand);
        }

        public async Task<Result<PaginatedResult<ProductDto>>> GetAllProductsAsync(ProudectQueryParams queryParams, CancellationToken ct = default)
        {
            var spec = new ProductSpecification(queryParams);

            var products = await unitOfWork.GetRepository<Product, int>().GetAllWithSpecificationsAsync(spec,ct);
            var MapperProdect = mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDto>>(products);
            var CountSpe= new ProductCountSpecification(queryParams);
            var TotalCount = await unitOfWork.GetRepository<Product, int>().GetProductCountWithSpecificayionsAsync(CountSpe, ct);
            return Result<PaginatedResult<ProductDto>>.Ok(new PaginatedResult<ProductDto>(MapperProdect,queryParams.PageIndex, products.Count, TotalCount));
        }

        public async  Task<Result<IReadOnlyList<TypeDto>>> GetAllProductTypsAsync(CancellationToken ct = default)
        {
            var Types = await unitOfWork.GetRepository<ProductsType, int>().GetAllAsync(ct);

            var MapperTypes = mapper.Map<IReadOnlyList<ProductsType>, IReadOnlyList<TypeDto>>(Types);
            return Result<IReadOnlyList<TypeDto>>.Ok(MapperTypes);
        }

       

        public async Task<Result<ProductDto>> GetByIdAsync(int Id, CancellationToken ct = default)
        {
            var spec = new ProductSpecification(Id);

            var Product = await unitOfWork.GetRepository<Product,int>().GetByIdWithSpecificationsAsync(spec, ct);
            if (Product is null)
                return Result<ProductDto>.fail(Error.NotFound("Product.NotFound", $"Product With Id {Id} is not found"));
            var MapperProduct = mapper.Map<Product,ProductDto>(Product);
            return MapperProduct;

        }
    }
}
