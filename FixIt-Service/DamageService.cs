using FixIt_Data.Context;
using FixIt_Dto.Dto;
using FixIt_Interface;
using FixIt_Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FixIt_Service
{
    public class DamageService : IDamageService
    {
        private readonly DataContext context;
        public DamageService(DataContext context)
        {
            this.context = context;
        }
        public List<Issue> GetDamageList()
        {
            var issues = context.Issues.Include(x => x.IssueCategories).Where(x => x.IsDeleted == false)
                   .OrderByDescending(y => y.DateCreated).Take(100).ToList();

            //var categories = context.Categories;
            //List<DamageDto> damages = new List<DamageDto>();

            //foreach (var issue in issues)
            //{
            //    damages.Add(new DamageDto
            //    {
            //        Id = issue.Id,
            //        Coordinates = new CoordsDto
            //        {
            //            Latitude = issue.Latitude,
            //            Longitude = issue.Longitude
            //        },
            //        Issues = issue.Issues,
            //        Location = issue.Location,
            //        //CategoryName = GetCategoryNameFromId(issue.IssueCategories.First().CategoryId)
            //    });
            //}
            return issues;
        }

        public List<Issue> GetDamageByCategoryName(string categoryName)
        {
            int categoryId = context.Categories.First(x => x.CategoryName == categoryName).Id;
            List<int> issueIds = GetIssueIdsFromCategoryId(categoryId);

            

            var issues = context.Issues.Where(x => issueIds.Contains(x.Id) && x.IsDeleted == false)
                                .OrderByDescending(y => y.DateCreated).Take(100).ToList();


            return issues;
        }

        public string GetCategoryNameFromId(int id)
        {
            return context.Categories.FirstOrDefault(x => x.Id == id).CategoryName;
        }

        public List<int> GetIssueIdsFromCategoryId(int id)
        {
            return context.IssueCategories
                          .Where(x => x.CategoryId == id).Select(x => x.IssueId).ToList();
        }
    }

    
}
