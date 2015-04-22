using BookStore.DO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DAL.Interface.Abstract
{
    public interface IGenreRepository:IStoreRepository<Genre>
    {
        IList<Book> GetBooks(string genre);
    }
}
