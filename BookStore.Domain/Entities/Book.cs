using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Entities
{
    public class Genre{
        public int GenreID { get; set; }
        public string Genre1{get;set;}

    }
    public class Book
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public double Rate { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
        public string Annotation { get; set; }
        public string Image_url { get; set; }
        public ICollection<Genre> genres { get; set; }



    }
}
