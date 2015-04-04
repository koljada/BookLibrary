using BookStore.DO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DAL.Abstract
{
    public interface IUserRepository : IStoreRepository<User>
    {
        IQueryable<Book> GetReccomendedBooks(int userID);
        IQueryable<Book> GetWishedBooks(int userID);
        IQueryable<Book> GetRatedBooks(int userID);
        IQueryable<Author> GetFavAuthors(int userID);
        ICollection<Role> GetRoles(int userID);
        IQueryable<Comment> GetComment(int userID);
        User GetUserByEmail(string email);
        void RateBook(Book book);
        void WishBook(Book book);
        void AddComment(Book book);
        void LikeAuthor(Author author);
    }
}
