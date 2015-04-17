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
        public AuthorService(IAuthorRepository repository):base(repository)
        {
            _repository = repository;
        }

        public Author GetByName(string lastName, string firstName)
        {
            return _repository.GetByName(lastName, firstName);
        }

        public void AddBook(Book book, Author toAuthor)
        {
            _repository.AddBook(book, toAuthor);
        }

        public IList<Book> GetBooks(string author)
        {
            return _repository.GetBooks(author);
        }
    }
}
