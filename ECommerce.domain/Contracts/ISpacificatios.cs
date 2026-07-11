using ECommerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

namespace ECommerce.Domain.Contracts
{
    public interface ISpecificatios<TEntity , TKey> where TEntity : BaseEntity<TKey>
    {
        List<Expression<Func<TEntity, object>>> IncludeExpressions { get; }
        Expression<Func<TEntity,bool>> Criteria { get; }
        Expression<Func<TEntity,object>>? OrderBy { get; }
        Expression<Func<TEntity, object>>? OrderByDesc { get; }
        int Skip { get; }
        int Take { get; }
        bool IsPaginated { get; }

    }
}
