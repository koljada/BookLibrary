using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Domain.Entities;
using BookStore.Domain.Abstract;
using System.Data.Entity;

namespace BookStore.Domain.Concrete
{
    public class EFBookRepository : IBookRepository
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
        public IQueryable<Tag> Tags
        {
            get { return context.Tages; }
        }

        public ICollection<Tag> GetTags(Book book)
        {
            ICollection<Tag> result = new List<Tag>();
            Tag tagTitle = context.Tages.FirstOrDefault(t => t.Tag_Name == book.Title);
            if (tagTitle == null)
            {
                tagTitle = new Tag() { Tag_Name = book.Title };
            }
            result.Add(tagTitle);
            Tag tagAuthor = context.Tages.FirstOrDefault(t => t.Tag_Name == book.Author.Last_Name);
            if (tagAuthor == null)
            {
                tagAuthor = new Tag() { Tag_Name = book.Author.Last_Name };
            }
            result.Add(tagAuthor);           
            return result;
        }
        public void SaveBook(Book book)
        {
            if (book.Book_ID == 0)
            {
                context.Books.Add(book);
            }
            else
            {
                IQueryable<string> tags = context.Tages.Select(t=>t.Tag_Name);
               List<Tag> tagForSave=new List<Tag>();
                Book dbEntry = context.Books.Include(x=>x.Author).Include(c=>c.Tages).FirstOrDefault(x=>x.Book_ID==book.Book_ID);
                if (dbEntry != null)
                {
                   foreach (Tag tag in book.Tages) {
                       if (!tags.Contains(tag.Tag_Name))
                       {
                           tagForSave.Add(new Tag() { Tag_Name=tag.Tag_Name});
                       }
                       else
                       {
                           tagForSave.Add(context.Tages.FirstOrDefault(t => t.Tag_Name == tag.Tag_Name));
                       }
                   }
                    dbEntry.Tages = tagForSave;
                    dbEntry.Title = book.Title;
                    dbEntry.Annotation = book.Annotation;
                    dbEntry.Price = book.Price;
                    dbEntry.Genre = book.Genre;
                    dbEntry.Author_ID = book.Author_ID;
                    dbEntry.Image_url = book.Image_url;
                    dbEntry.Rating = book.Rating;
                    dbEntry.RatedUsers = book.RatedUsers;
                }
            }
            context.SaveChanges();
        }

        //public void SaveAuthor(Author author)
        //{
        //    if (author.AuthorID == 0)
        //    {
        //        context.Authors.Add(author);
        //    }
        //    else
        //    {
        //        Author dbEntry = context.Authors.Find(author.AuthorID);
        //        if (dbEntry != null)
        //        {
        //            dbEntry.Name = author.Name;
        //            dbEntry.Books = author.Books;
        //            dbEntry.Biography = author.Biography;
        //            dbEntry.ImageUrl = author.ImageUrl;
        //            dbEntry.Rating = author.Rating;
        //        }
        //    }
        //    context.SaveChanges();
        //}

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

        //public Author DeleteAuthor(int authorID)
        //{
        //    Author dbEntry = context.Authors.Find(authorID);
        //    if (dbEntry != null)
        //    {
        //        context.Authors.Remove(dbEntry);
        //        context.SaveChanges();
        //    }
        //    return dbEntry;
        //}
    }
}
