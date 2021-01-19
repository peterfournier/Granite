using System;
using GraniteCore;
using MyCars.Domain;
using MyCars.Domain.Entities;

namespace MyCars.Services
{
    // GraniteCore install
    public class CustomerService : UserBasedService<Customer, CustomerEntity, Guid, string>, ICustomerService
    {
        public CustomerService(
            IUserBasedRepository<CustomerEntity, Guid, string> repository, 
            IGraniteMapper mapper) 
            : base(repository, mapper)
        {
        }
    }
}
