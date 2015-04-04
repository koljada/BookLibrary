using BookStore.DLL.Abstract;
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
        public AlphabeticalPagingViewModel(string selectedLetter,IBookService bookService)
        {
            this.SelectedLetter = selectedLetter;
            this.FirstLetters = bookService.Books
                .GroupBy(p => p.Title.Substring(0, 1))
                .Select(x => x.Key.ToUpper())
                .ToList();
            if (string.IsNullOrEmpty(selectedLetter) || selectedLetter == "All")
            {
                this.BookNames = bookService.Books
                    .Select(p => p.Title)
                    .ToList();
            }
            else
            {
                if (selectedLetter == "0-9")
                {
                    var numbers = Enumerable.Range(0, 10).Select(i => i.ToString());
                    this.BookNames = bookService.Books
                        .Where(p => numbers.Contains(p.Title.Substring(0, 1)))
                        .Select(p => p.Title)
                        .ToList();
                }
                else
                {
                    this.BookNames = bookService.Books
                        .Where(p => p.Title.StartsWith(selectedLetter))
                        .Select(p => p.Title)
                        .ToList();
                }
            }
        }

    }
}