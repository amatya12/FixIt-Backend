using FixIt_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FixIt_Interface
{
    public interface ICustomFilterService<T>
    {
        IQueryable<Category> GetAllByFilterQ(string Q);

        IQueryable<Category> GetAllByFilterId(IQueryable<T> source, List<int> id);

        IQueryable<Category> GetAllByFilterReferenceId(IQueryable<T> source, int referenceId);
        
    }
}
