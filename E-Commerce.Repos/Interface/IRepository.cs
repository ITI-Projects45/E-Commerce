using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Repos.Interface
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll(int page = 1);
        T GetById(int id);
        void Create(T entity);
        void Update(T entity);
        void Delete(int id);
        //void Save();
    }
}
