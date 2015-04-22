using System;
using System.Collections.Generic;
using System.Linq;
using BookStore.DAL.Interface.Abstract;
using BookStore.DLL.Interface.Abstract;
using Ninject;

namespace BookStore.DLL.RepositoryService
{

    public abstract  class StoreService<T> : IStoreService<T> where T:class
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

        public virtual IList<T> GetAll()
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
