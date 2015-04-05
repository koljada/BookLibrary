using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.DO.Entities;
using BookStore.DAL.Abstract;
using System.Data.Entity;

namespace BookStore.DAL.Concrete
{
    public class EFStoreRepository<T> : IStoreRepository<T> where T : class
    {
        protected readonly EFDbContext context = new EFDbContext();
        public virtual T GetByID(int ID)
        {
            return context.Set<T>().Find(ID);
        }
        public virtual IQueryable<T> GetAll()
        {
            return context.Set<T>();
        }

        public virtual void Save(T obj)
        {
            context.Set<T>().Add(obj);
            context.SaveChanges();
        }

        public virtual T Delete(int ID)
        {
            //context.Set<T>().Remove()
            return context.Set<T>().Find(ID);
        }


        public virtual void Create(T obj)
        {
            throw new NotImplementedException();
        }
    }
}
