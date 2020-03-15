using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FixIt_Backend.Dto
{
    public class IssueDto
    {
        public string Issues { get; set; }

        public int CategoryId { get; set; }

        public string Priority { get; set; }

        public string ImageUrl { get; set; }
        
        public string Location { get; set; }

        public double latitude { get; set; }

        public double longitude { get; set; }

        public string status { get; set; }

        public string DateCreated { get; set; }
       
    }
}
