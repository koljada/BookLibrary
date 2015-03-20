using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Domain.Entities;
using System.Data.Entity;

namespace BookStore.Domain
{
    public class EFDbContext: DbContext
    {
        public EFDbContext() : base() { }
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Author> Authors { get; set; }

    }
}
