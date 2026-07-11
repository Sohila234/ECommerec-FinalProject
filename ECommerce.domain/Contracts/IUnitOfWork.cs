using ECommerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Contracts
{
    public interface IUnitOfWork
    {
         Task<int> SaveChangesSync (CancellationToken ct);
        IGenaricRepository<TEntity,TKey>  GetRepository<TEntity,TKey> () where TEntity : BaseEntity<TKey>;

    }
}
