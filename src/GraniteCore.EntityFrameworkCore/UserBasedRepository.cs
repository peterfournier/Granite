using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Diagnostics;

namespace GraniteCore.EntityFrameworkCore
{
    public class UserBasedRepository<TDtoModel, TEntity, TPrimaryKey, TUser, TUserPrimaryKey> : BaseRepository<TDtoModel, TEntity, TPrimaryKey>, IUserBasedRepository<TDtoModel, TEntity, TPrimaryKey, TUser, TUserPrimaryKey>
        where TDtoModel : IDto<TPrimaryKey>, new()
        where TEntity : class, IBaseIdentityModel<TPrimaryKey>, new()
        where TUser : IBaseApplicationUser<TUserPrimaryKey>
    {
        public UserBasedRepository(
            DbContext dbContext,
            IGraniteMapper mapper
            ) : base (dbContext, mapper)
        {
        }

        #region Public CRUD methods
        
        public new virtual async Task<TDtoModel> Create(TDtoModel dtoModel, TUser user)
        {
            if (user == null)
                throw new ArgumentException("CreatedBy User is not set");

            if (dtoModel == null)
                throw new ArgumentException("DtoModel is not set");

            var entity = new TEntity();

            Mapper.Map(dtoModel, entity);

            if (dtoModel is IUserBasedDto<TPrimaryKey, TUser, TUserPrimaryKey> userBasedtoUpdated)
            {
                Debugger.Log(1,"", "Writing DTO fields");
                setCreatedFields(userBasedtoUpdated, user.ID);
                setLastUpdatedFields(userBasedtoUpdated, user.ID);
            }

            if (entity is IUserBasedModel<TPrimaryKey, TUser, TUserPrimaryKey> userBaseEntity)
            {
                Debugger.Log(1, "", "Writing ENTITY fields");
                setCreatedFields(userBaseEntity, user.ID);
                setLastUpdatedFields(userBaseEntity, user.ID);
            }

            await DbContext.Set<TEntity>().AddAsync(entity);
            await DbContext.SaveChangesAsync();

            dtoModel.ID = entity.ID;

            return dtoModel;
        }

        public new virtual async Task Update(TPrimaryKey id, TDtoModel dtoUpdated, TUser user)
        {
            if (user == null)
                throw new ArgumentException("User is not set");

            if (dtoUpdated is IUserBasedDto<TPrimaryKey, TUser, TUserPrimaryKey> userBasedtoUpdated)
                setLastUpdatedFields(userBasedtoUpdated, user.ID);

            var entity = await SetEntityFieldsFromDto(dtoUpdated);

            ignoreFieldsWhenUpdating(entity);

            DbContext.Set<TEntity>().Update(entity); // todo this does not handle partial updates
            await DbContext.SaveChangesAsync();
        }

        public new virtual async Task Delete(TPrimaryKey id, TUser user)
        {
            var entity = await GetByIDIncludeProperties(id);
            if (entity == null)
                throw new ArgumentException("Could not find entity");

            // todo: changed to a soft delete.
            if (entity is IUserBasedModel<TPrimaryKey, TUser, TUserPrimaryKey> userBaseEntity)
                setLastUpdatedFields(userBaseEntity, user.ID);

            DbContext.Set<TEntity>().Remove(entity);
            await DbContext.SaveChangesAsync();

        }
        #endregion


        #region Private methods
        private void ignoreFieldsWhenUpdating(TEntity entity)
        {
            if (entity is IUserBasedModel<TPrimaryKey, TUser, TUserPrimaryKey> userBaseEntity)
            {
                DbContext.Entry(userBaseEntity).Property(x => x.CreatedByUserID).IsModified = false;
                DbContext.Entry(userBaseEntity).Property(x => x.CreatedDatetime).IsModified = false;
            }
        }

        private void setCreatedFields(IUserBasedModel<TPrimaryKey, TUser, TUserPrimaryKey> model, TUserPrimaryKey userID)
        {
            if (model == null)
                throw new ArgumentNullException("model is not set");

            if (userID == null)
                throw new ArgumentNullException("userID is not set");

            model.CreatedDatetime = DateTime.UtcNow;            
            model.CreatedByUserID = userID;
        }

        private void setLastUpdatedFields(IUserBasedModel<TPrimaryKey, TUser, TUserPrimaryKey> model, TUserPrimaryKey userID)
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
