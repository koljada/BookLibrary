using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using BookStore.Models;
using System.Data.Entity;
using System.Net;

namespace BookStore.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        public AdminController()
        {

        }
        private IBookRepository repository;
        public AdminController(IBookRepository repo)
        {
            repository = repo;
        }
        public ViewResult Edit(int bookID, string image_url = null)
        {
            Book book = repository.Books.Include(x => x.Author).Include(x => x.Tages)
              .FirstOrDefault(p => p.Book_ID == bookID);
            if (image_url != null) book.Image_url = image_url;
            return View(book);
        }

        public ViewResult AuthorsView(int authorID)
        {
            return View(repository.Authors.FirstOrDefault(x => x.Author_ID == authorID));
        }
        public ViewResult Index()
        {
            return View(repository.Books.Include(x => x.Author).Include(x => x.Tages));
            // return View(repository.Books);
        }

        public ViewResult Create()
        {
            return View(new Book());
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
            Book book = repository.Books.FirstOrDefault(c => c.Book_ID == bookID);
            book.Image_url = image_url;
            repository.SaveBook(book);
            return RedirectToAction("Edit", new { bookID });
        }
        public ActionResult FindBookAnnotation(string title, string last_name, string first_name, int bookID)
        {
            string query = title + " " + last_name + " " + first_name;
            string liveUrl = SearchResult.getSearch(query).FirstOrDefault(x => x.link.Contains("livelib")).link;
            Book book = repository.Books.FirstOrDefault(x => x.Book_ID == bookID);
            book.Annotation = HttpUtility.HtmlDecode(SearchResult.GetInnerText(liveUrl));
            repository.SaveBook(book);
            return RedirectToAction("Edit", new { bookID });
        }

        [HttpPost]
        public ActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                Author author = repository.Authors.FirstOrDefault(x => x.Last_Name == book.Author.Last_Name && x.First_Name == book.Author.First_Name);
                if (author == null)
                {
                    author = new Author() { Last_Name = book.Author.Last_Name, First_Name = book.Author.First_Name, Middle_Name = book.Author.Middle_Name };
                }
               
                book.Author = author;
                ICollection<Tag> list = repository.GetTags(book);
                book.Tages=list;               
               
                repository.SaveBook(book);
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
                repository.SaveBook(book);
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
            Book deletedBook = repository.DeleteBook(bookId);
            if (deletedBook != null)
            {
                TempData["message"] = string.Format("{0} was deleted", deletedBook.Title);
            }
            return RedirectToAction("Index");
        }


    }
}
