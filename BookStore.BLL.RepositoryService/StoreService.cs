using System;
using System.Linq;
using BookStore.DAL.Abstract;
using BookStore.DLL.Abstract;
using Ninject;

namespace BookStore.BLL.RepositoryService
{

    public abstract  class StoreService<T> : IStoreService<T>
    {
        protected readonly IStoreRepository<T> _repositoryStore;
        protected StoreService()
        {

        }
        protected StoreService(IStoreRepository<T> repo)
        {
            _repositoryStore = repo;
        }

        public virtual T GetById(int id)
        {
            return _repositoryStore.GetById(id);
        }

        public virtual IQueryable<T> GetAll()
        {
            return _repositoryStore.GetAll(); 
        }

        
        public virtual void Save(T obj)
        {
            _repositoryStore.Save(obj);
        }

        public virtual T Delete(int id)
        {
            return _repositoryStore.Delete(id);
        }
        public virtual void Create(T obj)
        {
            _repositoryStore.Create(obj);
        }
    }
}
