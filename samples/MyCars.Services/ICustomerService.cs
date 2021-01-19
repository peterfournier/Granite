using GraniteCore;
using GraniteCore.Services;
using MyCars.Domain;
using MyCars.Domain.Entities;
using System;

namespace MyCars.Services
{
    // GraniteCore install
    public interface ICustomerService : IBaseService<Customer, CustomerEntity, Guid>, IUserModifierService<string>
    {
    }
}
