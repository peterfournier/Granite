using System;

namespace GraniteCore
{
    public abstract class UserBasedModel<TPrimaryKey, TUserPrimaryKey, TUser> : BaseModel<TPrimaryKey>, IUserBasedModel<TPrimaryKey, TUserPrimaryKey, TUser>
        where TUser : IBaseApplicationUser<TUserPrimaryKey>

    {
        public virtual TUserPrimaryKey CreatedByUserID { get; set; }

        public virtual DateTime CreatedDatetime { get; set; }

        public virtual TUserPrimaryKey LastModifiedByUserID { get; set; }

        public virtual DateTime LastModifiedDatetime { get; set; }


        #region Nav props
        public virtual TUser CreatedByUser { get; set; }

        public virtual TUser LastModifiedByUser { get; set; }
        #endregion
    }
}
