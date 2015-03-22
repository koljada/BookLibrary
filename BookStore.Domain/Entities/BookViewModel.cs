using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BookStore.Domain.Entities
{
    public class BookViewModel
    {
        public string Title { get; set; }

        public string AuthorName { get; set; }

        [HiddenInput(DisplayValue = true)]
        public double Rate { get; set; }

        public string Genre { get; set; }

        public decimal Price { get; set; }

        // [HiddenInput(DisplayValue = true)]
        [DataType(DataType.MultilineText)]
        public string Annotation { get; set; }

        //[HiddenInput(DisplayValue = false)]
        public string Image_url { get; set; }

        [HiddenInput(DisplayValue = false)]
        public ICollection<Genre> Genres { get; set; }
    }
}
