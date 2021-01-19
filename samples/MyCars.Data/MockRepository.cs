using GraniteCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyCars.Data
{
    // GraniteCore install
    public class MockRepository<TBaseEntityModel, TPrimaryKey> : IBaseRepository<TBaseEntityModel, TPrimaryKey>
        where TBaseEntityModel : class, IBaseIdentityModel<TPrimaryKey>, new()
    {
        private readonly IList<TBaseEntityModel> _store = new List<TBaseEntityModel>();

        public MockRepository()
        {
        }


        public Task<TBaseEntityModel> Create(TBaseEntityModel entity)
        {
            return Task.Run(() =>
            {
                _store.Add(entity);

                return entity;
            });
        }

        public Task Delete(TPrimaryKey id)
        {
            return Task.Run(() =>
            {
                var ent = _store.FirstOrDefault(x => x.ID.Equals(id));
                if (ent != null)
                    _store.Remove(ent);
            });
        }

        public IQueryable<TBaseEntityModel> GetAll()
        {
            return  _store.AsQueryable();
        }

        public Task<TBaseEntityModel> GetByID(TPrimaryKey id)
        {
            return Task.Run(() =>
            {
                return _store.FirstOrDefault(x => x.ID.Equals(id));
            });
        }

        public Task<TBaseEntityModel> GetByID(TPrimaryKey id, params Expression<Func<TBaseEntityModel, object>>[] includeProperties)
        {
            throw new NotImplementedException("Parameter 'includeProperties' not supported");
        }

        public Task Update(TPrimaryKey id, TBaseEntityModel entity)
        {
            throw new NotImplementedException();
        }
    }
}
