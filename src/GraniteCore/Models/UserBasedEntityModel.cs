using System;

namespace GraniteCore
{
    public abstract class UserBasedEntityModel<TPrimaryKey, TUser, TUserPrimaryKey> : BaseEntityModel<TPrimaryKey>, IUserBasedEntityModel<TPrimaryKey, TUser, TUserPrimaryKey>
        where TUser : class, IUser<TUserPrimaryKey>
    {
        public TUserPrimaryKey CreatedByUserID { get; set; }
        public TUserPrimaryKey LastModifiedByUserID { get; set; }

        public DateTime CreatedDatetime { get; set; }
        public DateTime LastModifiedDatetime { get; set; }

        public virtual TUser CreatedByUser { get; set; }
        public virtual TUser LastModifiedByUser { get; set; }

        public UserBasedEntityModel()
        {

        }
    }
}
