using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using HtmlAgilityPack;
using Gapi;
using Gapi.Search;


namespace BookStore.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        private IBookRepository repository;
        public AdminController(IBookRepository repo)
        {
            repository = repo;
        }
        public ViewResult Edit(int bookId)
        {
            Book product = repository.Books
              .FirstOrDefault(p => p.BookID == bookId);
            return View(product);
        }


        public ViewResult Index()
        {
            return View(repository.Books);
        }

        public ViewResult Create()
        {

            return View(new Book());
        }
        [HttpPost]
        public ActionResult FindBookImages(Book book)
        {
            IEnumerable<Author> author = repository.Authors.Where(p => p.Name == book.Author.Name);//should me somewhere in model
            if (author != null)
            {
                book.Author = author.First();
            }
            repository.SaveBook(book);
            SearchResults searchResults = Searcher.Search(SearchType.Image, book.Title + book.Author.Name);
            return PartialView(searchResults);
        }

        [HttpPost]
        public ActionResult Create(Book book)
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
