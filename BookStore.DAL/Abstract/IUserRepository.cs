using BookStore.DO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DAL.Interface.Abstract
{
    public interface IUserRepository : IStoreRepository<User>
    {
        IList<Book> GetReccomendedBooks(int userId);
        IList<Book> GetWishedBooks(int userId);
        IList<Rate> GetRatedBooks(int userId);
        IList<Author> GetFavAuthors(int userId);
        IList<Role> GetRoles(int userId);
        IList<Comment> GetComment(int userId);
        User GetUserByEmail(string email);
        Task RateBook(float rate,int userId, int bookId,bool isSuggestion);
        void WishBook(int bookId, int userId);
        void AddComment(Book book);
        void LikeAuthor(int authorId, int userId);
       // void Resuggest();
    }
}
