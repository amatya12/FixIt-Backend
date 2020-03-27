using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FixIt_Model
{
    public class Issue
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName ="varchar(1000)")]
        public string Issues{ get; set; }

        public string Priority { get; set; }

        public string ImageUrl { get; set; }

        public string Location { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Status { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public bool IsDeleted { get; set; }

        public IList<IssueCategory> IssueCategories { get; set; }

        public IList<IssueSubCategory> IssueSubCategories { get; set; }

        

    }
}
