using System;
using System.Collections.Generic;
using System.Text;

namespace FixIt_Interface
{
    public interface ICrudService<T>
    {
        void Create(T model);
        bool Exists(T model);
        T GetById(int id);
        IEnumerable<T> GetAll();
        void Save(T model);
        void Update(T model);
        void DeleteById(int id);
    }
}
