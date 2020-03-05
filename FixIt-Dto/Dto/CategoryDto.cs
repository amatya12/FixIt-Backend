using FixIt_Dto.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FixIt_Backend.Dto
{
    public class CategoryDto
    {
        public int Id { get; set; }

        public string CategoryName { get; set; }

        public List<SubCategoriesDto> SubCategories { get; set; }
    }
}
