using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCars.Domain.ViewModels
{
    public class CarViewModel
    {
        public int Year { get; set; }
        public string ColorHex { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
    }
}
