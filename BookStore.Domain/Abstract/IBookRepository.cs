using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Domain.Entities;

namespace BookStore.Domain.Abstract
{
    public interface IBookRepository
    {
        IQueryable<Book> Books { get; }
        IQueryable<Author> Authors { get; }
        IQueryable<Tag> Tags { get; }
        ICollection<Tag> GetTags(Book book);
        IQueryable<User> Users { get; }
        IQueryable<Role> Roles { get; }
        void SaveBook(Book book);
        Book DeleteBook(int bookID);
        //void SaveAuthor(Author author);
       // Author DeleteAuthor(int authorID);
    }
}
