using GraniteCore;
using MyCars.Domain.DTOs;
using MyCars.Domain.Models;
using System;
using System.Collections.Generic;

namespace MyCars.Services
{
    // GraniteCore install
    public interface ICustomerService : IBaseService<CustomerDTO, CustomerEntity, Guid, string>
    {
    }
}
