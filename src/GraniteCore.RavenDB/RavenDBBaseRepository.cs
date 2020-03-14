using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace GraniteCore.RavenDB
{
    public class RavenDBBaseRepository<TDtoModel, TEntity, TPrimaryKey, TUserPrimaryKey> : IBaseRepository<TDtoModel, TEntity, TPrimaryKey>
        where TDtoModel : class, IDto<TPrimaryKey>, new()
        where TEntity : class, IBaseIdentityModel<TPrimaryKey>, new()
    {
        protected IDocumentStore Store { get; }
        protected IGraniteMapper Mapper { get; }

        protected readonly IDocumentSession Session; // Playing with this idea

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
            Session = Store.OpenSession(new SessionOptions()
            {

            });

        }

        //~RavenDBBaseRepository()
        //{
        //    Session.Dispose();
        //}

        #region Public CRUD methods
        public virtual IQueryable<TDtoModel> GetAll()
        {
            var set = Session.Advanced.DocumentQuery<TEntity>()
                            .ToQueryable()
                            ;

            return Mapper.Map<TEntity, TDtoModel>(set);
        }

        public virtual async Task<TDtoModel> GetByID(
            TPrimaryKey id
            )
        {
            var entity = await getByID(id);
            return Mapper.Map<TEntity, TDtoModel>(entity);
        }

        public virtual async Task<TDtoModel> GetByID(
            TPrimaryKey id,
            params Expression<Func<TEntity, object>>[] includeProperties
            )
        {
            var entity = await getByID(id, includeProperties);
            return Mapper.Map<TEntity, TDtoModel>(entity);
        }

        public virtual Task<TDtoModel> Create(TDtoModel dtoModel)
        {
            return Task.Run(() =>
            {

                //if (userID == null)
                //    throw new ArgumentException("CreatedBy is not set");

                if (dtoModel == null)
                    throw new ArgumentException("DtoModel is not set");

                var entity = new TEntity();

                Mapper.Map(dtoModel, entity);

                //if (dtoModel is IUserBasedDto<TPrimaryKey, TUserPrimaryKey> userBasedtoUpdated)
                //{
                //    setCreatedFields(userBasedtoUpdated, userID);
                //    setLastUpdatedFields(userBasedtoUpdated, userID);
                //}

                //if (entity is IUserBasedModel<TPrimaryKey, TUserPrimaryKey> userBaseEntity)
                //{
                //    setCreatedFields(userBaseEntity, userID);
                //    setLastUpdatedFields(userBaseEntity, userID);
                //}

                Session.Store(entity, entity.ID.ToString());
                Session.SaveChanges();

                dtoModel.ID = entity.ID;

                return dtoModel;
            });
        }

        public async virtual Task Update(TPrimaryKey id, TDtoModel dtoUpdated)
        {
            //if (dtoUpdated is IUserBasedDto<TPrimaryKey, TUserPrimaryKey> userBasedtoUpdated)
            //    setLastUpdatedFields(userBasedtoUpdated, userID);

            var entity = await setEntityFieldsFromDto(dtoUpdated);

            ignoreFieldsWhenUpdating(entity);

            Session.Store(entity, entity.ID.ToString()); // todo this does not handle partial updates
            Session.SaveChanges();
        }

        public virtual async Task Delete(TPrimaryKey id)
        {
            var entity = await getByID(id);
            if (entity == null)
                throw new ArgumentException("Could not find entity");

            // todo: changed to a soft delete.
            //if (entity is IUserBasedModel<TPrimaryKey, TUserPrimaryKey> userBaseEntity)
            //    setLastUpdatedFields(userBaseEntity, userID);

            Session.Delete(entity);
            Session.SaveChanges();
        }

        #endregion


        #region Private methods
        private Task<TEntity> getByID(
            TPrimaryKey id,
            params Expression<Func<TEntity, object>>[] includeProperties
            )
        {
            return Task.Run(() =>
            {
                if (includeProperties.Any())
                {
                    var set = includeProperties.Aggregate<Expression<Func<TEntity, object>>, IQueryable<TEntity>>
                        (Session.Advanced.DocumentQuery<TEntity>().ToQueryable(),
                            (current, expression) => current.Include(expression));

                    return set.SingleOrDefault(s => s.ID.Equals(id));
                }

                return Session.Load<TEntity>(id?.ToString());
            });
        }

        private void ignoreFieldsWhenUpdating(TEntity entity)
        {
            //if (entity is IUserBasedModel<TPrimaryKey, TUserPrimaryKey> userBaseEntity)
            //{
                //_dbContext.Entry(userBaseEntity).Property(x => x.CreatedByUserID).IsModified = false;
                //_dbContext.Entry(userBaseEntity).Property(x => x.CreatedDatetime).IsModified = false;
            //}
        }

        //private void setCreatedFields(IUserBasedModel<TPrimaryKey, TUserPrimaryKey> model, TUserPrimaryKey userID)
        //{
        //    if (model == null)
        //        throw new ArgumentNullException("model is not set");

        //    if (userID == null)
        //        throw new ArgumentNullException("userID is not set");

        //    model.CreatedDatetime = DateTime.UtcNow;
        //    model.CreatedByUserID = userID;
        //}

        //private void setLastUpdatedFields(IUserBasedModel<TPrimaryKey, TUserPrimaryKey> model, TUserPrimaryKey userID)
        //{
        //    if (model == null)
        //        throw new ArgumentNullException("model is not set");

        //    if (userID == null)
        //        throw new ArgumentNullException("userID is not set");

        //    model.LastModifiedDatetime = DateTime.UtcNow;

        //    model.LastModifiedByUserID = userID;
        //}

        private Task<TEntity> setEntityFieldsFromDto(TDtoModel dtoUpdated)
        {
            return Task.Run(() =>
            {
                var entity = Session.Load<TEntity>(dtoUpdated.ID?.ToString());
                if (entity == null)
                    throw new ArgumentNullException($"{dtoUpdated.GetType().Name} cannot be found in the database. DtoModel ID: {dtoUpdated.ID}");

                return Mapper.Map(dtoUpdated, entity);
            });
        }
        #endregion
    }
}
