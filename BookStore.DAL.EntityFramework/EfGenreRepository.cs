using System.Linq;
using BookStore.DAL.Abstract;
using BookStore.DO.Entities;
using System.Data.Entity; 


namespace BookStore.DAL.EntityFramework
{
    public class EfGenreRepository:EfStoreRepository<Genre>,IGenreRepository
    {

        public override IQueryable<Genre> GetAll()
        {
            return Context.Genres;//TODO: Include Books
        }


        public IQueryable<Book> getBooks(string genre)
        {
            return Context.Genres.FirstOrDefault(g => g.Genre_Name == genre).Books.AsQueryable();
        }
    }
}
