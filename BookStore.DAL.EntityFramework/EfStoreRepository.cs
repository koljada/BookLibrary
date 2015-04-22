using System;
using System.Collections.Generic;
using System.Linq;
using BookStore.DAL.Interface.Abstract;

namespace BookStore.DAL.EntityFramework
{
    public abstract class EfStoreRepository<T> : IStoreRepository<T> where T : class
    {
        //protected readonly EfDbContext Context = new EfDbContext();
        public virtual T GetById(int id)
        {
            using (EfDbContext context=new EfDbContext())
            {
                return context.Set<T>().Find(id);
            }
        }
        public virtual IList<T> GetAll()
        {
            using (EfDbContext context = new EfDbContext())
            {
                return context.Set<T>().ToList();
            }
        }

        public virtual void Save(T obj)
        {
            using (EfDbContext context = new EfDbContext())
            {
                context.Set<T>().Add(obj);
                context.SaveChanges();
            }
        }

        public virtual T Delete(int id)
        {
            using (EfDbContext context = new EfDbContext())
            {
                var t = context.Set<T>().Find(id);
                context.Set<T>().Remove(t);
                context.SaveChanges();
                return t;
            }
        }

        public virtual void Create(T obj)
        {
            throw new NotImplementedException();
        }
    }
}
