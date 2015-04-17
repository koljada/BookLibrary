using System.Collections.Generic;
using System.Linq;
using BookStore.DAL.Abstract;
using BookStore.DO.Entities;
using System.Data.Entity;


namespace BookStore.DAL.EntityFramework
{
    public class EfAuthorRepository : EfStoreRepository<Author>, IAuthorRepository
    {
        public Author GetByName(string lastName, string firstName)
        {
            return Context.Authors.FirstOrDefault(a => a.First_Name == firstName && a.Last_Name == lastName);
        }
        public override IList<Author> GetAll()
        {
            return Context.Authors.Include(b => b.Books).OrderByDescending(t => t.Rating).ToList();
        }
        public void AddBook(Book book, Author toAuthor)
        {
            toAuthor.Books.Add(book);
            Context.SaveChanges();
        }

        public IList<Book> GetBooks(string author)
        {
            return Context.Authors.FirstOrDefault(x => x.Last_Name == author).Books.ToList();
        }

        public override Author GetById(int id)
        {
            return Context.Authors.Include(x => x.Books).FirstOrDefault(x => x.Author_ID == id);
        }

        public override void Save(Author auth)
        {
            Author authStore = Context.Authors.FirstOrDefault(a => a.Author_ID == auth.Author_ID);
            if (authStore == null)
            {
                Context.Authors.Add(auth);
            }
            else
            {
                authStore.First_Name = auth.First_Name;
                authStore.Last_Name = auth.Last_Name;
                authStore.Middle_Name = auth.Middle_Name;
                authStore.Rating = auth.Rating;
                authStore.Biography = auth.Biography;
                authStore.Image_url = auth.Image_url;

                ICollection<User> userNew = auth.FavotiteUsers;
                ICollection<User> userOld = authStore.FavotiteUsers;
                if (userNew != null)
                {
                    foreach (var user in userNew)
                    {
                        if (userOld.Any(x => x.User_ID == user.User_ID)) continue;
                        var userForSave = Context.Users.FirstOrDefault(a => a.User_ID == user.User_ID);
                        authStore.FavotiteUsers.Add(userForSave ?? new User() { User_ID = user.User_ID });
                    }
                }

                ICollection<Book> bookNew = auth.Books;
                ICollection<Book> bookOld = authStore.Books;
                if (bookNew != null)
                {
                    foreach (var user in bookNew)
                    {
                        if (bookOld.Any(x => x.Book_ID == user.Book_ID)) continue;
                        var bookForSave = Context.Books.FirstOrDefault(a => a.Book_ID == user.Book_ID);
                        authStore.Books.Add(bookForSave ?? new Book() { Book_ID = user.Book_ID });
                    }
                }
            }
            Context.SaveChanges();
        }

        public override Author Delete(int id)
        {
            var a = Context.Authors.Include(x => x.Books).FirstOrDefault(x => x.Author_ID == id);
            var books = new List<Book>(a.Books);
            foreach (var book in books)
            {
                if (book.BookAuthors.Count == 1)
                {
                    Context.Books.Remove(book);
                }
            }
            Context.Authors.Remove(a);
            Context.SaveChanges();
            return a;
        }
    }
}
