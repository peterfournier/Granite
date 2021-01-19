namespace GraniteCore
{
    public abstract class BaseEntityModel<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        public TPrimaryKey ID { get; set; }

        public BaseEntityModel()
        {

        }
    }
}
