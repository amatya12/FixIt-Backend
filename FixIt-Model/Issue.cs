﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FixIt_Model
{
    public class Issue
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName ="varchar(1000)")]
        public string Issues{ get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public string Priority { get; set; }

        public string ImageUrl { get; set; }

        public string Location { get; set; }

        public double latitude { get; set; }

        public double longitude { get; set; }

        public bool Status { get; set; }

        

    }
}
