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
using System.Data.Entity;


namespace BookStore.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        public AdminController()
        {

        }
        private IBookRepository repository;
        private Book currentBook;
        public AdminController(IBookRepository repo)
        {
            repository = repo;
        }
        public ViewResult Edit(int bookId)
        {
            Book book = repository.Books.Include(x => x.Author)
              .FirstOrDefault(p => p.BookID == bookId);
            //BookViewModel bookForView = new BookViewModel()
            //{
            //    Title = book.Title,
            //    Annotation = book.Annotation,
            //    Rate = book.Rate,
            //    Price = book.Price,
            //    Genre = book.Genre,
            //    Image_url = book.Image_url,
            //    Genres = book.Genres,
            //    AuthorName = repository.Authors.FirstOrDefault(p => p.AuthorID == book.AuthorID).Name
            //};
            return View(book);
        }

        public ViewResult AuthorsView(int authorID)
        {

            return View(repository.Authors.FirstOrDefault(x => x.AuthorID == authorID));
        }
        public ViewResult Index()
        {
            return View(repository.Books.Include(x=>x.Author));
        }

        public ViewResult Create()
        {
            return View(new Book());
        }
        public ActionResult FindBookImage(string Title, string Author)
        {
            SearchResults result = Searcher.Search(SearchType.Image, Title + Author);
            ViewData["BookID"] = repository.Books.FirstOrDefault(x => x.Title == Title).BookID;
            return PartialView(result);
        }

        public ActionResult SaveBookImage(string imageUrl, int bookID)
        {
            Book bookForSave = repository.Books.FirstOrDefault(x => x.BookID == bookID);
            bookForSave.Image_url = imageUrl;
            repository.SaveBook(bookForSave);
            return RedirectToAction("Edit", new { bookID });
        }


        [HttpPost]
        public ActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                //Book bookForSave = new Book();
                Author author = repository.Authors.FirstOrDefault(x => x.Name == book.Author.Name);
                if (author == null)
                {
                    author = new Author() { Name = book.Author.Name };
                    repository.SaveAuthor(author);
                }
                //bookForSave.Author = author;
                //bookForSave.AuthorID = author.AuthorID;
                //bookForSave.Title = book.Title;
                repository.SaveBook(book);
                // author.Books.Add(bookForSave);
                TempData["message"] = string.Format("{0} has been saved", book.Title);
                return RedirectToAction("Edit", new { bookID = book.BookID });
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
                //Book bookForSave = repository.Books.FirstOrDefault(x => x.Title == book.Title);
                // bookForSave.Annotation = book.Annotation;
                ///bookForSave.Price = book.Price;
                //bookForSave.Genre = book.Genre;
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
