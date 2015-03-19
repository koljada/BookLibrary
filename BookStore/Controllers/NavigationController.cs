using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Controllers;
using BookStore.Domain.Abstract;

namespace BookStore.Controllers
{
    public class NavController : Controller
    {
        //
        // GET: /Default1/
        private IBookRepository repository;
        public NavController(IBookRepository repo){
            repository = repo;
        }
        public PartialViewResult Menu(string genre=null)
        {
            ViewBag.SelectedGenre = genre;
            IEnumerable<string> genres = repository.Books
                .Select(x => x.Genre)
                .Distinct()
                .OrderBy(x => x);
            return PartialView(genres);
        }

    }
}
