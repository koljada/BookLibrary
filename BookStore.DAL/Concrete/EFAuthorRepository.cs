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
    public class EFAuthorRepository:EFStoreRepository<Author>,IAuthorRepository
    {
        public Author GetByName(string last_name, string first_name)
        {
            return context.Authors.FirstOrDefault(a => a.First_Name == first_name && a.Last_Name == last_name);
        }
        public override IQueryable<Author> GetAll()
        {
            return context.Authors.Include(b=>b.Books);
        }
        public void AddBook(Book book, Author toAuthor)
        {
            toAuthor.Books.Add(book);
            context.SaveChanges();
        }
    }
}
