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
    public class EFBookRepository : EFStoreRepository<Book>, IBookRepository
    {
        public IQueryable<Book> GetBooksByLetter(string letter)
        {
            var num = Enumerable.Range(0, 10).Select(i => i.ToString());
            return context.Books.Include(b=>b.Authors).Include(b=>b.Genres).Include(b=>b.Tages)
                .Where(p => letter == "All" 
                            || p.Title.StartsWith(letter)
                            || (num.Contains(p.Title.Substring(0, 1)) && letter == "0-9"));
        }

        public IQueryable<Book> GetBooksByAuthor(string last_name)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Book> GetBooksByGenre(string genre)
        {
            return context.Books.Include(b => b.Authors).Include(b => b.Genres).Include(b => b.Tages)
                  .Where(p => genre == null || p.Genres.Any(g=>g.Genre_Name==genre));
        }

        public IQueryable<Book> GetBooksByTitle(string title)
        {
            throw new NotImplementedException();

        }

        public IQueryable<Book> GetBooksByTag(int tagID)
        {
            return context.Books.Include(b => b.Authors).Include(b => b.Genres).Include(b => b.Tages)
                .Where(b => b.Tages.Any(t => t.Tag_ID == tagID));
        }

        public IQueryable<Comment> GetComment(Comment comment)
        {
            throw new NotImplementedException();
        }

        public override IQueryable<Book> GetAll()
        {
            return context.Books.Include(a => a.Authors).Include(a => a.Genres).Include(a => a.Tages);
        }
        public override Book GetByID(int ID)
        {
            return context.Books.Include(a => a.Authors).Include(a => a.Genres).Include(a => a.Tages).FirstOrDefault(b => b.Book_ID == ID);
        }
    }
}
