using ECommerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Contracts
{
    public interface IGenaricRepository<TEntity , TKey> where TEntity :BaseEntity<TKey>
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<TEntity?> GetByIdAsync(TKey Id, CancellationToken ct=default);
        Task<TEntity?> GetByIdWithSpecificationsAsync(ISpecificatios<TEntity, TKey> spacificatios, CancellationToken ct = default);
        Task <IReadOnlyList<TEntity>> GetAllAsync( CancellationToken ct);
        Task<IReadOnlyList<TEntity>> GetAllWithSpecificationsAsync(ISpecificatios<TEntity,TKey> spacificatios, CancellationToken ct);
        Task<int> GetProductCountWithSpecificayionsAsync(ISpecificatios<TEntity, TKey> spacificatios, CancellationToken ct);




    }
}
