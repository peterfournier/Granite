using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace GraniteCore.RavenDB
{
    public class RavenDBBaseRepository<TBaseEntityModel, TPrimaryKey, TUserPrimaryKey> : IBaseRepository<TBaseEntityModel, TPrimaryKey>
        where TBaseEntityModel : class, IBaseIdentityModel<TPrimaryKey>, new()
    {
        protected IDocumentStore Store { get; }

        protected readonly IDocumentSession Session; // Playing with this idea

        #region Constructor(s) / Finalizers

        #endregion
        public RavenDBBaseRepository(
            IDocumentStore store)
        {
            Store = store;

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
        public virtual IQueryable<TBaseEntityModel> GetAll()
        {
            var set = Session.Advanced.DocumentQuery<TBaseEntityModel>()
                            .ToQueryable()
                            ;

            return set;
        }

        public virtual async Task<TBaseEntityModel> GetByID(
            TPrimaryKey id
            )
        {
            var entity = await getByID(id);
            return entity;
        }

        public virtual async Task<TBaseEntityModel> GetByID(
            TPrimaryKey id,
            params Expression<Func<TBaseEntityModel, object>>[] includeProperties
            )
        {
            var entity = await getByID(id, includeProperties);
            return entity;
        }

        public virtual Task<TBaseEntityModel> Create(TBaseEntityModel entityModel)
        {
            return Task.Run(() =>
            {

                //if (userID == null)
                //    throw new ArgumentException("CreatedBy is not set");

                if (entityModel == null)
                    throw new ArgumentException("entityModel is not set");

                

                //if (entityModel is IUserBasedDto<TPrimaryKey, TUserPrimaryKey> userBasedtoUpdated)
                //{
                //    setCreatedFields(userBasedtoUpdated, userID);
                //    setLastUpdatedFields(userBasedtoUpdated, userID);
                //}

                //if (entity is IUserBasedModel<TPrimaryKey, TUserPrimaryKey> userBaseEntity)
                //{
                //    setCreatedFields(userBaseEntity, userID);
                //    setLastUpdatedFields(userBaseEntity, userID);
                //}

                Session.Store(entityModel, entityModel.ID.ToString());
                Session.SaveChanges();

                entityModel.ID = entityModel.ID;

                return entityModel;
            });
        }

        public async virtual Task Update(TPrimaryKey id, TBaseEntityModel dtoUpdated)
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
        private Task<TBaseEntityModel> getByID(
            TPrimaryKey id,
            params Expression<Func<TBaseEntityModel, object>>[] includeProperties
            )
        {
            return Task.Run(() =>
            {
                if (includeProperties.Any())
                {
                    var set = includeProperties.Aggregate<Expression<Func<TBaseEntityModel, object>>, IQueryable<TBaseEntityModel>>
                        (Session.Advanced.DocumentQuery<TBaseEntityModel>().ToQueryable(),
                            (current, expression) => current.Include(expression));

                    return set.SingleOrDefault(s => s.ID.Equals(id));
                }

                return Session.Load<TBaseEntityModel>(id?.ToString());
            });
        }

        private void ignoreFieldsWhenUpdating(TBaseEntityModel entity)
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

        private Task<TBaseEntityModel> setEntityFieldsFromDto(TBaseEntityModel entityToUpdate)
        {
            return Task.Run(() =>
            {
                var entity = Session.Load<TBaseEntityModel>(entityToUpdate.ID?.ToString());
                if (entity == null)
                    throw new ArgumentNullException($"{entityToUpdate.GetType().Name} cannot be found in the database. entityModel ID: {entityToUpdate.ID}");

                return entity;
            });
        }
        #endregion
    }
}
