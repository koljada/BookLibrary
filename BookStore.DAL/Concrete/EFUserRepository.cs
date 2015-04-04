using BookStore.DAL.Abstract;
using BookStore.DO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BookStore.DAL.Concrete
{
    public class EFUserRepository:EFStoreRepository<User>,IUserRepository
    {
        public IQueryable<Book> GetReccomendedBooks(int userID)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Book> GetWishedBooks(int userID)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Book> GetRatedBooks(int userID)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Author> GetFavAuthors(int userID)
        {
            throw new NotImplementedException();
        }

        public ICollection<Role> GetRoles(int userID)
        {
            return context.Users.FirstOrDefault(u => u.User_ID == userID).Roles;
        }

        public IQueryable<Comment> GetComment(int userID)
        {
            throw new NotImplementedException();
        }

        public User GetUserByEmail(string email)
        {
            return context.Users.Include(e=>e.Roles).FirstOrDefault(e => e.Email == email);
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
    }
}
