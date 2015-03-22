using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Entities
{
    public class Author
    {
        public int AuthorID { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Biography { get; set; }
        public int Rating { get; set; }
        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<User> FavotiteUsers { get; set; }

    }
}
