using System;

namespace Granite.Models
{
    public interface IUserBasedModel<TPrimaryKey, TUserPrimaryKey> : IBaseIdentityModel<TPrimaryKey>
    {
        BaseApplicationUser<TUserPrimaryKey> CreatedByUser { get; set; }
        BaseApplicationUser<TUserPrimaryKey> LastModifiedByUser { get; set; }
        DateTime CreatedDatetime { get; set; }
        DateTime LastModifiedDatetime { get; set; }
    }
}
