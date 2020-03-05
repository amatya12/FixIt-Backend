using FixIt_Data.Context;
using FixIt_Interface;
using FixIt_Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FixIt_Service.CrudServices
{
    public class CategoryService : ICrudService<Category>, ICustomFilterService<Category>
    {
        private readonly DataContext context;
        public CategoryService(DataContext context)
        {
            this.context = context;
        }
        public void Create(Category model) => context.Categories.Add(model);


        public void DeleteById(int id) => context.Categories.Remove(GetById(id));
        

        public bool Exists(Category model) => GetById(model.Id) != null;


        public IEnumerable<Category> GetAll() => context.Categories.Include(x => x.SubCategories);


        public Category GetById(int id) => context.Categories.Include(x => x.SubCategories).FirstOrDefault(x => x.Id == id);
        

        public void Save(Category model)
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

        public void Update(Category model) => context.Categories.Update(model);

        public IQueryable<Category> GetAllByFilterQ(string Q)
        {
           return context.Categories.Include(x => x.SubCategories).Where(x => x.CategoryName.Contains(Q));
        }
        public IQueryable<Category> GetAllByFilterId(IQueryable<Category> category,List<int> id)
        {
            return category.Where(x => id.Contains(x.Id));
        }

        public IQueryable<Category> GetAllByFilterReferenceId(IQueryable<Category> category, int referenceId)
        {
            return category.Where(x => x.Id == referenceId);
        }
    }
}
