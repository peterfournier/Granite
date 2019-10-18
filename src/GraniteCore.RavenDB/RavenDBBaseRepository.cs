using GraniteCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Threading;

namespace GraniteCore.RavenDB
{
    public class RavenDBBaseRepository<TDtoModel, TEntity, TPrimaryKey, TUserPrimaryKey> : IBaseRepository<TDtoModel, TEntity, TPrimaryKey, TUserPrimaryKey>
        where TDtoModel : class, IDto<TPrimaryKey>, new()
        where TEntity : class, IBaseIdentityModel<TPrimaryKey>, new()
    {
        private readonly object _conventions;
        private readonly string _database;
        private readonly string _x509CertificateFilePath;
        private readonly string[] _urls;
        protected IDocumentStore Store { get; }
        protected IGraniteMapper Mapper { get; }

        protected readonly IAsyncDocumentSession AsyncSession; // Playing with this idea

        #region Constructor(s) / Finalizers

        #endregion
        public RavenDBBaseRepository(
            IDocumentStore store,
            IGraniteMapper mapper
            )
        {
            Store = store;
            Mapper = mapper;

            // Opening sessions per instance
            // should be scoped anyway
            AsyncSession = Store.OpenAsyncSession(new SessionOptions()
            {
                NoTracking = true
            });

        }

        ~RavenDBBaseRepository()
        {
            AsyncSession.Dispose();
        }

        #region Public CRUD methods
        public virtual IQueryable<TDtoModel> GetAll()
        {
            var set = AsyncSession.Advanced.AsyncDocumentQuery<TEntity>()
                            .ToQueryable()
                            ;

            return Mapper.Map<TEntity, TDtoModel>(set);
        }

        public virtual async Task<TDtoModel> GetById(
            TPrimaryKey id
            )
        {
            var entity = await getByID(id);
            return Mapper.Map<TEntity, TDtoModel>(entity);
        }

        public virtual async Task<TDtoModel> GetById(
            TPrimaryKey id,
            params Expression<Func<TEntity, object>>[] includeProperties
            )
        {
            var entity = await getByID(id, includeProperties);
            return Mapper.Map<TEntity, TDtoModel>(entity);
        }

        public async virtual Task<TDtoModel> Create(TDtoModel dtoModel, TUserPrimaryKey userID)
        {
            if (userID == null)
                throw new ArgumentException("CreatedBy is not set");

            if (dtoModel == null)
                throw new ArgumentException("DtoModel is not set");

            var entity = new TEntity();

            Mapper.Map(dtoModel, entity);

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

            await AsyncSession.StoreAsync(entity);
            await AsyncSession.SaveChangesAsync();

            dtoModel.ID = entity.ID;

            return dtoModel;
        }

        public virtual async Task Update(TPrimaryKey id, TDtoModel dtoUpdated, TUserPrimaryKey userID)
        {
            if (dtoUpdated is IUserBasedDto<TPrimaryKey, TUserPrimaryKey> userBasedtoUpdated)
                setLastUpdatedFields(userBasedtoUpdated, userID);

            var entity = await setEntityFieldsFromDto(dtoUpdated);

            ignoreFieldsWhenUpdating(entity);

            await AsyncSession.StoreAsync(entity); // todo this does not handle partial updates
            await AsyncSession.SaveChangesAsync();
        }

        public virtual async Task Delete(TPrimaryKey id, TUserPrimaryKey userID)
        {
            var entity = await getByID(id);
            if (entity == null)
                throw new ArgumentException("Could not find entity");

            // todo: changed to a soft delete.
            if (entity is IUserBasedModel<TPrimaryKey, TUserPrimaryKey> userBaseEntity)
                setLastUpdatedFields(userBaseEntity, userID);

            AsyncSession.Delete(entity);
            await AsyncSession.SaveChangesAsync();

        }

        #endregion


        #region Private methods
        private async Task<TEntity> getByID(
            TPrimaryKey id,
            params Expression<Func<TEntity, object>>[] includeProperties
            )
        {
            if (includeProperties.Any())
            {
                var set = includeProperties.Aggregate<Expression<Func<TEntity, object>>, IQueryable<TEntity>>
                    (AsyncSession.Advanced.AsyncDocumentQuery<TEntity>().ToQueryable(), 
                        (current, expression) => current.Include(expression));

                return set.SingleOrDefault(s => s.ID.Equals(id));
            }

            return await AsyncSession.LoadAsync<TEntity>(id?.ToString(), CancellationToken.None);
        }

        private void ignoreFieldsWhenUpdating(TEntity entity)
        {
            if (entity is IUserBasedModel<TPrimaryKey, TUserPrimaryKey> userBaseEntity)
            {
                //_dbContext.Entry(userBaseEntity).Property(x => x.CreatedByUserID).IsModified = false;
                //_dbContext.Entry(userBaseEntity).Property(x => x.CreatedDatetime).IsModified = false;
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
            var entity = await AsyncSession.LoadAsync<TEntity>(dtoUpdated.ID?.ToString(), CancellationToken.None);
            if (entity == null)
                throw new ArgumentNullException($"{dtoUpdated.GetType().Name} cannot be found in the database. DtoModel ID: {dtoUpdated.ID}");

            return Mapper.Map(dtoUpdated, entity);
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
