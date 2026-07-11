using ECommerce.Application.Params;
using ECommerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Specifications
{
    public class ProductCountSpecification :BaseSpecification<Product,int>
    {
        public ProductCountSpecification(ProudectQueryParams queryParams) :
            base(b => (!queryParams.BrandId.HasValue || b.BrandId == queryParams.BrandId)
            && (!queryParams.TypeId.HasValue || b.TypeId == queryParams.TypeId)
            && (string.IsNullOrEmpty(queryParams.SearchValue) || b.Name.ToLower().Contains(queryParams.SearchValue.ToLower())))
        { }
        

    }
}
