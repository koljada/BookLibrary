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

        public UserService(IUserRepository repository)
            : base(repository)
        {
            _repository = repository;
        }

        public IQueryable<Book> GetReccomendedBooks(int userId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Book> GetWishedBooks(int userId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Rate> GetRatedBooks(int userId)
        {
            return _repository.GetRatedBooks(userId);
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

        public void WishBook(Book book)
        {
            throw new NotImplementedException();
        }

        public void AddComment(Book book)
        {
            throw new NotImplementedException();
        }

        public void LikeAuthor(int authorId, int userId)
        {
               _repository.LikeAuthor(userId, authorId);
        }


        public User GetUserByEmail(string email)
        {
            return _repository.GetUserByEmail(email);
        }
        
        public void RateBook(float rate, int userId, int bookId,bool isSuggestion)
        {

            //Rate Rate=new Rate{RateValue = rate, User_ID = userId};
            _repository.RateBook(rate, userId, bookId,isSuggestion);
        }
    }
}
