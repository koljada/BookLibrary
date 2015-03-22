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
        public EFDbContext() : base() {
            //this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Author> Authors { get; set; }
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    //modelBuilder.Entity<Book>().HasRequired(x => x.Author).WithMany(x=>x.Books).HasForeignKey(x=>x.AuthorID);
        //    modelBuilder.Entity<Author>().HasMany<Book>(b => b.Books).WithRequired(b => b.Author).HasForeignKey(b => b.AuthorID);
        //    base.OnModelCreating(modelBuilder);
        //}

    }
}
