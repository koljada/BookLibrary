using System;
using System.Collections.Generic;
using System.Linq;
using BookStore.DAL.Abstract;
using BookStore.DO.Entities;
using System.Data.Entity; 


namespace BookStore.DAL.EntityFramework
{
    public class EfUserRepository : EfStoreRepository<User>, IUserRepository
    {
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
            return Context.Users.FirstOrDefault(u => u.User_ID == userId).Roles;
        }

        public IQueryable<Comment> GetComment(int userId)
        {
            throw new NotImplementedException();
        }

        public User GetUserByEmail(string email)
        {
            return Context.Users.Include(e=>e.Roles).FirstOrDefault(e => e.Email == email);
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

        public override void Create(User obj)
        {
            Role user = Context.Roles.FirstOrDefault(x=>x.Name=="user");
            user.Users.Add(obj);
            Context.SaveChanges();
        }
    }
}
