using ECommerce.Domain.Common;
using ECommerce.Domain.Contracts;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Repsitories
{
    public class GenaricRepsitory<TEntity, Tkey> : IGenaricRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        private readonly StoreDBContext storeDBContext;

        public GenaricRepsitory( StoreDBContext storeDBContext)
        {
            this.storeDBContext = storeDBContext;
        }

        public void Add(TEntity entity)
        {
            storeDBContext.Set<TEntity>().Add(entity);
        }

        public void Delete(TEntity entity)
        {
            storeDBContext.Set<TEntity>().Remove(entity);
        }
        public void Update(TEntity entity)
        {
            storeDBContext.Set<TEntity>().Update(entity);
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct)
        {
            return await storeDBContext.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(Tkey Id, CancellationToken ct = default)
        {
            return await storeDBContext.Set<TEntity>().FindAsync(Id, ct);
       }

        public async Task<IReadOnlyList<TEntity>> GetAllWithSpecificationsAsync(ISpecificatios<TEntity, Tkey> spacificatios, CancellationToken ct)
        {
            var Result = SpecificationEvaluator.CreateQuery<TEntity, Tkey>(storeDBContext.Set<TEntity>(), spacificatios);
            return await Result.ToListAsync(ct);
        }

        public async Task<TEntity?> GetByIdWithSpecificationsAsync(ISpecificatios<TEntity, Tkey> spacificatios, CancellationToken ct = default)
        {
            var Result = SpecificationEvaluator.CreateQuery<TEntity, Tkey>(storeDBContext.Set<TEntity>(), spacificatios);
            return await Result.FirstOrDefaultAsync(ct);
        }

        public async  Task<int> GetProductCountWithSpecificayionsAsync(ISpecificatios<TEntity, Tkey> spacificatios, CancellationToken ct)
        {
            var Result = SpecificationEvaluator.CreateQuery<TEntity, Tkey>(storeDBContext.Set<TEntity>(), spacificatios);
            return await Result.CountAsync(ct);
        }
    }
}
