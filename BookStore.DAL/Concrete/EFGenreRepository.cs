using BookStore.DAL.Abstract;
using BookStore.DO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BookStore.DAL.Concrete
{
    public class EFGenreRepository:EFStoreRepository<Genre>,IGenreRepository
    {

        public override IQueryable<Genre> GetAll()
        {
            return context.Genres;//TODO: Include Books
        }


        public IQueryable<Book> getBooks(string genre)
        {
            return context.Genres.FirstOrDefault(g => g.Genre_Name == genre).Books.AsQueryable<Book>();
        }
    }
}
