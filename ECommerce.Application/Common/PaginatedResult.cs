using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Common
{
    public class PaginatedResult<TEntity>
    {
        public PaginatedResult(IReadOnlyList<TEntity> data, int pageIndex, int pageSize, int count)
        {
            Data = data;
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
        }

        public IReadOnlyList<TEntity> Data { get; set; } = [];
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }


    }
}
