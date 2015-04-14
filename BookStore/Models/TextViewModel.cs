using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class TextViewModel
    {
        public string Text { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public int CurrentChapter { get; set; }
        public IList<string> Chapters { get; set; }
        public string CurrentPath { get; set; }
    }
}