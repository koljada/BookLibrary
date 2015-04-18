using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public IList<Book> GetReccomendedBooks(int userId)
        {
            throw new NotImplementedException();
        }

        public IList<Book> GetWishedBooks(int userId)
        {
            throw new NotImplementedException();
        }

        public IList<Rate> GetRatedBooks(int userId)
        {
            return _repository.GetRatedBooks(userId);
        }

        public IList<Author> GetFavAuthors(int userId)
        {
            throw new NotImplementedException();
        }

        public IList<Role> GetRoles(int userId)
        {
            return _repository.GetRoles(userId);
        }

        public IList<Comment> GetComment(int userId)
        {
            throw new NotImplementedException();
        }

        public void WishBook(int  bookId,int userId)
        {
            _repository.WishBook(bookId,userId);
        }

        public void AddComment(Book book)
        {
            throw new NotImplementedException();
        }

        public void LikeAuthor(int authorId, int userId)
        {
               _repository.LikeAuthor(authorId,userId);
        }


        public User GetUserByEmail(string email)
        {
            return _repository.GetUserByEmail(email);
        }
        
        public async Task RateBook(float rate, int userId, int bookId,bool isSuggestion)
        {
            //Rate Rate=new Rate{RateValue = rate, User_ID = userId};
             await _repository.RateBook(rate, userId, bookId,isSuggestion);
        }
    }
}
