using System;
using System.Collections.Generic;
using System.Linq;
using BookStore.DAL.Abstract;
using BookStore.DLL.Abstract;
using BookStore.DO.Entities;
using Ninject;

namespace BookStore.BLL.RepositoryService
{
    public class UserService:StoreService<User>,IUserService
    {
        [Inject] readonly IUserRepository _repository;
        public UserService(IUserRepository repo)
        {
            _repository = repo;
        }

        public IQueryable<Book> GetReccomendedBooks(int userId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Book> GetWishedBooks(int userId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Book> GetRatedBooks(int userId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Author> GetFavAuthors(int userId)
        {
            throw new NotImplementedException();
        }

        public ICollection<Role> GetRoles(int userId)
        {
            return _repository.GetRoles(userId);
        }

        public IQueryable<Comment> GetComment(int userId)
        {
            throw new NotImplementedException();
        }

        public void RateBook(Book book)
        {
            throw new NotImplementedException();
        }

        public void WishBook(Book book)
        {
            throw new NotImplementedException();
        }

        public void AddComment(Book book)
        {
            throw new NotImplementedException();
        }

        public void LikeAuthor(Author author)
        {
            throw new NotImplementedException();
        }


        public User GetUserByEmail(string email)
        {
            return _repository.GetUserByEmail(email);
        }

        public override void Create(User obj)
        {
            _repository.Create(obj) ; 
        }
    }
}
