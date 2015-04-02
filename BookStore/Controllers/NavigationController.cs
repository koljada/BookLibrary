using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Controllers;
using BookStore.Domain.Abstract;
using System.Data.Entity;

namespace BookStore.Controllers
{
    public class NavController : Controller
    {
        public NavController()
        {

        }
        //
        // GET: /Default1/
        private IBookRepository repository;
        public NavController(IBookRepository repo){
            repository = repo;
        }
        public PartialViewResult Menu(string genre = null)
        {
            ViewBag.SelectedGenre =genre==null?null:repository.Genres.FirstOrDefault(g=>g.Genre_Name==genre).Genre_Name;
            IEnumerable<string> genres = repository.Genres.Where(c=>c.Books.Count()>0 )
                .Select(x => x.Genre_Name) ;
            return PartialView(genres);
        }

    }
}
