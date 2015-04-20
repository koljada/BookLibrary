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

    public class Book
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        //[ScaffoldColumn(false)]
        public int Book_ID { get; set; }
        [Display(Name = "Название")]
        [MaxLength(50)]
        public string Title { get; set; }
        [Display(Name = "Рейтинг")]
        [HiddenInput(DisplayValue = true)]
        public double Rating { get; set; }
        [Display(Name = "Жанры")]
        public virtual ICollection<Genre> Genres { get; set; }
        [Required]
        [Display(Name = "Цена")]
        [Range(0.00, double.MaxValue, ErrorMessage = "Please enter a positive price")]
        public decimal Price { get; set; }
        [Display(Name = "Описание")]
        [DataType(DataType.MultilineText)]
        [MaxLength(5000)]
        public string Annotation { get; set; }
        [Display(Name = "Файл")]
        public string ContentUrl { get; set; }
        public string Image_url { get; set; }
        [Display(Name = "Автор")]
        [Required]
        public virtual ICollection<Author> BookAuthors { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        [Display(Name = "Теги")]
        public virtual ICollection<Tag> Tages { get; set; }
        public virtual ICollection<Rate> RatedUsers { get; set; }
        public virtual ICollection<User> ReccomendedUsers { get; set; }
        public virtual ICollection<User> WishedUsers { get; set; }

        public Book()
        {
            RatedUsers = new List<Rate>();
            Comments = new List<Comment>();
            WishedUsers=new List<User>();
        }
    }
}
