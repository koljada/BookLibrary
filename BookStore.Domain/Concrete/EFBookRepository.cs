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
    }
}
