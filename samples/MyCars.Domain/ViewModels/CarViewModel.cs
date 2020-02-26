using System;

namespace MyCars.Domain.ViewModels
{
    public class CarViewModel
    {
        public int Year { get; set; }
        public string ColorHex { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public Guid ID { get; set; }
    }
}
