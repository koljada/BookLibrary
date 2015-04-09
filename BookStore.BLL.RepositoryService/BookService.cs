using System;
using System.Linq;
using BookStore.DAL.Abstract;
using BookStore.DLL.Abstract;
using BookStore.DO.Entities;

namespace BookStore.BLL.RepositoryService
{

    public class BookService : StoreService<Book>, IBookService
    {
        private readonly IBookRepository _repository;
        public BookService(IBookRepository repo)
        {
            _repository = repo;
        }
        public IQueryable<Book> Books
        {
            get
            {
                return _repository.GetAll().OrderByDescending(b => b.Rating);
            }
        }

        public IQueryable<Book> GetBooksByLetter(string letter)
        {
            return _repository.GetBooksByLetter(letter).OrderByDescending(b => b.Rating);
        }

        public IQueryable<Book> GetBooksByAuthor(string lastName)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Book> GetBooksByGenre(string genre)
        {
            return _repository.GetBooksByGenre(genre).OrderByDescending(b => b.Rating);
        }

        public IQueryable<Book> GetBooksByTitle(string title)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Book> GetBooksByTag(int tagId)
        {
            return _repository.GetBooksByTag(tagId).OrderByDescending(b => b.Rating);
        }

        public IQueryable<Comment> GetComment(Comment comment)
        {
            throw new NotImplementedException();
        }

        public override Book GetById(int id)
        {
            return _repository.GetById(id);
        }

        public override IQueryable<Book> GetAll()
        {
            return _repository.GetAll().OrderByDescending(b => b.Rating);
        }
        public override void Save(Book obj){
            _repository.Save(obj);
        }
        public override void Create(Book obj)
        {
            _repository.Create(obj);
        }


        public void AddComment( Comment comment)
        {
            _repository.AddComment(comment);
        }
    }
}
