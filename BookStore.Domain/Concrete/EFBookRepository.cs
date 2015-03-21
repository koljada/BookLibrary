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
        public IQueryable<Author> Authors
        {
            get { return context.Authors; }
        }
        public void SaveBook(Book book)
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
                    dbEntry.Rate = book.Rate;
                    dbEntry.Genres = book.Genres;
                }
            }
            context.SaveChanges();
        }

        public void SaveAuthor(Author author)
        {
            if (author.AuthorID == 0)
            {
                context.Authors.Add(author);
            }
            else
            {
                Author dbEntry = context.Authors.Find(author.AuthorID);
                if (dbEntry != null)
                {
                    dbEntry.Name = author.Name;
                    dbEntry.Books = author.Books;
                    dbEntry.Biography = author.Biography;
                    dbEntry.ImageUrl = author.ImageUrl;
                    dbEntry.Rate = author.Rate;
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

        public Author DeleteAuthor(int authorID)
        {
            Author dbEntry = context.Authors.Find(authorID);
            if (dbEntry != null)
            {
                context.Authors.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
