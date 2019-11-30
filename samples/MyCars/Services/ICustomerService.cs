using GraniteCore;
using GraniteCore.Services;
using MyCars.Domain.DTOs;
using MyCars.Domain.Models;
using System;

namespace MyCars.Services
{
    // GraniteCore install
    public interface ICustomerService : IBaseService<CustomerDTO, CustomerEntity, Guid>, IUserModifierService<string>
    {
    }
}
