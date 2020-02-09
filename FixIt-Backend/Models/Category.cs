using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FixIt_Backend.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName ="varchar(200)")]
        public string CategoryName { get; set; }

    }
}
