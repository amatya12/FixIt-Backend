using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FixIt_Model
{
    public class SubCategories
    {
        [Key]
        public int Id { get; set; }

        public string SubCategoryName { get; set; }
        
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public ICollection<IssueSubCategory> IssueSubCategories { get; set; }
    }
}
