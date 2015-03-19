using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BookStore.Domain.Entities
{
    public class Genre{
        public int GenreID { get; set; }
        public string Genre1{get;set;}

    }
    public class Book
    {
        [HiddenInput(DisplayValue = false)]
        public int BookID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
         [HiddenInput(DisplayValue = false)]
        public double Rate { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
        [DataType(DataType.MultilineText)]
        public string Annotation { get; set; }
        public string Image_url { get; set; }
        public ICollection<Genre> genres { get; set; }



    }
}
