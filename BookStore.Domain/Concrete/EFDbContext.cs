using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Domain.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace BookStore.Domain
{
    public class EFDbContext: DbContext
    {
        public EFDbContext() : base() {
            //this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Tag> Tages { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Role> Roles { get; set; }
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    //modelBuilder.Entity<Book>().HasRequired(x => x.Author).WithMany(x=>x.Books).HasForeignKey(x=>x.AuthorID);
        //    modelBuilder.Entity<Author>().HasMany<Book>(b => b.Books).WithRequired(b => b.Author).HasForeignKey(b => b.AuthorID);
        //    base.OnModelCreating(modelBuilder);
        //}
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           // modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<User>()
                .HasMany(c => c.ReccomendedBooks).WithMany(i => i.ReccomendedUsers)
                .Map(t => t.MapLeftKey("User_ID")
                    .MapRightKey("Book_ID")
                    .ToTable("Reccomenation"));
            modelBuilder.Entity<User>()
                .HasMany(c => c.WishedBooks).WithMany(i => i.WishedUsers)
                .Map(t => t.MapLeftKey("User_ID")
                    .MapRightKey("Book_ID")
                    .ToTable("Wish"));
            //modelBuilder.Entity<User>().HasRequired(p => p.Role).WithMany(b => b.Users).HasForeignKey(p => p.Role_ID);
            
            
        } 


    }
}
