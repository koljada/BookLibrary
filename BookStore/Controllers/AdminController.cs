using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.DO.Entities;
using BookStore.Models;
using BookStore.DLL.Abstract;
using System.Net;
using System.IO;
using System.Web.Hosting;
using NLog;

namespace BookStore.Controllers
{

    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AdminController()
        {

        }

        private readonly IBookService _bookService;
        private readonly IGenreService _genreService;
        private IAuthorService _authorService;

        public AdminController(IBookService repo, IGenreService genreService, IAuthorService authorService)
        {
            _bookService = repo;
            _genreService = genreService;
            _authorService = authorService;
        }

        public ViewResult Edit(int bookId)
        {
            Book book = _bookService.GetById(bookId);
            List<SelectListItem> genreList = new List<SelectListItem>();
            foreach (Genre genre in _genreService.Genres)
            {
                genreList.Add(new SelectListItem()
                {
                    Text = genre.Genre_Name,
                    Value = genre.Genre_Name
                });
            }
            ViewBag.Genres = genreList;
            return View(book);
        }

        public ViewResult Index()
        {
            return View(_bookService.GetAll());
        }

        public ViewResult Create()
        {
            return View(new Book { BookAuthors = new List<Author> { new Author() }, Genres = new List<Genre> { new Genre() } });
        }

        [HttpPost]
        public ActionResult FindBookImage(string title, string lastName, string firstName, int bookId)
        {
            string query =title + " " + lastName + " " + firstName ;
            logger.Info(query);
                IList < SearchResult > searchResults = SearchResult.GetSearch(query,"&searchType=image");
            ViewData["BookID"] = bookId;
            return PartialView(searchResults.Select(x => x.link));
        }
       
        [HttpPost]
        public string CopyImageToHost(string imageUrl, int bookId)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(imageUrl);
            httpWebRequest.AllowAutoRedirect = false;
            var ex = imageUrl.Split('.');
            var imgName = "imgBook" + bookId.ToString() + '.' + ex.Last();
            var dbUrl = "~/Content/Images/" + imgName;
            var svUrl = Server.MapPath("~/Content/Images/");
            var path = string.Format(svUrl + imgName);

            using (var fileStream = System.IO.File.Create(path))
            {
                using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (var stream = httpWebResponse.GetResponseStream())
                    {
                        stream.CopyTo(fileStream);
                    }
                }
            }
            return dbUrl;
        }

        [HttpPost]
        public ActionResult FindBookAnnotation(string title, string lastName, string firstName, int bookId)
        {
            string query = title + " " + lastName + " " + firstName + " " + "litres";
            logger.Info(query);
            List<string> links = SearchResult.GetSearch(query).Select(x => x.link).ToList();
            ViewData["BookID"] = bookId;
            return PartialView(SearchResult.GetInnerText(links));
        }

        [HttpPost]
        public ActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                _bookService.Create(book);
                logger.Info(book.Title+" created");
                TempData["message"] = string.Format("{0} has been saved", book.Title);
                return RedirectToAction("Edit", new { bookID = book.Book_ID });
            }
            return View(book);
        }

        [HttpPost]
        public ActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                _bookService.Save(book);
                logger.Info(book.Title + " edited");
                TempData["message"] = string.Format("{0} has been saved", book.Title);
                return RedirectToAction("Index");
            }
            return View(book);
        }

        [HttpPost]
        public ActionResult Delete(int bookId)
        {
            Book deletedBook = _bookService.Delete(bookId);

            if (deletedBook != null)
            {
                logger.Info(deletedBook.Title + " dletedted");
                TempData["message"] = string.Format("{0} was deleted", deletedBook.Title);
            }
            return RedirectToAction("Index");
        }
    }
}
