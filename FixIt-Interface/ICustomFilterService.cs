using FixIt_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FixIt_Interface
{
    public interface ICustomFilterService<T>
    {
        IQueryable<T> GetAllByFilterQ(string Q);

        IQueryable<T> GetAllByFilterId(IQueryable<T> source, List<int> id);

        IQueryable<T> GetAllByFilterReferenceId(IQueryable<T> source, int referenceId);
        
    }
}
