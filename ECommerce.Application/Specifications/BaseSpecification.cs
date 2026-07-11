using ECommerce.Domain.Common;
using ECommerce.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ECommerce.Application.Specifications
{
    public abstract class BaseSpecification<TEntity, Tkey> : ISpecificatios<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        #region Where
        public Expression<Func<TEntity, bool>> Criteria { get; private set; }
        protected BaseSpecification(Expression<Func<TEntity, bool>> criteria = null)
        {
            Criteria = criteria;
        }

        #endregion
        #region Include
        public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; private set; } = [];


        public void AddInclude(Expression<Func<TEntity, object>> expression)
        {
            IncludeExpressions.Add(expression);
        }
        #endregion
        #region Sorting
        public Expression<Func<TEntity, object>>? OrderBy { get; private set; }
        public void AddOrderBy(Expression<Func<TEntity, object>>? orderBy)
            => OrderBy = orderBy;

        public Expression<Func<TEntity, object>>? OrderByDesc { get; private set; }


        public void AddOrderByDesc(Expression<Func<TEntity, object>>? orderByDesc)
            => OrderByDesc = orderByDesc;
        #endregion
        #region Paginated
        public int Skip { get; private set; }

        public int Take { get; private set; }

        public bool IsPaginated { get; private set; }
        public void ApplyPagination (int PageSize , int PageIndex)
        {
            IsPaginated = true;
            Take = PageSize;
            Skip = (PageIndex - 1) * PageSize;
        }
        #endregion
    }
}
