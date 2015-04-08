using System;
using System.Collections.Generic;
using System.Linq;
using BookStore.DAL.Abstract;
using BookStore.DO.Entities;
using System.Data.Entity; 

namespace BookStore.DAL.EntityFramework
{
    public class EfBookRepository : EfStoreRepository<Book>, IBookRepository
    {
        public IQueryable<Book> GetBooksByLetter(string letter)
        {
            var num = Enumerable.Range(0, 10).Select(i => i.ToString());
            return Context.Books.Include(b => b.BookAuthors).Include(b => b.Genres).Include(b => b.Tages).Where( p => letter == "All"
                            || p.Title.StartsWith(letter)
                            || (num.Contains(p.Title.Substring(0, 1)) && letter == "0-9"));
        }

        public IQueryable<Book> GetBooksByAuthor(string lastName)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Book> GetBooksByGenre(string genre)
        {
            return Context.Books.Include(b => b.BookAuthors).Include(b => b.Genres).Include(b => b.Tages).Where( p => p.Genres.Any( g => g.Genre_Name == genre));
        }

        public IQueryable<Book> GetBooksByTitle(string title)
        {
            throw new NotImplementedException();

        }

        public IQueryable<Book> GetBooksByTag(int tagId)
        {
            return Context.Books.Include(b => b.BookAuthors).Include(b => b.Genres).Include(b => b.Tages).Where(b => b.Tages.Any(t => t.Tag_ID == tagId));
        }

        public IQueryable<Comment> GetComment(Comment comment)
        {
            throw new NotImplementedException();
        }

        public override IQueryable<Book> GetAll()
        {
            return Context.Books.Include(a => a.BookAuthors).Include(a => a.Genres).Include(a => a.Tages);
        }
        public override Book GetById(int id)
        {
            return Context.Books.Include(a => a.BookAuthors).Include(a => a.Genres).Include(a => a.Tages).FirstOrDefault( b => b.Book_ID == id);
        }
        public override Book Delete(int id)
        {
            Book book = Context.Books.FirstOrDefault(b => b.Book_ID == id);
            if (book != null)
            {
                Context.Books.Remove(book);
                Context.SaveChanges();
            }
            return book;
        }
        public override void Save(Book obj)
        {
            Book bookForSave = Context.Books.FirstOrDefault(b => b.Book_ID == obj.Book_ID);
            if (bookForSave == null)
            {
                Context.Books.Add(obj);
            }
            else
            {
                bookForSave.Annotation = obj.Annotation;
                bookForSave.Image_url = obj.Image_url;
                bookForSave.Price = obj.Price;
                bookForSave.Rating = obj.Rating;
                bookForSave.Title = obj.Title;
                ICollection<Author> authorsNew = obj.BookAuthors;
                ICollection<Author> authorsOld = bookForSave.BookAuthors;
                foreach (var author in authorsNew)
                {
                    if (authorsOld.Any(x => x.Last_Name == author.Last_Name && x.First_Name == author.First_Name))
                        continue;
                    var authorForSave = Context.Authors.FirstOrDefault( a => a.Last_Name == author.Last_Name && author.First_Name == a.First_Name);
                    bookForSave.BookAuthors.Add(authorForSave != null
                        ? author
                        : new Author()
                        {
                            Last_Name = author.Last_Name,
                            First_Name = author.First_Name,
                            Middle_Name = author.Middle_Name
                        });
                }
                ICollection<Tag> tagsNew = obj.Tages;
                ICollection<Tag> tagsOld = bookForSave.Tages;
                if (tagsNew != null)
                {
                    foreach (var tag in tagsNew)
                    {
                        if (tagsOld.Any(x => x.Tag_Name == tag.Tag_Name)) continue;
                        var tagForSave = Context.Tages.FirstOrDefault( a => a.Tag_Name == tag.Tag_Name);
                        bookForSave.Tages.Add(tagForSave ?? new Tag() { Tag_Name = tag.Tag_Name });
                    }
                }
                ICollection<Genre> genresNew = obj.Genres;
                ICollection<Genre> genresOld = bookForSave.Genres;
                if (genresNew != null)
                {
                    foreach (var genre in genresNew)
                    {
                        if (genresOld.Any(x => x.Genre_Name == genre.Genre_Name)) continue;
                        var genreForSave = Context.Genres.FirstOrDefault(a => a.Genre_Name == genre.Genre_Name);
                        bookForSave.Genres.Add(genreForSave ?? new Genre() { Genre_Name = genre.Genre_Name });
                    }
                }
            }
            Context.SaveChanges();
        }
        public override void Create(Book obj)
        {
            ICollection<Author> authors = obj.BookAuthors;
            obj.BookAuthors = new List<Author>();
            foreach (var author in authors)
            {
                Author authorForSave = Context
                    .Authors
                    .FirstOrDefault(a => a.Last_Name == author.Last_Name && author.First_Name == a.First_Name) ??
                                       new Author() { Last_Name = author.Last_Name, First_Name = author.First_Name, Middle_Name = author.Middle_Name };

                obj.BookAuthors.Add(authorForSave);
            }
            Context.Books.Add(obj);
            Context.SaveChanges();
        }
    }
}
