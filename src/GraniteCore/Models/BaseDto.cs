namespace GraniteCore
{
    public abstract class BaseDto<TPrimaryKey> : IDto<TPrimaryKey>
    {
        public TPrimaryKey ID { get; set; }

        public BaseDto()
        {

        }
    }
}
