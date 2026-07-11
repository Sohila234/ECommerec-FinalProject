using ECommerce.Domain.Common;
using ECommerce.Domain.Contracts;
using ECommerce.Infrastructure.Data;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Repsitories
{
    public class UnitOfWork(StoreDBContext dBContext) : IUnitOfWork
    {
        private readonly Dictionary<string, object> _Repos = [];

        public IGenaricRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var TypeName = typeof(TEntity).Name;
            if(_Repos.TryGetValue(TypeName, out object oldRepo))
            {
                return (IGenaricRepository<TEntity,TKey>) oldRepo;
            }
            var NewRepo =new GenaricRepsitory<TEntity,TKey>(dBContext);
            _Repos[TypeName] = NewRepo;
            return NewRepo;
        }

        public async Task<int> SaveChangesSync(CancellationToken ct)
        {
            return await dBContext.SaveChangesAsync(ct);
        }
    }
}
