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
        [ScaffoldColumn(false)]
        public int Author_ID { get; set; }
        [Display(Name = "Фамилия")]
        public string Last_Name { get; set; }
        [Display(Name = "Имя")]
        public string First_Name { get; set; }
        [Display(Name = "Отчество")]
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
