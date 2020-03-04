using FixIt_Data.Context;
using FixIt_Interface;
using FixIt_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FixIt_Service.CrudServices
{
    public class SubCategoryService : ICrudService<SubCategories>, ICustomFilterService<SubCategories>
    {
        private readonly DataContext context;
        public SubCategoryService(DataContext context)
        {
            this.context = context;
        }
        public void Create(SubCategories model)
        {
            context.SubCategories.Add(model);
        }

        public void DeleteById(int id) => context.SubCategories.Remove(GetById(id));


        public bool Exists(SubCategories model) => GetById(model.Id) != null ? true : false;
        

        public IEnumerable<SubCategories> GetAll()
        {
            return context.SubCategories;   
        }

        public SubCategories GetById(int id)
        {
            return context.SubCategories.FirstOrDefault(x => x.Id == id);
        }

        public void Save(SubCategories model)
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

        public void Update(SubCategories model)
        {
            context.SubCategories.Update(model);
        }

        public IQueryable<SubCategories> GetAllByFilterId(IQueryable<SubCategories> source, List<int> id)
        {
            return source.Where(x => id.Contains(x.Id));
        }

        public IQueryable<SubCategories> GetAllByFilterQ(string Q)
        {
            return context.SubCategories.Where(x => x.SubCategoryName.Contains(Q));
        }

        public IQueryable<SubCategories> GetAllByFilterReferenceId(IQueryable<SubCategories> source, int referenceId)
        {
            return source.Where(x => x.Id == referenceId);
        }
    }
}
