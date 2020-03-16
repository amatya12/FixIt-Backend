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


        public IEnumerable<Issue> GetAll() => context.Issues.Include(c => c.Category).Where(x => x.IsDeleted == false);

        public Issue GetById(int id) => context.Issues.Include(x => x.Category).FirstOrDefault(i => i.Id == id);

        public void Save(Issue model)
        {
            if(model.Id == 0)
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
           return context.Issues.Include(c => c.Category).Where(x => x.Issues.Contains(Q));
        }

        public IQueryable<Issue> GetAllByFilterReferenceId(IQueryable<Issue> source, int referenceId)
        {
            return source.Where(x => x.CategoryId == referenceId);
        }

    }
}
