﻿using System;

namespace GraniteCore
{
    public abstract class UserBasedDto<TPrimaryKey, TUserPrimaryKey> : BaseDto<TPrimaryKey>, IUserBasedDto<TPrimaryKey, TUserPrimaryKey>
    {
        public TUserPrimaryKey CreatedByUserID { get; set; }
        public TUserPrimaryKey LastModifiedByUserID { get; set; }
        public DateTime CreatedDatetime { get; set; }
        public DateTime LastModifiedDatetime { get; set; }
        public virtual BaseApplicationUser<TUserPrimaryKey> CreatedByUser { get; set; }
        public virtual BaseApplicationUser<TUserPrimaryKey> LastModifiedByUser { get; set; }

        public UserBasedDto()
        {

        }
    }
}