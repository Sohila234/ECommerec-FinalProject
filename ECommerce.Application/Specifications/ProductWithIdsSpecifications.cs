using ECommerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Specifications
{
    public class ProductWithIdsSpecifications : BaseSpecification<Product, int>
    {
        public ProductWithIdsSpecifications(IEnumerable<int> Ids)
            : base(P => Ids.Contains(P.Id))
        {

        }

    }
}
