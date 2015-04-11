using System.Collections.Generic;
using System.Linq;
using BookStore.DAL.Abstract;
using BookStore.DO.Entities;
using System.Data.Entity; 


namespace BookStore.DAL.EntityFramework
{
    public class EfAuthorRepository:EfStoreRepository<Author>,IAuthorRepository
    {
        public Author GetByName(string lastName, string firstName)
        {
            return Context.Authors.FirstOrDefault(a => a.First_Name == firstName && a.Last_Name == lastName);
        }
        public override IQueryable<Author> GetAll()
        {
            return Context.Authors.Include(b => b.Books).OrderByDescending(t => t.Rating);
        }
        public void AddBook(Book book, Author toAuthor)
        {
            toAuthor.Books.Add(book);
            Context.SaveChanges();
        }

        public ICollection<Book> GetBooks(string author)
        {
            return Context.Authors.FirstOrDefault(x => x.Last_Name == author).Books;
        }

        public override Author GetById(int id)
        {
            return Context.Authors.Include(x=>x.Books).FirstOrDefault(x => x.Author_ID == id);
        }
    }
}
