using System;

namespace GraniteCore
{
    public abstract class BaseDto<TPrimaryKey> : IBaseIdentityModel<TPrimaryKey>
    {
        public TPrimaryKey ID { get; set; }

        public BaseDto()
        {

        }
    }
}
