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
            return Context.Authors.Include(b=>b.Books);
        }
        public void AddBook(Book book, Author toAuthor)
        {
            toAuthor.Books.Add(book);
            Context.SaveChanges();
        }
    }
}
