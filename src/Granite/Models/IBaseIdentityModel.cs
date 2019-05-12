namespace Granite.Models
{
    public interface IBaseIdentityModel<TPrimaryKey>
    {
        TPrimaryKey ID { get; set; }
    }
}
