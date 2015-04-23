using System.Collections.Generic;
using System.Linq;
using BookStore.DO.Entities;
using System.Data.Entity;
using BookStore.DAL.Interface.Abstract;


namespace BookStore.DAL.EntityFramework
{
    public class EfGenreRepository : EfStoreRepository<Genre>, IGenreRepository
    {

        public override IList<Genre> GetAll()
        {
            using (EfDbContext context = new EfDbContext())
            {
                return context.Genres.ToList(); 
            }
        }

        public IList<Book> GetBooks(string genre)
        {
            using (EfDbContext context = new EfDbContext())
            {
                return context.Genres.FirstOrDefault(g => g.Genre_Name == genre).Books.Select(x=>x.Book).ToList();
            }
        }
    }
}
