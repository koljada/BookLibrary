using System;
using System.Linq;
using BookStore.DAL.Abstract;
using BookStore.DLL.Abstract;

namespace BookStore.BLL.RepositoryService
{

    public  class StoreService<T> : IStoreService<T>
    {
        private readonly IStoreRepository<T> _repository;
        public StoreService()
        {

        }
        public StoreService(IStoreRepository<T> repo)
        {
            _repository = repo;
        }

        public virtual T GetById(int id)
        {
            return _repository.GetById(id);
        }

        public virtual IQueryable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        
        public virtual void Save(T obj)
        {
            _repository.Save(obj);
        }

        public virtual T Delete(int id)
        {
            return _repository.Delete(id);
        }
        public virtual void Create(T obj)
        {
            _repository.Create(obj);
        }
    }
}
