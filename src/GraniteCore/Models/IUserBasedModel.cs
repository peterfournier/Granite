﻿using System;

namespace GraniteCore
{
    public interface IUserBasedModel<TPrimaryKey, TUser, TUserPrimaryKey> : IBaseIdentityModel<TPrimaryKey>
        where TUser : IBaseApplicationUser<TUserPrimaryKey>
    {
        TUserPrimaryKey CreatedByUserID { get; set; }
        TUserPrimaryKey LastModifiedByUserID { get; set; }

        TUser CreatedByUser { get; set; }
        TUser LastModifiedByUser { get; set; }

        DateTime CreatedDatetime { get; set; }
        DateTime LastModifiedDatetime { get; set; }
    }
}
