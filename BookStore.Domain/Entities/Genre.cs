using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Entities
{
    public class Genre
    {
        public int GenreID { get; set; }
        public string GenreName { get; set; }
        public ICollection<Book> Books { get; set; }
        public Genre()
        {
            Books = new List<Book>();
        }

    }
}
