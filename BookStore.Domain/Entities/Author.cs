using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DO.Entities
{
    public class Author
    {
        public Author()
        {
            Books = new List<Book>();
        }

        [Key]
        [ScaffoldColumn(false)]
        public int Author_ID { get; set; }
        [Display(Name = "Фамилия Автора")]
        public string Last_Name { get; set; }
        [Display(Name = "Имя Автора")]
        public string First_Name { get; set; }
        [Display(Name = "Отчество Автора")]
        public string Middle_Name { get; set; }
        public string Image_Url { get; set; }
        [Display(Name = "Биография")]
        public string Biography { get; set; }
        [Display(Name = "Рейтинг")]
        public int Rating { get; set; }
        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<User> FavotiteUsers { get; set; }
    }
}
