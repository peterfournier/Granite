using System;

namespace GraniteCore
{
    public abstract class BaseModel<TPrimaryKey> : IBaseIdentityModel<TPrimaryKey>
    {
        private TPrimaryKey _id;

        public virtual TPrimaryKey ID
        {
            get { return _id; }
            set { _id = value; }
        }
    }
}
