﻿using System;
using System.ComponentModel.DataAnnotations;
using GraniteCore;

namespace MyCars.Domain.Models
{
    public class CarEntity : BaseModel<Guid>
    {
        public int Year { get; set; }
        public string ColorHex { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }

        public CarEntity()
        {
            ID = Guid.NewGuid();
        }
    }
}
