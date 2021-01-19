using System;
using System.Collections.Generic;
using System.Text;

namespace GraniteCore.LocalFileRepository.IntegrationTests.Models
{
    class Workout : BaseEntityModel<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
