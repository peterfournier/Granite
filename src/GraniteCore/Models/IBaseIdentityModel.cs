namespace GraniteCore
{
    public interface IBaseIdentityModel<TPrimaryKey>
    {
        TPrimaryKey ID { get; set; }
    }
}
