using GraniteCore;
using MyCars.Domain;
using MyCars.Domain.Entities;
using System;
using System.Collections.Generic;

namespace MyCars.Services
{
    // GraniteCore install
    public interface ICarService : IBaseService<Car, CarEntity, Guid>
    {
        IList<Car> GetTopCars(int take = 5);
    }
}
