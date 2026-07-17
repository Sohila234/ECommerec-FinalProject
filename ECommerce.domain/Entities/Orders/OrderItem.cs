using ECommerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Entities.Orders
{
    public class OrderItem:BaseEntity<int>
    {
        public ProductItemOrder Product { get; set; } = default!;

        public decimal Price;
        public int quantity { get; set; }

    }
}
