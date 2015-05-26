using System;
using System.Collections.Generic;
using System.Linq;
using BookStore.DO.Entities;
using System.Data.Entity;
using BookStore.DAL.Interface.Abstract;


namespace BookStore.DAL.EntityFramework
{
    public class EfAuthorRepository : EfStoreRepository<Author>, IAuthorRepository
    {
        public Author GetByName(string lastName, string firstName)
        {
            using (EfDbContext context = new EfDbContext())
            {
                return context.Authors.FirstOrDefault(a => a.First_Name == firstName && a.Last_Name == lastName);
            }
        }

        public override IList<Author> GetAll()
        {
            using (EfDbContext context = new EfDbContext())
            {
                return context.Authors.Include(b => b.Books).OrderByDescending(x => x.AuthorDetail.FavoriteUsers.Count).ToList();
            }
        }

        public void AddBook(Book book, Author toAuthor)
        {
            using (EfDbContext context = new EfDbContext())
            {
                toAuthor.Books.Add(book);
                context.SaveChanges();
            }
        }

        public IList<Book> GetBooks(string author)
        {
            using (EfDbContext context = new EfDbContext())
            {
                if (author == null) throw new ArgumentNullException("author");
                Author firstOrDefault = context.Authors.FirstOrDefault(x => x.Last_Name == author);
                return firstOrDefault.Books.ToList();
            }
        }

        public override Author GetById(int id)
        {
            using (EfDbContext context = new EfDbContext())
            {
                return context.Authors.Include(x => x.Books.Select(c=>c.BookAuthors)).Include(x=>x.AuthorDetail.FavoriteUsers).FirstOrDefault(q => q.Author_ID == id);
            }
        }

        public override void Save(Author auth)
        {
            using (EfDbContext context = new EfDbContext())
            {
                Author authStore = context.Authors.Include(x=>x.AuthorDetail).FirstOrDefault(a => a.Author_ID == auth.Author_ID);
                if (authStore == null)
                {
                    context.Authors.Add(auth);
                }
                else
                {
                    authStore.First_Name = auth.First_Name;
                    authStore.Last_Name = auth.Last_Name;
                    authStore.Middle_Name = auth.Middle_Name;
                    authStore.AuthorDetail.Biography = auth.AuthorDetail.Biography;
                    authStore.AuthorDetail.Image_url = auth.AuthorDetail.Image_url;

                    ICollection<UserProfile> userNew = auth.AuthorDetail.FavoriteUsers;
                    ICollection<UserProfile> userOld = authStore.AuthorDetail.FavoriteUsers;
                    if (userNew != null)
                    {
                        foreach (var user in userNew)
                        {
                            if (userOld.Any(x => x.User_ID == user.User_ID)) continue;
                            var userForSave = context.Users.FirstOrDefault(a => a.User_ID == user.User_ID);
                            authStore.AuthorDetail.FavoriteUsers.Add(userForSave.Profile );
                        }
                    }

                    ICollection<Book> bookNew = auth.Books;
                    ICollection<Book> bookOld = authStore.Books;
                    if (bookNew != null)
                    {
                        foreach (var user in bookNew)
                        {
                            if (bookOld.Any(x => x.Book_ID == user.Book_ID)) continue;
                            var bookForSave = context.Books.FirstOrDefault(a => a.Book_ID == user.Book_ID);
                            authStore.Books.Add(bookForSave ?? new Book() { Book_ID = user.Book_ID });
                        }
                    }
                }
                context.SaveChanges();
            }
        }

        public override Author Delete(int id)
        {
            using (EfDbContext context = new EfDbContext())
            {
                Author a = context.Authors.Include(x => x.Books).Include(x=>x.AuthorDetail).FirstOrDefault(x => x.Author_ID == id);
                var books = new List<Book>(a.Books);
                foreach (var book in books)
                {
                    if (book.BookAuthors.Count == 1)
                    {
                        context.Books.Remove(book);
                    }
                }
                context.Authors.Remove(a);
                context.SaveChanges();
                return a;
            }
        }
    }
}
