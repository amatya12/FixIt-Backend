﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FixIt_Dto.Dto
{
    public class SubCategoriesDto
    {
        public int Id { get; set; }
        public string SubCategoryName { get; set; }

        public int CategoryId { get; set; }
    }
}
