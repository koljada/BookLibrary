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
        IQueryable<Book> GetReccomendedBooks(int userId);
        IQueryable<Book> GetWishedBooks(int userId);
        IQueryable<Book> GetRatedBooks(int userId);
        IQueryable<Author> GetFavAuthors(int userId);
        ICollection<Role> GetRoles(int userId);
        IQueryable<Comment> GetComment(int userId);
        User GetUserByEmail(string email);
        void RateBook(Rate rate, int bookId);
        void WishBook(Book book);
        void AddComment(Book book);
        void LikeAuthor(Author author);
    }
}
