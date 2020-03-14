using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace GraniteCore.EntityFrameworkCore
{
    public class BaseRepository<TDtoModel, TEntity, TPrimaryKey> : IBaseRepository<TDtoModel, TEntity, TPrimaryKey>
        where TDtoModel : IDto<TPrimaryKey>, new()
        where TEntity : class, IBaseIdentityModel<TPrimaryKey>, new()
    {
        protected internal readonly DbContext DbContext;
        protected internal readonly IGraniteMapper Mapper;

        public BaseRepository(
            DbContext dbContext,
            IGraniteMapper mapper
            )
        {
            DbContext = dbContext;
            Mapper = mapper;
        }

        #region Public CRUD methods
        public virtual IQueryable<TDtoModel> GetAll()
        {
            var set = DbContext.Set<TEntity>()                            
                            .AsNoTracking()
                            ;
            return Mapper.Map<TEntity, TDtoModel>(set);
        }

        public virtual async Task<TDtoModel> GetByID(
            TPrimaryKey id
            )
        {
            var entity = await GetByIDIncludeProperties(id);
            return Mapper.Map<TEntity, TDtoModel>(entity);
        }

        public virtual async Task<TDtoModel> GetByID(
            TPrimaryKey id,
            params Expression<Func<TEntity, object>>[] includeProperties
            )
        {
            var entity = await GetByIDIncludeProperties(id, includeProperties);
            return Mapper.Map<TEntity, TDtoModel>(entity);
        }

        public virtual async Task<TDtoModel> Create(TDtoModel dtoModel)
        {
            if (dtoModel == null)
                throw new ArgumentException("DtoModel is not set");

            var entity = new TEntity();

            Mapper.Map(dtoModel, entity);

            await DbContext.Set<TEntity>().AddAsync(entity);
            await DbContext.SaveChangesAsync();

            dtoModel.ID = entity.ID;

            return dtoModel;
        }

        public virtual async Task Update(TPrimaryKey id, TDtoModel dtoUpdated)
        {
            var entity = await SetEntityFieldsFromDto(dtoUpdated);

            DbContext.Set<TEntity>().Update(entity); // todo this does not handle partial updates
            await DbContext.SaveChangesAsync();
        }

        public virtual async Task Delete(TPrimaryKey id)
        {
            var entity = await GetByIDIncludeProperties(id);
            if (entity == null)
                throw new ArgumentException("Could not find entity");

            // todo: changed to a soft delete.
            DbContext.Set<TEntity>().Remove(entity);
            await DbContext.SaveChangesAsync();

        }
        #endregion


        #region Private & Protected methods
        protected internal Task<TEntity> GetByIDIncludeProperties(
            TPrimaryKey id,
            params Expression<Func<TEntity, object>>[] includeProperties
            )
        {
            return Task.Run(() =>
            {
                if (includeProperties.Any())
                {
                    var set = includeProperties
                      .Aggregate<Expression<Func<TEntity, object>>, IQueryable<TEntity>>
                        (DbContext.Set<TEntity>(), (current, expression) => current.Include(expression));

                    return set.SingleOrDefault(s => s.ID.Equals(id));
                }

                return DbContext.Set<TEntity>().Find(id);
            });
        }


        protected internal async Task<TEntity> SetEntityFieldsFromDto(TDtoModel dtoUpdated)
        {
            var entity = await DbContext.Set<TEntity>().SingleOrDefaultAsync(e => e.ID.Equals(dtoUpdated.ID));
            if (entity == null)
                throw new ArgumentNullException($"{dtoUpdated.GetType().Name} cannot be found in the database. DtoModel ID: {dtoUpdated.ID}");

            return Mapper.Map(dtoUpdated, entity);
        }

        #endregion
    }
}
