using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DAL.Abstract
{
    public interface IStoreRepository<T>
    {
        T GetByID(int ID);
        IQueryable<T> GetAll();
        void Save(T obj);
        void Delete(int id);
    }
}
