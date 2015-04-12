using System.Collections.Generic;
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
            return _repository.GetAll();
        }


        public void AddBook(Book book, Author toAuthor)
        {
            _repository.AddBook(book, toAuthor);
        }

        public ICollection<Book> GetBooks(string author)
        {
            return _repository.GetBooks(author);
        }

        public override Author GetById(int id)
        {
            return _repository.GetById(id);
        }
    }
}
