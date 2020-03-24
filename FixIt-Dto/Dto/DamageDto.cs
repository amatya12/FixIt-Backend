using System;
using System.Collections.Generic;
using System.Text;

namespace FixIt_Dto.Dto
{
    public class DamageDto
    {
        public int Id { get; set; }

        public string Issues { get; set; }

        public string Location { get; set; }

        public CoordsDto Coordinates { get; set; }

    }
}
