using System;
using System.Linq;
using BookStore.DAL.Abstract;

namespace BookStore.DAL.EntityFramework
{
    public class EfStoreRepository<T> : IStoreRepository<T> where T : class
    {
        protected readonly EfDbContext Context = new EfDbContext();
        public virtual T GetById(int id)
        {
            return Context.Set<T>().Find(id);
        }
        public virtual IQueryable<T> GetAll()
        {
            return Context.Set<T>();
        }

        public virtual void Save(T obj)
        {
            Context.Set<T>().Add(obj);
            Context.SaveChanges();
        }

        public virtual T Delete(int id)
        {
            //context.Set<T>().Remove()
            return Context.Set<T>().Find(id);
        }

        public virtual void Create(T obj)
        {
            throw new NotImplementedException();
        }
    }
}
