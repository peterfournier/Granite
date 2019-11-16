using System;
using GraniteCore;
using MyCars.Areas.Identity;
using MyCars.Domain.DTOs;
using MyCars.Domain.Models;

namespace MyCars.Services
{
    // GraniteCore install
    public class CustomerService : UserBasedService<CustomerDTO, CustomerEntity, Guid, GraniteCoreApplicationUser, string>, ICustomerService
    {
        public CustomerService(
            IUserBasedRepository<CustomerDTO, CustomerEntity, Guid, GraniteCoreApplicationUser, string> repository, 
            IGraniteMapper mapper) 
            : base(repository, mapper)
        {
        }
    }
}
