using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.DO.Entities;

namespace BookStore.DLL.Abstract
{
    public interface IBookService : IStoreService<Book>
    {
        IQueryable<Book> GetBooksByLetter(string letter);
        IQueryable<Book> GetBooksByAuthor(string lastName);//TODO: sorting?
        IQueryable<Book> GetBooksByGenre(string genre);
        IQueryable<Book> GetBooksByTitle(string title);
        IQueryable<Book> GetBooksByTag(int tagId);
        IQueryable<Comment> GetComment(Comment comment);
        void AddComment(Comment comment);
        Rate GetRate(int bookId, int userId);

    }
}
