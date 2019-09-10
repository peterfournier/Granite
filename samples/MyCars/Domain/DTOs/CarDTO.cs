using GraniteCore;
using System;
using System.Drawing;

namespace MyCars.Domain.DTOs
{
    public class CarDTO : BaseDto<Guid>
    {
        public int Year { get; set; }
        public string ColorHex { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
    }
}
