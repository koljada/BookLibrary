namespace BookStore.Domain.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using BookStore.Domain.Entities;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<BookStore.Domain.EFDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "BookStore.Domain.EFDbContext";
        }

        protected override void Seed(BookStore.Domain.EFDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //


            Book book = new Book()
            {
                Title = "White Fang",
                Annotation = "White Fang is a novel by American author Jack London (1876–1916) — and the name of the book's eponymous character, a wild wolfdog. First serialized in Outing magazine, it was published in 1906. The story takes place in Yukon Territory, Canada, during the 1890s Klondike Gold Rush and details White Fang's journey to domestication. It is a companion novel (and a thematic mirror) to London's best-known work, The Call of the Wild, which is about a kidnapped, domesticated dog embracing his wild ancestry to survive and thrive in the wild.",
                Genre = "Story",
                Rating = 8,
                Price = 342,
                Image_url = "http://upload.wikimedia.org/wikipedia/commons/thumb/1/14/JackLondonwhitefang1.jpg/220px-JackLondonwhitefang1.jpg"
            };
            List<Book> bookList = new List<Book>() { book };
            Author author = new Author()
            {
                First_Name = "Jack",
                Last_Name="London",
                Biography = "John Griffith London (born John Griffith Chaney,January 12, 1876 – November 22, 1916) was an American author, journalist, and social activist. He was a pioneer in the then-burgeoning world of commercial magazine fiction and was one of the first fiction writers to obtain worldwide celebrity and a large fortune from his fiction alone. Some of his most famous works include The Call of the Wild and White Fang, both set in the Klondike Gold Rush, as well as the short stories To Build a Fire An Odyssey of the North, and Love of Life. He also wrote of the South Pacific in such stories as The Pearls of Parlay and The Heathen, and of the San Francisco Bay area in The Sea Wolf.",
                Books = bookList,
                Rating = 7,
                Image_Url = "http://upload.wikimedia.org/wikipedia/commons/thumb/2/2d/Jack_London_young.jpg/250px-Jack_London_young.jpg"
            };
            User user = new User()
            {
                Avatar_Url = "data:image/jpeg;base64,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",
                Birthday = DateTime.Today,
                Email = "user@user.com",
                FavoriteAuthors = new List<Author>() { author },
                First_Name = "User",
                Last_Name = "User",
                Password = "1234",
                Rating = 2,
                Sex = "Male",
            };
            List<Rate> rate = new List<Rate>(){new Rate() {
                Book_ID = book.Book_ID,
                User_ID = user.User_ID,
                RateValue = 9
            }};
            user.RatedBooks = rate;
            book.RatedUsers = rate;
            Tag tag = new Tag() { Tag_Name = book.Title, Books = bookList };
            book.Tages = new List<Tag>() { tag };
            context.Users.Add(user);
            context.SaveChanges();
        }
    }
}
