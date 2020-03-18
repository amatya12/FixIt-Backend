using System;
using System.Collections.Generic;
using System.Text;

namespace FixIt_Dto.Dto
{
    public class IssueForListDto
    {
        public int Id { get; set; }

        public string Issues { get; set; }

        public List<int> CategoryId { get; set; }

        public List<int> SubCategoryId { get; set; }

        public string Priority { get; set; }

        public string ImageUrl { get; set; }

        public string Location { get; set; }

        public CoordsDto Coords { get; set; }

        public string Status { get; set; }

        public string DateCreated { get; set; }
    }
}
