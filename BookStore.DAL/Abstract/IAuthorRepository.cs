using BookStore.DO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DAL.Abstract
{
    public interface IAuthorRepository:IStoreRepository<Author>
    {
        Author GetByName(string last_name, string first_name);
        void AddBook(Book book, Author toAuthor);
    }
}
