namespace Granite
{

    public abstract class BaseApplicationUser<TPrimaryKey> : IBaseApplicationUser<TPrimaryKey>
    {
        public TPrimaryKey ID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public string FullName => FirstName + " " + LastName;
    }
}
