using GraniteCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyCars.Data
{
    // GraniteCore install
    public class MockRepository<TDtoModel, TEntity, TPrimaryKey, TUserID> : IBaseRepository<TDtoModel, TEntity, TPrimaryKey, TUserID>
        where TDtoModel : class, IDto<TPrimaryKey>, new()
        where TEntity : class, IBaseIdentityModel<TPrimaryKey>, new()
    {
        private readonly IList<TEntity> _store = new List<TEntity>();
        protected readonly IGraniteMapper _mapper;

        public MockRepository(
            IGraniteMapper mapper
            )
        {
            _mapper = mapper;
        }


        public Task<TDtoModel> Create(TDtoModel entity, TUserID userId)
        {
            return Task.Run(() =>
            {
                _store.Add(_mapper.Map<TDtoModel, TEntity>(entity));

                return entity;
            });
        }

        public Task Delete(TPrimaryKey id, TUserID userId)
        {
            return Task.Run(() =>
            {
                var ent = _store.FirstOrDefault(x => x.ID.Equals(id));
                if (ent != null)
                    _store.Remove(ent);
            });
        }

        public IQueryable<TDtoModel> GetAll()
        {
            return  _mapper.Map<TEntity, TDtoModel>(_store.AsQueryable());
        }

        public Task<TDtoModel> GetById(TPrimaryKey id)
        {
            return Task.Run(() =>
            {
                return _mapper.Map<TEntity, TDtoModel>(_store.FirstOrDefault(x => x.ID.Equals(id)));
            });
        }

        public Task<TDtoModel> GetById(TPrimaryKey id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            throw new NotImplementedException("Parameter 'includeProperties' not supported");
        }

        public Task Update(TPrimaryKey id, TDtoModel entity, TUserID userId)
        {
            throw new NotImplementedException();
        }
    }
}
