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
        public string Title { get; set; }
        //[HiddenInput(DisplayValue = false)]
        //public int Author_ID { get; set; }
        //[ForeignKey("Author_ID")]
        //public Author Author { get; set; }
        [Display(Name = "Рейтинг")]
        [HiddenInput(DisplayValue = true)]
        public double Rating { get; set; }
        [Display(Name = "Жанры")]
        //public virtual Genre Genre { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
        [Required]
        [Display(Name = "Цена")]
        [Range(0.00, double.MaxValue, ErrorMessage = "Please enter a positive price")]
        public decimal Price { get; set; }
        [Display(Name = "Описание")]
        // [HiddenInput(DisplayValue = true)]
        [DataType(DataType.MultilineText)]
        public string Annotation { get; set; }

        //[HiddenInput(DisplayValue = false)]
        public string Image_url { get; set; }
        [Display(Name = "Автор")]
        public virtual ICollection<Author> Authors { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Tag> Tages { get; set; }
        public virtual ICollection<Rate> RatedUsers { get; set; }
        public virtual ICollection<User> ReccomendedUsers { get; set; }
        public virtual ICollection<User> WishedUsers { get; set; }
    }
}
