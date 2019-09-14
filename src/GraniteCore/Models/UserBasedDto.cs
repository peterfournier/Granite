using System;

namespace GraniteCore
{
    public abstract class UserBasedDto<TPrimaryKey, TUserPrimaryKey, TUser> : BaseDto<TPrimaryKey>, IUserBasedDto<TPrimaryKey, TUserPrimaryKey, TUser>
        where TUser : IBaseApplicationUser<TUserPrimaryKey>
    {
        public TUserPrimaryKey CreatedByUserID { get; set; }
        public TUserPrimaryKey LastModifiedByUserID { get; set; }
        public DateTime CreatedDatetime { get; set; }
        public DateTime LastModifiedDatetime { get; set; }
        public virtual TUser CreatedByUser { get; set; }
        public virtual TUser LastModifiedByUser { get; set; }

        public UserBasedDto()
        {

        }
    }
}
