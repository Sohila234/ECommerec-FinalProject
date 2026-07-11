using ECommerce.Domain.Common;
using ECommerce.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ECommerce.Infrastructure.Repsitories
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> CreateQuery <TEntity,TKey> (IQueryable<TEntity> InputQuery ,ISpecificatios <TEntity , TKey> spacifications)
            where TEntity : BaseEntity<TKey>
        {
            var Query = InputQuery;
            if (spacifications.IncludeExpressions.Count > 0)
                Query = spacifications.IncludeExpressions.Aggregate(Query, (current, expression) => current.Include(expression));
            if(spacifications.Criteria is  not null)
                Query=Query.Where(spacifications.Criteria);
            if(spacifications.OrderBy is not null)
                Query= Query.OrderBy(spacifications.OrderBy);
            if (spacifications.OrderByDesc is not null)
                Query = Query.OrderByDescending(spacifications.OrderByDesc);
            if(spacifications.IsPaginated)
                Query=Query.Skip(spacifications.Skip).Take(spacifications.Take);

            return Query;

        }
    }
}
