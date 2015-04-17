using System.Collections.Generic;
using System.Linq;
using BookStore.DAL.Abstract;
using BookStore.DO.Entities;
using System.Data.Entity; 


namespace BookStore.DAL.EntityFramework
{
    public class EfGenreRepository:EfStoreRepository<Genre>,IGenreRepository
    {

        public override IList<Genre> GetAll()
        {
            return Context.Genres.ToList();//TODO: Include Books
        }


        public IList<Book> GetBooks(string genre)
        {
            return Context.Genres.FirstOrDefault(g => g.Genre_Name == genre).Books.ToList();
        }
    }
}
