using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace GraniteCore.EntityFrameworkCore
{
    public class BaseRepository<TBaseEntityModel, TPrimaryKey> : IBaseRepository<TBaseEntityModel, TPrimaryKey>
        where TBaseEntityModel : class, IBaseIdentityModel<TPrimaryKey>, new()
    {
        protected internal readonly IGraniteMapper Mapper;
        protected internal readonly DbContext DbContext;

        public BaseRepository(
            DbContext dbContext,
            IGraniteMapper mapper
            )
        {
            DbContext = dbContext;
            Mapper = mapper;
        }

        #region Public CRUD methods
        public virtual IQueryable<TBaseEntityModel> GetAll()
        {
            var set = DbContext.Set<TBaseEntityModel>()
                            .AsNoTracking()
                            ;
            return set;
        }

        public virtual async Task<TBaseEntityModel> GetByID(
            TPrimaryKey id
            )
        {
            var entity = await GetByIDIncludeProperties(id);
            return entity;
        }

        public virtual async Task<TBaseEntityModel> GetByID(
            TPrimaryKey id,
            params Expression<Func<TBaseEntityModel, object>>[] includeProperties
            )
        {
            var entity = await GetByIDIncludeProperties(id, includeProperties);
            return entity;
        }

        public virtual async Task<TBaseEntityModel> Create(TBaseEntityModel entity)
        {
            await DbContext.Set<TBaseEntityModel>().AddAsync(entity);
            await DbContext.SaveChangesAsync();

            return entity;
        }

        public virtual async Task Update(TPrimaryKey id, TBaseEntityModel entityToUpdate)
        {
            DbContext.Set<TBaseEntityModel>()
                     .Update(entityToUpdate); // todo this does not handle partial updates
            await DbContext.SaveChangesAsync();
        }

        public virtual async Task Delete(TPrimaryKey id)
        {
            var entity = await GetByIDIncludeProperties(id);
            if (entity == null)
                throw new ArgumentException("Could not find entity");

            // todo: changed to a soft delete.
            DbContext.Set<TBaseEntityModel>().Remove(entity);
            await DbContext.SaveChangesAsync();

        }
        #endregion


        #region Private & Protected methods
        protected internal Task<TBaseEntityModel> GetByIDIncludeProperties(
            TPrimaryKey id,
            params Expression<Func<TBaseEntityModel, object>>[] includeProperties
            )
        {
            return Task.Run(() =>
            {
                if (includeProperties.Any())
                {
                    var set = includeProperties
                      .Aggregate<Expression<Func<TBaseEntityModel, object>>, IQueryable<TBaseEntityModel>>
                        (DbContext.Set<TBaseEntityModel>(), (current, expression) => current.Include(expression));

                    return set.SingleOrDefault(s => s.ID.Equals(id));
                }

                return DbContext.Set<TBaseEntityModel>().Find(id);
            });
        }


        //protected internal async Task<TBaseEntityModel> SetEntityFieldsFromUpdateModel(TBaseEntityModel updatedEntity)
        //{
        //    var entity = await DbContext.Set<TBaseEntityModel>().SingleOrDefaultAsync(e => e.ID.Equals(updatedEntity.ID));
        //    if (entity == null)
        //        throw new ArgumentNullException($"{updatedEntity.GetType().Name} cannot be found in the database. DtoModel ID: {updatedEntity.ID}");

        //    return Mapper.Map(updatedEntity, entity);
        //}

        #endregion
    }
}
