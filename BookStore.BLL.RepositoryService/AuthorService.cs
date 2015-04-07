using System.Linq;
using BookStore.DAL.Abstract;
using BookStore.DLL.Abstract;
using BookStore.DO.Entities;

namespace BookStore.BLL.RepositoryService
{
    public class AuthorService:StoreService<Author>,IAuthorService
    {
        private readonly IAuthorRepository _repository;
        public AuthorService(IAuthorRepository repo)
        {
            _repository = repo;
        }

        public Author GetByName(string lastName, string firstName)
        {
            return _repository.GetByName(lastName, firstName);
        }
        public override IQueryable<Author> GetAll()
        {
            return _repository.GetAll().OrderByDescending(t=>t.Rating);
        }


        public void AddBook(Book book, Author toAuthor)
        {
            _repository.AddBook(book, toAuthor);
        }
    }
}
