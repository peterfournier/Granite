using System;

namespace GraniteCore
{
    public abstract class BaseDto<TPrimaryKey, TUserPrimaryKey> : IUserBasedDto<TPrimaryKey, TUserPrimaryKey>
    {
        public TPrimaryKey ID { get; set; }
        public DateTime CreatedDatetime { get; set; }
        public DateTime LastModifiedDatetime { get; set; }
        public virtual BaseApplicationUser<TUserPrimaryKey> CreatedByUser { get; set; }
        public virtual BaseApplicationUser<TUserPrimaryKey> LastModifiedByUser { get; set; }

        public BaseDto()
        {

        }
    }
}
