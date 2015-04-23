using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.DO.Entities;

namespace BookStore.DAL.Interface.Abstract
{
    public interface IBookRepository : IStoreRepository<Book>
    {
        IList<Book> GetBooksByLetter(string letter);
        IList<Book> GetBooksByAuthor(string lastName);//TODO: sorting?
        IList<Book> GetBooksByGenre(string genre);
        IList<Book> GetBooksByTitle(string title);
        IList<Book> GetBooksByTag(int tagId);
        IList<Comment> GetComment(Comment comment);
        void AddComment(Comment comment);
        Rate GetRate(int bookId, int userId);
        IList<Book> GetAllWithDetail();

    }
}
