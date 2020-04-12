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

        public IQueryable<Issue> GetAllByCustomFilterStatus(IQueryable<Issue> source, string customFilter)
        {
            return source = source.Where(x => x.Status == customFilter);
        }

        public IQueryable<Issue> GetAllByCustomFilterPriority(IQueryable<Issue> source, string customFilter)
        {
            return source = source.Where(x => x.Priority == customFilter);
        }

        public IQueryable<Issue> GetAllByStartAndEndDate(IQueryable<Issue> issues, string fromDate, string toDate)
        {
            DateTime from_Date = DateTime.Parse(fromDate);
            DateTime to_Date = DateTime.Parse(toDate);
            int result = DateTime.Compare(from_Date, to_Date);
            if (result <= 0)
            {
                return issues.Where(x => x.DateCreated >= from_Date && x.DateCreated <= to_Date);
            }
            else
            {
                return issues.Where(x => x.DateCreated >=from_Date);
            }

        }

        public IQueryable<Issue> GetAllByStartAndEndDate(IQueryable<Issue> issues, string fromDate)
        {
            return issues.Where(x => x.DateCreated >= DateTime.Parse(fromDate));
        }

        public IQueryable<Issue> GetAllByEndDate(IQueryable<Issue> issues, string toDate)
        {
            return issues.Where(x => x.DateCreated <= DateTime.Parse(toDate));
        }
    }
}
