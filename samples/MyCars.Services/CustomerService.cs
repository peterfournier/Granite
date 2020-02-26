using System;
using GraniteCore;
using MyCars.Domain.DTOs;
using MyCars.Domain.Models;

namespace MyCars.Services
{
    // GraniteCore install
    public class CustomerService : UserBasedService<CustomerDTO, CustomerEntity, Guid, string>, ICustomerService
    {
        public CustomerService(
            IUserBasedRepository<CustomerDTO, CustomerEntity, Guid, string> repository, 
            IGraniteMapper mapper) 
            : base(repository, mapper)
        {
        }
    }
}
