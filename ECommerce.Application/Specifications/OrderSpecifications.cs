using ECommerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Specifications
{
    public class OrderSpecifications : BaseSpecification<Order, Guid>
    {
        public OrderSpecifications(string email)
            : base(o => o.BuyerEmail == email)
        {
            AddInclude(o => o.DeliveryMethod);
            AddOrderByDesc(o => o.OrderDate);
        }

        public OrderSpecifications(Guid id, string email)
            : base(o => o.Id == id && o.BuyerEmail == email)
        {

        }
    }
}
