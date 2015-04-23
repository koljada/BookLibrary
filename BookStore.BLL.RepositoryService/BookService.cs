using System;
using System.Collections.Generic;
using System.Linq;
using BookStore.DAL.Interface.Abstract;
using BookStore.DLL.Interface.Abstract;
using BookStore.DO.Entities;

namespace BookStore.DLL.RepositoryService
{

    public class BookService : StoreService<Book>, IBookService
    {
        private readonly IBookRepository _repository;
        public BookService(IBookRepository repository)
            : base(repository)
        {
            _repository = repository;
        }

        public IList<Book> GetBooksByLetter(string letter)
        {
            return _repository.GetBooksByLetter(letter);
        }

        public IList<Book> GetBooksByAuthor(string lastName)
        {
            throw new NotImplementedException();
        }

        public IList<Book> GetBooksByGenre(string genre)
        {
            return _repository.GetBooksByGenre(genre);
        }

        public IList<Book> GetBooksByTitle(string title)
        {
            throw new NotImplementedException();
        }

        public IList<Book> GetBooksByTag(int tagId)
        {
            return _repository.GetBooksByTag(tagId);
        }

        public IList<Comment> GetComment(Comment comment)
        {
            throw new NotImplementedException();
        }

        public void AddComment(Comment comment)
        {
            _repository.AddComment(comment);
        }


        public Rate GetRate(int bookId, int userId)
        {
            return _repository.GetRate(bookId, userId);
        }



        public IList<Book> GetAllWithDetails()
        {
            return _repository.GetAllWithDetail();
        }
    }
}
