using BookStore.DO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DAL.Interface.Abstract
{
    public interface IAuthorRepository : IStoreRepository<Author>
    {
        Author GetByName(string lastName, string firstName);
        void AddBook(Book book, Author toAuthor);
        IList<Book> GetBooks(string author);
    }
}
