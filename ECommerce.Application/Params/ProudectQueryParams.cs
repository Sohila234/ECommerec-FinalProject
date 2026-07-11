using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace ECommerce.Application.Params
{
    public class ProudectQueryParams
    {
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string? SearchValue { get; set; }

        public ProductSortingOptions Sort { get; set; }

        private const int DefaultPageSize = 5;
        private const int MaxPageSize = 10;
        private int pageSize = DefaultPageSize;
        public int PageIndex { get; set; } = 1;
        public int PageSize {
            get => pageSize;
            set => pageSize = value > MaxPageSize ? MaxPageSize : (value < 1 ? DefaultPageSize : value);
        }


    }
}
