using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Entities
{
    public class Author
    {
        [Key]
        public int Author_ID { get; set; }
        public string Last_Name { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Image_Url { get; set; }
        public string Biography { get; set; }
        public int Rating { get; set; }
        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<User> FavotiteUsers { get; set; }

    }
}
