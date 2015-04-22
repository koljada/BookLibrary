using System;
using System.Collections.Generic;
using System.Linq;
using BookStore.DO.Entities;
using System.Data.Entity;
using BookStore.DAL.Interface.Abstract;

namespace BookStore.DAL.EntityFramework
{
    public class EfBookRepository : EfStoreRepository<Book>, IBookRepository
    {
        public IList<Book> GetBooksByLetter(string letter)
        {
            using (EfDbContext context = new EfDbContext())
            {
                var num = Enumerable.Range(0, 10).Select(i => i.ToString());
                return context.Books
                    .Include(b => b.BookAuthors)
                    //.Include(b => b.Genres)
                    //.Include(b => b.Tages)
                    .Where(p =>letter == "All" || p.Title.StartsWith(letter) ||(num.Contains(p.Title.Substring(0, 1)) && letter == "0-9")).ToList();
            }
        }

        public IList<Book> GetBooksByAuthor(string lastName)
        {
            throw new NotImplementedException();
        }

        public IList<Book> GetBooksByGenre(string genre)
        {
            using (EfDbContext context = new EfDbContext())
            {
                Genre curGenre = context.Genres.Include(x=>x.Books).FirstOrDefault(x => x.Genre_Name == genre);
                GetChilds(curGenre.Genre_ID);
                List<Book> books = context.Books
                    .Include(x=>x.BookAuthors)
                    .Where(x=>x.Genres.Select(c=>c.Genre_ID).Contains(curGenre.Genre_ID)).ToList();
                //if (_childs.Any())
                //{
                //    foreach (var g in _childs.Where(x=>x.Books.Any()))
                //    {
                //        books.AddRange(g.Books);
                //    }
                //}
                return books;
            }
        }

        private readonly List<Genre> _childs = new List<Genre>();
        private void GetChilds(int id)
        {
            using (EfDbContext context = new EfDbContext())
            {
                var genres = context.Genres.Include(x=>x.Books).Where(x => x.ParentID == id).ToList();
                if (genres.Any())
                {
                    foreach (var genre in genres)
                    {
                        _childs.Add(genre);
                        GetChilds(genre.Genre_ID);
                    }
                }
            }
        }

        public IList<Book> GetBooksByTitle(string title)
        {
            throw new NotImplementedException();

        }

        public IList<Book> GetBooksByTag(int tagId)
        {
            using (EfDbContext context = new EfDbContext())
            {
                return context.Books
                    .Include(b => b.BookAuthors)
                    //.Include(b => b.Genres)
                    //.Include(b => b.Tages)
                    .Where(b => b.Tages.Any(t => t.Tag_ID == tagId)).ToList();
            }
        }

        public IList<Comment> GetComment(Comment comment)
        {
            throw new NotImplementedException();
        }

        public override IList<Book> GetAll()
        {
            using (EfDbContext context = new EfDbContext())
            {
                return context.Books.Include(a => a.BookAuthors).ToList();
                    //.Include(a => a.Genres)
                    //.Include(a => a.Tages)
                    //.OrderByDescending(b => b.Rating)
            }
        }

        public override Book GetById(int id)
        {
            using (EfDbContext context = new EfDbContext())
            {
                var book = context.Books.Include(x=>x.RatedUsers)
                    .Include(a => a.BookAuthors)
                    .Include(a => a.Genres)
                    .Include(a => a.Tages)
                    .Include(x => x.Comments)
                    .Include(x=>x.WishedUsers)
                    .FirstOrDefault(b => b.Book_ID == id);
                return book;
            }
        }

        public override Book Delete(int id)
        {
            using (EfDbContext context = new EfDbContext())
            {
                Book book = context.Books.FirstOrDefault(b => b.Book_ID == id);
                if (book != null)
                {
                    context.Books.Remove(book);
                    context.SaveChanges();
                }
                return book;
            }
        }

        public override void Save(Book obj)
        {
            using (EfDbContext context = new EfDbContext())
            {
                Book bookForSave = context.Books.FirstOrDefault(b => b.Book_ID == obj.Book_ID);
                if (bookForSave == null)
                {
                    context.Books.Add(obj);
                }
                else
                {
                    bookForSave.Annotation = obj.Annotation;
                    bookForSave.Image_url = obj.Image_url;
                    bookForSave.Price = obj.Price;
                    bookForSave.Rating = obj.Rating;
                    bookForSave.Title = obj.Title;
                    bookForSave.ContentUrl = obj.ContentUrl;
                    ICollection<Author> authorsNew = obj.BookAuthors;
                    ICollection<Author> authorsOld = bookForSave.BookAuthors;
                    foreach (var author in authorsNew)
                    {
                        if (authorsOld.Any(x => x.Last_Name == author.Last_Name && x.First_Name == author.First_Name))
                            continue;
                        var authorForSave =
                            context.Authors.FirstOrDefault(
                                a => a.Last_Name == author.Last_Name && author.First_Name == a.First_Name);
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
                            var tagForSave = context.Tages.FirstOrDefault(a => a.Tag_Name == tag.Tag_Name);
                            bookForSave.Tages.Add(tagForSave ?? new Tag { Tag_Name = tag.Tag_Name });
                        }
                    }
                    ICollection<Genre> genresNew = obj.Genres;
                    ICollection<Genre> genresOld = bookForSave.Genres;
                    if (genresNew != null)
                    {
                        foreach (var genre in genresNew)
                        {
                            if (genresOld.Any(x => x.Genre_Name == genre.Genre_Name)) continue;
                            var genreForSave = context.Genres.FirstOrDefault(a => a.Genre_Name == genre.Genre_Name);
                            bookForSave.Genres.Add(genreForSave ?? new Genre() { Genre_Name = genre.Genre_Name });
                        }
                    }
                }
                context.SaveChanges();
            }
        }

        public override void Create(Book obj)
        {
            using (EfDbContext context = new EfDbContext())
            {
                ICollection<Author> authors = obj.BookAuthors;
                obj.BookAuthors = new List<Author>();
                foreach (var author in authors)
                {
                    Author authorForSave = context
                        .Authors
                        .FirstOrDefault(a => a.Last_Name == author.Last_Name && author.First_Name == a.First_Name) ??
                                           new Author
                                           {
                                               Last_Name = author.Last_Name,
                                               First_Name = author.First_Name,
                                               Middle_Name = author.Middle_Name
                                           };
                    obj.BookAuthors.Add(authorForSave);
                }
                context.Books.Add(obj);
                context.SaveChanges();
            }
        }


        public void AddComment(Comment comment)
        {
            using (EfDbContext context = new EfDbContext())
            {
                context.Comments.Add(comment);
                context.SaveChanges();
            }
        }

        public Rate GetRate(int bookId, int userId)
        {
            using (EfDbContext context = new EfDbContext())
            {
                return context.Rates.FirstOrDefault(x => x.User_ID == userId && x.Book.Book_ID == bookId);
            }
        }
    }
}
