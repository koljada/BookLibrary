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
            return context.Books.Include(b => b.Authors).Include(b => b.Genres).Include(b => b.Tages)
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
                  .Where(p => genre == null || p.Genres.Any(g => g.Genre_Name == genre));
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
        public override Book Delete(int ID)
        {
            Book book = context.Books.FirstOrDefault(b => b.Book_ID == ID);
            if (book != null)
            {
                context.Books.Remove(book);
                context.SaveChanges();
            }
            return book;
        }
        public override void Save(Book obj)
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
                ICollection<Author> authorsNew = obj.Authors;
                ICollection<Author> authorsOld = bookForSave.Authors;
                foreach (var author in authorsNew)
                {
                    if (!authorsOld.Any(x => x.Last_Name == author.Last_Name && x.First_Name == author.First_Name))
                    {
                        var _author = context.Authors.FirstOrDefault(a => a.Last_Name == author.Last_Name && author.First_Name == a.First_Name);
                        if (_author != null)
                        {
                            bookForSave.Authors.Add(author);
                        }
                        else
                        {
                            bookForSave.Authors.Add(new Author() { Last_Name = author.Last_Name, First_Name = author.First_Name, Middle_Name = author.Middle_Name });
                        }
                    }
                }
                ICollection<Tag> tagsNew = obj.Tages;
                ICollection<Tag> tagsOld = bookForSave.Tages;
                if (tagsNew != null)
                {
                    foreach (var tag in tagsNew)
                    {
                        if (!tagsOld.Any(x => x.Tag_Name == tag.Tag_Name))
                        {
                            var _tag = context.Tages.FirstOrDefault(a => a.Tag_Name == tag.Tag_Name);
                            if (_tag != null)
                            {
                                bookForSave.Tages.Add(_tag);
                            }
                            else
                            {
                                bookForSave.Tages.Add(new Tag() { Tag_Name = tag.Tag_Name });
                            }
                        }
                    }
                }
                ICollection<Genre> genresNew = obj.Genres;
                ICollection<Genre> genresOld = bookForSave.Genres;
                if (genresNew != null)
                {
                    foreach (var genre in genresNew)
                    {
                        if (!genresOld.Any(x => x.Genre_Name == genre.Genre_Name))
                        {
                            var _genre = context.Genres.FirstOrDefault(a => a.Genre_Name == genre.Genre_Name);
                            if (_genre != null)
                            {
                                bookForSave.Genres.Add(_genre);
                            }
                            else
                            {
                                bookForSave.Genres.Add(new Genre() { Genre_Name = genre.Genre_Name });
                            }
                        }
                    }
                }
            }
            context.SaveChanges();
        }
        public override void Create(Book obj)
        {
            ICollection<Author> authors = obj.Authors;
            obj.Authors = null;
            foreach (var author in authors)
            {
                Author authorForSave = context.Authors.FirstOrDefault(a => a.Last_Name == author.Last_Name && author.First_Name == a.First_Name);
                if (authorForSave == null)
                {
                    authorForSave = new Author() { Last_Name = author.Last_Name, First_Name = author.First_Name, Middle_Name = author.Middle_Name };
                }
                authorForSave.Books.Add(obj);
            }
            context.SaveChanges();
        }
    }
}
