using System;

namespace Granite
{
    public abstract class BaseModel<TPrimaryKey, TUserPrimaryKey> : IBaseEntity<TPrimaryKey, TUserPrimaryKey>
    {
        private TPrimaryKey _id;

        public virtual TPrimaryKey ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual TUserPrimaryKey CreatedByUserID { get; set; }

        public virtual DateTime CreatedDatetime { get; set; }

        public virtual TUserPrimaryKey LastModifiedByUserID { get; set; }

        public virtual DateTime LastModifiedDatetime { get; set; }


        #region Nav props
        public virtual BaseApplicationUser<TUserPrimaryKey> CreatedByUser { get; set; }

        public virtual BaseApplicationUser<TUserPrimaryKey> LastModifiedByUser { get; set; }
        #endregion
    }
}
