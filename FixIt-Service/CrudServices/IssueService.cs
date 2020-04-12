using FixIt_Data.Context;
using FixIt_Interface;
using FixIt_Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FixIt_Service.CrudServices
{
    public class IssueService : ICrudService<Issue>, ICustomFilterService<Issue>
    {
        private readonly DataContext context;
        public IssueService(DataContext context)
        {
            this.context = context;
        }
        public void Create(Issue model) => context.Issues.Add(model);


        public void DeleteById(int id)
        {
            var issue = GetById(id);
            issue.IsDeleted = true;
            context.SaveChanges();
        }

        public bool Exists(Issue model) => GetById(model.Id) != null;


        public IEnumerable<Issue> GetAll() => context.Issues.Include(c => c.IssueCategories)
                                .Include(x => x.IssueSubCategories)
                                .Where(x => x.IsDeleted == false)
                                .OrderByDescending(x => x.DateCreated);

        public Issue GetById(int id) => context.Issues.Include(x => x.IssueCategories).FirstOrDefault(i => i.Id == id);

        public void Save(Issue model)
        {
            if (model.Id == 0)
            {
                Create(model);
            }
            else
            {
                Update(model);
            }
        }

        public void Update(Issue model) => context.Issues.Update(model);

        public IQueryable<Issue> GetAllByFilterId(IQueryable<Issue> source, List<int> id)
        {
            return source.Where(x => id.Contains(x.Id));
        }

        public IQueryable<Issue> GetAllByFilterQ(string Q)
        {
            return context.Issues.Include(c => c.IssueCategories)
                                 .Include(x => x.IssueSubCategories)
                                 .Where(x => x.Issues.Contains(Q))
                                 .OrderByDescending(x => x.DateCreated);
        }

        public IQueryable<Issue> GetAllByFilterReferenceId(IQueryable<Issue> source, int referenceId)
        {
            //var result =  source.Select(x => x.IssueCategories.Where(y => y.CategoryId == referenceId).ToList());
            IList<Issue> result = new List<Issue>();
            foreach (var s in source)
            {
                var count = s.IssueCategories.Where(x => x.CategoryId == referenceId).Count() > 0;
                if (count)
                {
                    result.Add(s);
                }
            }

            return result.AsQueryable();
        }

        

    }
}

