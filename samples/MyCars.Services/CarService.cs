using System;
using System.Collections.Generic;
using System.Linq;
using GraniteCore;
using MyCars.Domain;
using MyCars.Domain.Entities;

namespace MyCars.Services
{
    // GraniteCore install
    public class CarService : BaseService<Car, CarEntity, Guid>, ICarService  // todo remove this TUserPrimaryKey 
    {
        public CarService(
            IBaseRepository<CarEntity, Guid> repository, 
            IGraniteMapper mapper) 
            : base(repository, mapper)
        {
        }

        public IList<Car> GetTopCars(int take = 5)
        {

            return Mapper.Map<CarEntity,Car>(Repository.GetAll())
                             .Take(take)
                             .ToList();
        }
    }
}
