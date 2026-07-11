using ECommerce.Application.Params;
using ECommerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Specifications
{
    public class ProductSpecification : BaseSpecification<Product,int>
    {
        public ProductSpecification(ProudectQueryParams queryParams) :
            base(b => (!queryParams.BrandId.HasValue || b.BrandId == queryParams.BrandId)
            && (!queryParams.TypeId.HasValue || b.TypeId == queryParams.TypeId)
            && (string.IsNullOrEmpty(queryParams.SearchValue) || b.Name.ToLower().Contains(queryParams.SearchValue.ToLower())))
        {
            AddInclude(a => a.Type);
            AddInclude(a=>a.Brand);
            switch(queryParams.Sort)
            {
                case ProductSortingOptions.NameAsc: AddOrderBy(p => p.Name);break;
                case ProductSortingOptions.NameDesc: AddOrderByDesc(p => p.Name); break;
                case ProductSortingOptions.PriceAsc: AddOrderBy(p => p.Price); break;
                case ProductSortingOptions.PriceDesc: AddOrderByDesc(p => p.Price); break;

                _: break;
            }
            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
        }
        public ProductSpecification(int Id) : base(b =>b .Id==Id)
        {
            AddInclude(a => a.Type);
            AddInclude(a => a.Brand);
        }
    }
}
