using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class AlphabeticalPagingViewModel
    {
        public List<string> BookNames { get; set; }
        public List<string> FirstLetters { get; set; }
        public string SelectedLetter { get; set; }

    }
}