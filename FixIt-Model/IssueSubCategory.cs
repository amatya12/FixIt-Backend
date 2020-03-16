using System;
using System.Collections.Generic;
using System.Text;

namespace FixIt_Model
{
    public class IssueSubCategory
    {
        public int Id { get; set; }

        public int IssueId { get; set; }
        public Issue Issue { get; set; }

        public int SubCategoryId { get; set; }
        public SubCategories SubCategory { get; set; }
    }
}
