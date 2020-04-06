using System;
using System.Collections.Generic;
using System.Text;

namespace FixIt_Dto.Dto
{
    public class CategoryWithSubCategoryDto
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public List<Children> Children { get; set; }
        
    }

    public class Children
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
}
