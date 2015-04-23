using BookStore.DO.Entities;
using System.Data.Entity;

namespace BookStore.DAL.EntityFramework
{
    public class EfDbContext : DbContext
    {
        public EfDbContext()
            : base()
        {
            //this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookDetail> BookDetails { get; set; }

        public DbSet<Tag> Tages { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<AuthorDetail> AuthorDetails { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<Rate> Rates { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Comment> Comments { get; set; }
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    //modelBuilder.Entity<Book>().HasRequired(x => x.Author).WithMany(x=>x.Books).HasForeignKey(x=>x.AuthorID);
        //    modelBuilder.Entity<Author>().HasMany<Book>(b => b.Books).WithRequired(b => b.Author).HasForeignKey(b => b.AuthorID);
        //    base.OnModelCreating(modelBuilder);
        //}
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<UserProfile>()
                .HasMany(c => c.ReccomendedBooks).WithMany(i => i.ReccomendedUsers)
                .Map(t => t.MapLeftKey("User_ID")
                .MapRightKey("Book_ID")
                .ToTable("Recommendation"));
            modelBuilder.Entity<UserProfile>()
                .HasMany(c => c.WishedBooks).WithMany(i => i.WishedUsers)
                .Map(t => t.MapLeftKey("User_ID")
                .MapRightKey("Book_ID")
                .ToTable("Wishes"));
            modelBuilder.Entity<Book>()
                .HasMany(c => c.BookAuthors).WithMany(i => i.Books)
                .Map(t => t.MapLeftKey("Book_ID")
                    .MapRightKey("Author_ID"));
        }
    }
}
