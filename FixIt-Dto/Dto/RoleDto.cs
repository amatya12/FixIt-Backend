using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FixIt_Backend.Dto
{
    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string Description { get; set; }
    }
}
