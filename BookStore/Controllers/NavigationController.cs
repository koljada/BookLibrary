using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Controllers;
using BookStore.DAL.Abstract;
using System.Data.Entity;
using BookStore.DLL.Abstract;
using BookStore.Models;

namespace BookStore.Controllers
{
    public class NavController : Controller
    {
        private readonly IBookService bookService;
        private readonly IGenreService genreService;
        private readonly IAuthorService authorService;

        public NavController(IBookService book_service, IGenreService genre_service,IAuthorService author_service)
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
            //ViewBag.SelectedGenre = genre == null ? null : genreService.Genres.FirstOrDefault(g => g.Genre_Name == genre).Genre_Name;
            ViewBag.SelectedGenre = genre;
            IEnumerable<string> genres = genreService.Genres.Where(c => c.Books.Any())
                .Select(x => x.Genre_Name);
            return PartialView(genres);
        }

        public PartialViewResult Authors(string author=null)
        {
            ViewBag.SelectedAuthor = author;
            IEnumerable<string> authors = authorService.GetAll().Select(x => x.Last_Name);
            return PartialView(authors);
        }

    }
}
