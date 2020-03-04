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
    public class IssueService : ICrudService<Issue>
    {
        private readonly DataContext context;
        public IssueService(DataContext context)
        {
            this.context = context;
        }
        public void Create(Issue model) => context.Issues.Add(model);


        public void DeleteById(int id) => context.Issues.Remove(GetById(id));

        public bool Exists(Issue model) => GetById(model.Id) != null;


        public IEnumerable<Issue> GetAll() => context.Issues.Include(c => c.Category);

        public Issue GetById(int id) => context.Issues.FirstOrDefault(i => i.Id == id);

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
        
    }
}
