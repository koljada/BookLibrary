using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BookStore.DO.Entities
{
    public class Author
    {
        [Key]
        [ScaffoldColumn(false)]
        public int Author_ID { get; set; }
        [Display(Name = "Фамилия Автора")]
        [Required]
        [MaxLength(20)]
        public string Last_Name { get; set; }
        [Display(Name = "Имя Автора")]
        [MaxLength(20)]
        public string First_Name { get; set; }
        [Display(Name = "Отчество Автора")]
        [MaxLength(20)]
        public string Middle_Name { get; set; }
        [Required]
        public virtual AuthorDetail AuthorDetail { get; set; }
        public virtual ICollection<Book> Books { get; set; }
        public Author()
        {
            Books = new List<Book>();
           // AuthorDetail=new AuthorDetail();
        }
    }

    public class AuthorDetail
    {
        
        [Key, ForeignKey("Author")]
        public virtual int Author_ID { get; set; }
         public string Image_url { get; set; }
        [Display(Name = "Биография")]
        [AllowHtml]
        [MaxLength(10000)]
        public string Biography { get; set; }
        
        public virtual ICollection<UserProfile> FavoriteUsers { get; set; }
        public virtual Author Author { get; set; }
        //[Display(Name = "Рейтинг")]
        //public double Rate { get; set; }
    }
}
