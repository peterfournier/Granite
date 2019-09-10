using GraniteCore;
using MyCars.Domain.DTOs;
using MyCars.Domain.Models;
using System;
using System.Collections.Generic;

namespace MyCars.Services
{
    // GraniteCore install
    public interface ICarService : IBaseService<CarDTO, CarEntity, Guid, Guid>
    {
        IList<CarDTO> GetTopCars(int take = 5);
    }
}
