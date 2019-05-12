using Granite.Models;
using System;

namespace Granite
{
    public interface IBaseApplicationUser<TPrimaryKey> : IBaseIdentityModel<TPrimaryKey>
    {        
        string FirstName { get; set; }
        string LastName { get; set; }
        string UserName { get; set; }
        string Email { get; set; }
    }
}
