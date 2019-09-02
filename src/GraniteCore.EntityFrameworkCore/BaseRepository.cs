using GraniteCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System;

namespace GraniteCore.EntityFrameworkCore
{
    public class BaseRepository<TDtoModel, TEntity, TPrimaryKey, TUserPrimaryKey> : IBaseRepository<TDtoModel, TEntity, TPrimaryKey, TUserPrimaryKey>
        where TDtoModel : class, IDto<TPrimaryKey>, new()
        where TEntity : class, IBaseIdentityModel<TPrimaryKey>, new()
    {
        //private List<Expression<Func<TEntity, object>>> defaultIncludes = new List<Expression<Func<TEntity, object>>>
        //{
        //    (TEntity entity) => entity.CreatedByUser,
        //    (TEntity entity) => entity.LastModifiedByUser
        //};

        protected readonly DbContext _dbContext;
        protected readonly IGraniteMapper _mapper;

        public BaseRepository(
            DbContext dbContext,
            IGraniteMapper mapper
            )
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        #region Public CRUD methods
        public virtual IQueryable<TDtoModel> GetAll()
        {
            var set = _dbContext.Set<TEntity>()                            
                            .AsNoTracking()
                            ;

            if (set is IQueryable<IUserBasedModel<TPrimaryKey, TUserPrimaryKey>> userBaseSet)
            {
                userBaseSet = userBaseSet.Include(x => x.CreatedByUser)
                                         .Include(x => x.LastModifiedByUser);

                return _mapper.Map<IUserBasedModel<TPrimaryKey, TUserPrimaryKey>, TDtoModel>(userBaseSet);
            }

            return _mapper.Map<TEntity, TDtoModel>(set);
        }

        public virtual async Task<TDtoModel> GetById(
            TPrimaryKey id
            )
        {
            var entity = await getByID(id);
            return _mapper.Map<TEntity, TDtoModel>(entity);
        }

        public virtual async Task<TDtoModel> GetById(
            TPrimaryKey id,
            params Expression<Func<TEntity, object>>[] includeProperties
            )
        {
            var entity = await getByID(id, includeProperties);
            return _mapper.Map<TEntity, TDtoModel>(entity);
        }

        public virtual async Task<TDtoModel> Create(TDtoModel dtoModel, TUserPrimaryKey userID)
        {
            if (userID == null)
                throw new ArgumentException("CreatedBy is not set");

            if (dtoModel == null)
                throw new ArgumentException("DtoModel is not set");

            var entity = new TEntity();

            _mapper.Map(dtoModel, entity);

            if (dtoModel is IUserBasedDto<TPrimaryKey, TUserPrimaryKey> userBasedtoUpdated)
            {
                setCreatedFields(userBasedtoUpdated, userID);
                setLastUpdatedFields(userBasedtoUpdated, userID);
            }

            if (entity is IUserBasedModel<TPrimaryKey, TUserPrimaryKey> userBaseEntity)
            {
                setCreatedFields(userBaseEntity, userID);
                setLastUpdatedFields(userBaseEntity, userID);
            }

            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            dtoModel.ID = entity.ID;

            return dtoModel;
        }

        public virtual async Task Update(TPrimaryKey id, TDtoModel dtoUpdated, TUserPrimaryKey userID)
        {
            var entity = await setEntityFieldsFromDto(dtoUpdated);

            if (dtoUpdated is IUserBasedDto<TPrimaryKey, TUserPrimaryKey> userBasedtoUpdated)
                setLastUpdatedFields(userBasedtoUpdated, userID);

            ignoreFieldsWhenUpdating(entity);

            _dbContext.Set<TEntity>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task Delete(TPrimaryKey id, TUserPrimaryKey userID)
        {
            var entity = await getByID(id);
            if (entity == null)
                throw new ArgumentException("Could not find entity");

            // todo: changed to a soft delete.
            if (entity is IUserBasedModel<TPrimaryKey, TUserPrimaryKey> userBaseEntity)
                setLastUpdatedFields(userBaseEntity, userID);

            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();

        }
        #endregion


        #region Private methods
        private Task<TEntity> getByID(
            TPrimaryKey id,
            params Expression<Func<TEntity, object>>[] includeProperties
            )
        {
            return Task.Run<TEntity>(() =>
            {
                if (includeProperties.Any())
                {
                    var set = includeProperties
                      .Aggregate<Expression<Func<TEntity, object>>, IQueryable<TEntity>>
                        (_dbContext.Set<TEntity>(), (current, expression) => current.Include(expression));

                    return set.SingleOrDefault(s => s.ID.Equals(id));
                }

                return _dbContext.Set<TEntity>().Find(id);
            });
        }

        private void ignoreFieldsWhenUpdating(TEntity entity)
        {
            if (entity is IUserBasedModel<TPrimaryKey, TUserPrimaryKey> userBaseEntity)
            {
                _dbContext.Entry(userBaseEntity).Property(x => x.CreatedByUserID).IsModified = false;
                _dbContext.Entry(userBaseEntity).Property(x => x.CreatedDatetime).IsModified = false;
            }
        }

        private void setCreatedFields(IUserBasedModel<TPrimaryKey, TUserPrimaryKey> model, TUserPrimaryKey userID)
        {
            if (model == null)
                throw new ArgumentNullException("model is not set");

            if (userID == null)
                throw new ArgumentNullException("userID is not set");

            model.CreatedDatetime = DateTime.UtcNow;            
            model.CreatedByUserID = userID;
        }

        private void setLastUpdatedFields(IUserBasedModel<TPrimaryKey, TUserPrimaryKey> model, TUserPrimaryKey userID)
        {
            if (model == null)
                throw new ArgumentNullException("model is not set");

            if (userID == null)
                throw new ArgumentNullException("userID is not set");

            model.LastModifiedDatetime = DateTime.UtcNow;

            model.LastModifiedByUserID = userID;
        }

        private async Task<TEntity> setEntityFieldsFromDto(TDtoModel dtoUpdated)
        {
            var entity = await _dbContext.Set<TEntity>().SingleOrDefaultAsync(e => e.ID.Equals(dtoUpdated.ID));
            if (entity == null)
                throw new ArgumentNullException($"{dtoUpdated.GetType().Name} cannot be found in the database. DtoModel ID: {dtoUpdated.ID}");

            return _mapper.Map(dtoUpdated, entity);
        }

        private bool IsSimple(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // nullable type, check if the nested type is simple.
                return IsSimple(typeInfo.GetGenericArguments()[0]);
            }
            return typeInfo.IsPrimitive
              || typeInfo.IsEnum
              || type.Equals(typeof(string))
              || type.Equals(typeof(decimal));
        }
        #endregion
    }
}
