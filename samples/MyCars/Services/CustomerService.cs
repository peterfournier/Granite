using System;
using System.Collections.Generic;
using System.Linq;
using GraniteCore;
using MyCars.Domain.DTOs;
using MyCars.Domain.Models;

namespace MyCars.Services
{
    // GraniteCore install
    public class CustomerService : BaseService<CustomerDTO, CustomerEntity, Guid, string>, ICustomerService
    {
        public CustomerService(
            IBaseRepository<CustomerDTO, CustomerEntity, Guid, string> repository, 
            IGraniteMapper mapper) 
            : base(repository, mapper)
        {
        }
    }
}
