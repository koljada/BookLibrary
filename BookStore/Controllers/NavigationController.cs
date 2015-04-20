using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.DLL.Abstract;
using BookStore.DO.Entities;
using BookStore.Models;

namespace BookStore.Controllers
{
    public class NavController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IGenreService _genreService;
        private readonly IAuthorService _authorService;
        private readonly log4net.ILog logger =
           log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public NavController(IBookService bookService, IGenreService genreService, IAuthorService authorService)
        {
            this._bookService = bookService;
            this._genreService = genreService;
            _authorService = authorService;
        }
        public ActionResult Alphabet(string selectedLetter = null)
        {
            AlphabeticalPagingViewModel model = new AlphabeticalPagingViewModel(selectedLetter, _bookService);
            return PartialView(model);
        }

        public PartialViewResult Menu()
        {
            return PartialView();
        }
        public PartialViewResult Genres(string genre = null)
        {
            IEnumerable<Genre> genres = new List<Genre>();
            if (genre != null)
            {
                Genre currentGenre = _genreService.GetAll().FirstOrDefault(x => x.Genre_Name == genre);
                ViewBag.SelectedGenre = genre;
                genres = _genreService.GetAll().Where(x => x.ParentID == currentGenre.Genre_ID);
                if (genres.Any())
                {
                    ViewBag.ParentName = currentGenre.Genre_Name;
                    return PartialView("NodeGenre",genres);
                }
                else
                {
                    return PartialView("LeafGenre",currentGenre);
                }
            }

            return PartialView(genres);
        }

        public PartialViewResult Authors(string author = null)
        {
            ViewBag.SelectedAuthor = author;
            IEnumerable<string> authors = _authorService
                .GetAll()
                .Select(x => x.Last_Name);
            return PartialView(authors);
        }
    }
}
