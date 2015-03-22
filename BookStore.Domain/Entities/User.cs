using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Domain.Entities
{
    public class User
    {
        public int UserID { get; set; }
        [Required]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
       
        public string  AvatarUrl { get; set; }
        public string Sex { get; set; }
        //public virtual ICollection<User> Friends { get; set; }
        public int Rating { get; set; }
        public virtual ICollection<Author> FavoriteAuthors { get; set; }
        public virtual ICollection<Book> WishedBooks { get; set; }
        
        public virtual ICollection<Book> ReccomendedBooks { get; set; }
        public virtual ICollection<Rate> RatedBooks { get; set; }

    }
}
