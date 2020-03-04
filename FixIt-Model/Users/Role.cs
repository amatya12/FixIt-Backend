using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FixIt_Model.Users
{
    public class Role : IdentityRole<int>
    {
        [Column(TypeName = "varchar(300")]
        public string Description { get; set; }
    }
}
