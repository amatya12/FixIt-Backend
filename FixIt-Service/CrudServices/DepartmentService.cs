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
    public class DepartmentService : ICrudService<Department> , ICustomFilterService<Department>
    {
        private readonly DataContext context;
        public DepartmentService(DataContext context) 
        {
            this.context = context;
        }

        public void Create(Department model)
        {
            context.Departments.Add(model);
        }

        public void DeleteById(int id) => context.Departments.Remove(GetById(id));
        

        public bool Exists(Department model) => GetById(model.Id) != null;

        public IEnumerable<Department> GetAll() => context.Departments;

       

        public Department GetById(int id) => context.Departments.FirstOrDefault(x => x.Id == id);
       

        public void Save(Department model)
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

        public void Update(Department model) => context.Departments.Update(model);

        public IQueryable<Department> GetAllByFilterId(IQueryable<Department> source, List<int> id)
        {
            return source.Where(x => id.Contains(x.Id));
        }

        public IQueryable<Department> GetAllByFilterQ(string Q)
        {
            return context.Departments.Where(x => x.Name.Contains(Q));
        }

        public IQueryable<Department> GetAllByFilterReferenceId(IQueryable<Department> source, int referenceId)
        {
            throw new NotImplementedException();
        }
    }
}
