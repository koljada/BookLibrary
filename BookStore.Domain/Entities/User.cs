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
        //public Role Role { get; set; }
        [Required]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public virtual ICollection<Role> Roles { get; set; }

        public virtual UserProfile Profile { get; set; }

    }

    public class UserProfile
    {
        [Key, ForeignKey("User")]
        public virtual int User_ID { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public DateTime Birthday { get; set; }
        public string Avatar_Url { get; set; }
        public string Sex { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<AuthorDetail> FavoriteAuthors { get; set; }
        public virtual ICollection<BookDetail> WishedBooks { get; set; }

        public virtual ICollection<BookDetail> ReccomendedBooks { get; set; }
        public virtual ICollection<Rate> RatedBooks { get; set; }
        public virtual User User { get; set; }
        //public virtual ICollection<User> Friends { get; set; }
       // public int Rating { get; set; }
    }
}
