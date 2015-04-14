using BookStore.DO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DLL.Abstract
{
    public interface IAuthorService:IStoreService<Author>
    {
        Author GetByName(string lastName, string firstName);
        void AddBook(Book book, Author toAuthor);
        ICollection<Book> GetBooks(string author);
    }
}
