using System;
using System.Collections.Generic;
using System.Text;

namespace FixIt_Dto.Dto
{
    public class LoginResponseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Jwt { get; set; }
        public string Email { get; set; }
        public List<string> Role { get; set; }
    }
}
