using System;
using System.Collections.Generic;
using System.Text;

namespace FixIt_Dto.Dto
{
    public class IssueForEditDto
    {
        public int Id { get; set; }

        public string Issues { get; set; }

        public int CategoryId { get; set; }

        public string Priority { get; set; }

        public string ImageUrl { get; set; }

        public string Location { get; set; }

        public double latitude { get; set; }

        public double longitude { get; set; }

        public string Status { get; set; }

        public string DateCreated { get; set; }
    }
}
