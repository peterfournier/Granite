using System;
using System.Collections.Generic;
using System.Linq;
using GraniteCore;
using MyCars.Domain.DTOs;
using MyCars.Domain.Models;

namespace MyCars.Services
{
    // GraniteCore install
    public class CarService : BaseService<CarDTO, CarEntity, Guid, Guid>, ICarService
    {
        public CarService(
            IBaseRepository<CarDTO, CarEntity, Guid, Guid> repository, 
            IGraniteMapper mapper) 
            : base(repository, mapper)
        {
        }

        public IList<CarDTO> GetTopCars(int take = 5)
        {
            // maintain encapsulation of the generic Repository
            return Repository.GetAll()
                             .Take(take)
                             .ToList();
        }
    }
}
