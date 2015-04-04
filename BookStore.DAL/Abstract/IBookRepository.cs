using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.DO.Entities;

namespace BookStore.DAL.Abstract
{
    public interface IBookRepository : IStoreRepository<Book>
    {
        IQueryable<Book> GetBooksByLetter(string letter);
        IQueryable<Book> GetBooksByAuthor(string last_name);//TODO: sorting?
        IQueryable<Book> GetBooksByGenre(string genre);
        IQueryable<Book> GetBooksByTitle(string title);
        IQueryable<Book> GetBooksByTag(int tagID);
        IQueryable<Comment> GetComment(Comment comment);

    }
}
