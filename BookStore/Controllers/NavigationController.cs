using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.DLL.Abstract;
using BookStore.Models;

namespace BookStore.Controllers
{
    public class NavController : Controller
    {
        private readonly IBookService bookService;
        private readonly IGenreService genreService;
        private readonly IAuthorService authorService;
        private readonly log4net.ILog logger =
           log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public NavController(IBookService book_service, IGenreService genre_service, IAuthorService author_service)
        {
            this.bookService = book_service;
            this.genreService = genre_service;
            authorService = author_service;
        }
        public ActionResult Alphabet(string selectedLetter = null)
        {
            AlphabeticalPagingViewModel model = new AlphabeticalPagingViewModel(selectedLetter, bookService);
            return PartialView(model);
        }

        public PartialViewResult Menu()
        {
           return PartialView();
        }
        public PartialViewResult Genres(string genre = null)
        {
            ViewBag.SelectedGenre = genre;
            IEnumerable<string> genres = genreService
                .GetAll()
                .Where(x=>x.Books.Count>0)
                .Select(x => x.Genre_Name);
            return PartialView(genres);
        }

        public PartialViewResult Authors(string author = null)
        {
            ViewBag.SelectedAuthor = author;
            IEnumerable<string> authors = authorService
                .GetAll()
                .Select(x => x.Last_Name);
            return PartialView(authors);
        }
    }
}
