using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.DAL.Abstract;
using BookStore.DO.Entities;
using BookStore.Models;
using System.Data.Entity;
using System.Net;
using BookStore.DLL.Abstract;

namespace BookStore.Controllers
{

   // [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        public AdminController()
        {

        }
        private IBookService bookService;
        private IGenreService genreService;
        private IAuthorService authorService;
        public AdminController(IBookService repo, IGenreService genre_service, IAuthorService author_service)
        {
            bookService = repo;
            genreService = genre_service;
            authorService = author_service;
        }
        public ViewResult Edit(int bookID, string image_url = null)
        {
            Book book = bookService.GetByID(bookID);
            if (image_url != null) book.Image_url = image_url;
            List<SelectListItem> GenreList = new List<SelectListItem>();
            foreach (Genre genre in genreService.Genres)
            {
                GenreList.Add(new SelectListItem()
                {
                    Text = genre.Genre_Name,
                    Value = genre.Genre_Name
                });
            }
            ViewBag.Genres = GenreList;
            return View(book);
        }
        public ViewResult Index()
        {
            return View(bookService.GetAll());
        }
        public ViewResult Create()
        {
            return View(new Book() { Authors = new List<Author>() { new Author() }, Genres = new List<Genre>() { new Genre()} });
        }
        [HttpPost]
        public ActionResult FindBookImage(string title, string last_name, string first_name, int book_ID)
        {
            IList<SearchResult> searchResults = SearchResult.getSearch(title + " " + last_name + " " + first_name, "&searchType=image");
            ViewData["BookID"] = book_ID;
            return PartialView(searchResults.Select(x => x.link));
        }
        //[HttpPost]
        public ActionResult SaveBookImage(string image_url, int bookID)
        {
            Book book = bookService.Books.FirstOrDefault(c => c.Book_ID == bookID);
            book.Image_url = image_url;
            bookService.Save(book);
            return RedirectToAction("Edit", new { bookID });
        }
        [HttpPost]
        public ActionResult SaveAnnotation(string annotation, int bookID)
        {
            Book book = bookService.Books.FirstOrDefault(c => c.Book_ID == bookID);
            book.Annotation = annotation;
            bookService.Save(book);
            return RedirectToAction("Edit", new { bookID });
        }
        [HttpPost]
        public ActionResult FindBookAnnotation(string title, string last_name, string first_name, int book_ID)
        {
            string query = title + " " + last_name + " " + first_name + " " + "litres";
            List<string> links = SearchResult.getSearch(query).Select(x => x.link).ToList();
            ViewData["BookID"] = book_ID;
            return PartialView(SearchResult.GetInnerText(links));
        }
        [HttpPost]
        public ActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                bookService.Create(book);
                //book.Authors = authors;     
                //.Save(book);
                TempData["message"] = string.Format("{0} has been saved", book.Title);
                return RedirectToAction("Edit", new { bookID = book.Book_ID });
            }
            else
            {
                // there is something wrong with the data values
                return View(book);
            }
        }
        [HttpPost]
        public ActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                //Genre genre = genreService.Genres.FirstOrDefault(g => g.Genre_Name == book.Genre.Genre_Name);
                //book.Genre = genre;
                bookService.Save(book);
                TempData["message"] = string.Format("{0} has been saved", book.Title);
                return RedirectToAction("Index");
            }
            else
            {
                // there is something wrong with the data values
                return View(book);
            }
        }

        [HttpPost]
        public ActionResult Delete(int bookId)
        {
            Book deletedBook = bookService.Delete(bookId);
            if (deletedBook != null)
            {
                TempData["message"] = string.Format("{0} was deleted", deletedBook.Title);
            }
            return RedirectToAction("Index");
        }


    }
}
