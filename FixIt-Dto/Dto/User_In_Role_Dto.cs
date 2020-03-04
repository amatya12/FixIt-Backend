using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FixIt_Backend.Dto
{
    public class User_In_Role_Dto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public List<RoleDto> Role { get; set; }
    }
}
