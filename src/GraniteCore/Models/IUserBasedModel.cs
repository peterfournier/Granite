using System;

namespace GraniteCore
{
    public interface IUserBasedModel<TPrimaryKey, TUserPrimaryKey> : IBaseIdentityModel<TPrimaryKey>
    {
        TUserPrimaryKey CreatedByUserID { get; set; }
        TUserPrimaryKey LastModifiedByUserID { get; set; }

        BaseApplicationUser<TUserPrimaryKey> CreatedByUser { get; set; }
        BaseApplicationUser<TUserPrimaryKey> LastModifiedByUser { get; set; }
        DateTime CreatedDatetime { get; set; }
        DateTime LastModifiedDatetime { get; set; }
    }
}
