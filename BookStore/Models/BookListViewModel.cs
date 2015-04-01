using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStore.Domain.Entities;

namespace BookStore.Models
{
    public class BookListViewModel
    {
        public IEnumerable<Book> Books { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentGenre { get; set; }
        public string CurrentLetter { get; set; }
    }
}