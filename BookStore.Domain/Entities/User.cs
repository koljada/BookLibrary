using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.DO.Entities
{
    public class User
    {
        [Key]
        public int User_ID { get; set; }
        public Role Role { get; set; }
        [Required]
        public string Email { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public DateTime Birthday { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
       
        public string  Avatar_Url { get; set; }
        public string Sex { get; set; }
        //public virtual ICollection<User> Friends { get; set; }
        public int Rating { get; set; }       
        public virtual ICollection<Author> FavoriteAuthors { get; set; }
        public virtual ICollection<Book> WishedBooks { get; set; }
        
        public virtual ICollection<Book> ReccomendedBooks { get; set; }
        public virtual ICollection<Rate> RatedBooks { get; set; }

    }
}
