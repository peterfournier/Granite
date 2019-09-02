using System;

namespace GraniteCore
{
    public abstract class UserBasedModel<TPrimaryKey, TUserPrimaryKey> : BaseModel<TPrimaryKey>, IUserBasedModel<TPrimaryKey, TUserPrimaryKey>
    {
        public virtual TUserPrimaryKey CreatedByUserID { get; set; }

        public virtual DateTime CreatedDatetime { get; set; }

        public virtual TUserPrimaryKey LastModifiedByUserID { get; set; }

        public virtual DateTime LastModifiedDatetime { get; set; }


        #region Nav props
        public virtual BaseApplicationUser<TUserPrimaryKey> CreatedByUser { get; set; }

        public virtual BaseApplicationUser<TUserPrimaryKey> LastModifiedByUser { get; set; }
        #endregion
    }
}
