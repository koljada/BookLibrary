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
        private IBookService bookService;
        private IGenreService genreService;
        public NavController(IBookService book_service, IGenreService genre_service)
        {
            this.bookService = book_service;
            this.genreService = genre_service;
        }
        public ActionResult Alphabet(string selectedLetter = null)
        {
            AlphabeticalPagingViewModel model = new AlphabeticalPagingViewModel(selectedLetter, bookService);           
            return PartialView(model);
        }

        public PartialViewResult Menu(string genre = null)
        {
            ViewBag.SelectedGenre =genre==null?null:genreService.Genres.FirstOrDefault(g=>g.Genre_Name==genre).Genre_Name;
            IEnumerable<string> genres = genreService.Genres.Where(c => c.Books.Count() > 0)
                .Select(x => x.Genre_Name) ;
            return PartialView(genres);
        }

    }
}
