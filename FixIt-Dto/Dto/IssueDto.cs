using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FixIt_Backend.Dto
{
    public class IssueDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public double latitude { get; set; }

        public double longitude { get; set; }

        public string Severity { get; set; }

        public string ExtraInformation { get; set; }

        public bool status { get; set; }

        public int  CategoryId { get; set; }
    }
}
