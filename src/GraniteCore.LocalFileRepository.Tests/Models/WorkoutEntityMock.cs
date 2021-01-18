using System;
using System.Collections.Generic;
using System.Text;

namespace GraniteCore.LocalFileRepository.IntegrationTests.Models
{
    class WorkoutEntityMock : GraniteCore.BaseModel<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public WorkoutEntityMock()
        {
            generateRandomID();
        }

        private void generateRandomID()
        {
            if (ID == 0)
            {
                ID = Guid.NewGuid().GetHashCode();
            }
        }
    }
}
