using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FixIt_Model
{
    public class Feedback
    {
        [Key]
        public int Key { get; set; }

        [Column(TypeName = "varchar(2000)")]
        public string Message { get; set; }
    }
}
