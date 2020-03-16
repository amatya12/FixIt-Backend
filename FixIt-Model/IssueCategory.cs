using System;
using System.Collections.Generic;
using System.Text;

namespace FixIt_Model
{
    public class IssueCategory
    {
        public int Id { get; set; }

        public int IssueId { get; set; }
        public Issue Issue { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
