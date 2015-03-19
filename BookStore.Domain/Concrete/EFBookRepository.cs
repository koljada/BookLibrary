using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Domain.Entities;
using BookStore.Domain.Abstract;

namespace BookStore.Domain.Concrete
{
    public class EFBookRepository:IBookRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<Book> Books
        {
            get { return context.Books; }
        }
        public void SaveProduct(Book book)
        {
            if (book.BookID == 0)
            {
                context.Books.Add(book);
            }
            else
            {
                Book dbEntry = context.Books.Find(book.BookID);
                if (dbEntry != null)
                {
                    dbEntry.Title = book.Title;
                    dbEntry.Annotation = book.Annotation;
                    dbEntry.Price = book.Price;
                    dbEntry.Genre = book.Genre;
                    dbEntry.Author = book.Author;
                    dbEntry.Image_url = book.Image_url;
                }
            }
            context.SaveChanges();
        }

        public Book DeleteBook(int bookID)
        {
            Book dbEntry = context.Books.Find(bookID);
            if (dbEntry != null)
            {
                context.Books.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
