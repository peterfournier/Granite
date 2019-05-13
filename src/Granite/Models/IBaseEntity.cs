namespace GraniteCore
{
    public interface IBaseEntity<TPrimaryKey, TUserPrimaryKey> : IUserBasedModel<TPrimaryKey, TUserPrimaryKey>
    {
        TUserPrimaryKey CreatedByUserID { get; set; }
        TUserPrimaryKey LastModifiedByUserID { get; set; }
    }
}
