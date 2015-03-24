using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BookStore.Domain.Entities
{

    public class Book
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Book_ID { get; set; }

        public string Title { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int Author_ID { get; set; }
        [ForeignKey("Author_ID")]
        public  Author Author { get; set; }

        [HiddenInput(DisplayValue = true)]
        public double Rating { get; set; }

        public string Genre { get; set; }

        public decimal Price { get; set; }

       // [HiddenInput(DisplayValue = true)]
        [DataType(DataType.MultilineText)]
        public string Annotation { get; set; }

        //[HiddenInput(DisplayValue = false)]
        public string Image_url { get; set; }

        public virtual ICollection<Tag> Tages { get; set; }
        public virtual ICollection<Rate> RatedUsers { get; set; }
        public virtual ICollection<User> ReccomendedUsers { get; set; }
        public virtual ICollection<User> WishedUsers { get; set; }
       
    }
}
