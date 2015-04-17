using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DLL.Abstract
{
    public interface IStoreService<T> where T : class
    {
         T GetById(int id);
         IQueryable<T> GetAll();
         void Create(T obj);
         void Save(T obj);
         T Delete(int id);
    }
}
