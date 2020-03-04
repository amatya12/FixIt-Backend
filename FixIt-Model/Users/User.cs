using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FixIt_Model.Users
{
    public class User : IdentityUser<int>
    {

        [Column(TypeName = "nvarchar(100")]
        public string Name { get; set; }
        
        public virtual ICollection<UserRole> UserRoles { get; set; }

    }
}
