using FixIt_Data.Context;
using FixIt_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FixIt_Service.HelperFunctions
{
    public class IssueHelperFunction
    {
        private readonly DataContext context;

        public IssueHelperFunction(DataContext context)
        {
            this.context = context;
        }

        //This methods takes List of subCategory Id and provide List of Category based on the subcategoryId's.
        public List<Category> GetCategoryFromSubCategory(List<int> subCategoryId)
        {
            List<Category> categories = new List<Category>();
            foreach(var id in subCategoryId)
            {
                var subCategoryEntity = context.SubCategories.FirstOrDefault(x => x.Id == id);
                var category = context.Categories.FirstOrDefault(x => x.Id == subCategoryEntity.CategoryId);
                categories.Add(category);
            }
            return categories;
        }

    }
}
