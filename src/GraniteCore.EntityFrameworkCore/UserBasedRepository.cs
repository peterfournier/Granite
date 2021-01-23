using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Diagnostics;

namespace GraniteCore.EntityFrameworkCore
{
    public class UserBasedRepository<TBaseEntityModel, TPrimaryKey, TUserPrimaryKey> 
        : BaseRepository<TBaseEntityModel, TPrimaryKey>, IUserBasedRepository<TBaseEntityModel, TPrimaryKey, TUserPrimaryKey>
            where TBaseEntityModel : class, IBaseIdentityModel<TPrimaryKey>, new()
    {
        public UserBasedRepository(
            DbContext dbContext,
            IGraniteMapper mapper
            ) : base(dbContext, mapper)
        {
        }

        #region Public CRUD methods
        
        public virtual async Task<TBaseEntityModel> Create(TBaseEntityModel entityModel, IUser<TUserPrimaryKey> user)
        {
            if (user == null)
                throw new ArgumentException("CreatedBy User is not set");

            if (entityModel == null)
                throw new ArgumentException("DtoModel is not set");

            if (entityModel is UserBasedEntityModel<TPrimaryKey, IUser<TUserPrimaryKey>, TUserPrimaryKey> userBasedtoUpdated)
            {
                setCreatedFields(userBasedtoUpdated, user.ID);
                setLastUpdatedFields(userBasedtoUpdated, user.ID);
            }

            await DbContext.Set<TBaseEntityModel>().AddAsync(entityModel);
            await DbContext.SaveChangesAsync();

            entityModel.ID = entityModel.ID;

            return entityModel;
        }

        public virtual async Task Update(TPrimaryKey id, TBaseEntityModel entityModel, IUser<TUserPrimaryKey> user)
        {
            if (user == null)
                throw new ArgumentException("User is not set");

            if (entityModel is UserBasedEntityModel<TPrimaryKey, IUser<TUserPrimaryKey>, TUserPrimaryKey> userBasedtoUpdated)
                setLastUpdatedFields(userBasedtoUpdated, user.ID);

            DbContext.Set<TBaseEntityModel>().Update(entityModel); // todo this does not handle partial updates
            ignoreFieldsWhenUpdating(entityModel);
            await DbContext.SaveChangesAsync();
        }

        public virtual async Task Delete(TPrimaryKey id, IUser<TUserPrimaryKey> user)
        {
            var entity = await GetByIDIncludeProperties(id);
            if (entity == null)
                throw new ArgumentException("Could not find entity");

            // todo: changed to a soft delete.
            if (entity is UserBasedEntityModel<TPrimaryKey, IUser<TUserPrimaryKey>, TUserPrimaryKey> userBaseEntity)
                setLastUpdatedFields(userBaseEntity, user.ID);

            DbContext.Set<TBaseEntityModel>().Remove(entity);
            await DbContext.SaveChangesAsync();

        }
        #endregion


        #region Private methods
        private void ignoreFieldsWhenUpdating(TBaseEntityModel entity)
        {
            if (entity is UserBasedEntityModel<TPrimaryKey, IUser<TUserPrimaryKey>, TUserPrimaryKey> userBaseEntity)
            {
                DbContext.Entry(userBaseEntity).Property(x => x.CreatedByUserID).IsModified = false;
                DbContext.Entry(userBaseEntity).Property(x => x.CreatedDatetime).IsModified = false;
            }
        }

        private void setCreatedFields(UserBasedEntityModel<TPrimaryKey, IUser<TUserPrimaryKey>, TUserPrimaryKey> model, TUserPrimaryKey userID)
        {
            if (model == null)
                throw new ArgumentNullException("model is not set");

            if (userID == null)
                throw new ArgumentNullException("userID is not set");

            model.CreatedDatetime = DateTime.UtcNow;
            model.CreatedByUserID = userID;
        }

        private void setLastUpdatedFields(UserBasedEntityModel<TPrimaryKey, IUser<TUserPrimaryKey>, TUserPrimaryKey> model, TUserPrimaryKey userID)
        {
            if (model == null)
                throw new ArgumentNullException("model is not set");

            if (userID == null)
                throw new ArgumentNullException("userID is not set");

            model.LastModifiedDatetime = DateTime.UtcNow;

            model.LastModifiedByUserID = userID;
        }
        #endregion
    }
}
