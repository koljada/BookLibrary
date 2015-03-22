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
            Book book = repository.Books
              .FirstOrDefault(p => p.BookID == bookId);
            BookViewModel bookForView = new BookViewModel()
            {
                Title = book.Title,
                Annotation = book.Annotation,
                Rate = book.Rate,
                Price = book.Price,
                Genre = book.Genre,
                Image_url = book.Image_url,
                Genres = book.Genres,
                AuthorName = repository.Authors.FirstOrDefault(p => p.AuthorID == book.AuthorID).Name
            };
            return View(bookForView);
        }


        public ViewResult Index()
        {
            return View(repository.Books);
        }

        public ViewResult Create()
        {
            return View(new BookViewModel());
        }
        public ActionResult FindBookImage(string Title, string Author)
        {
            SearchResults result = Searcher.Search(SearchType.Image, Title + Author);

            int id = repository.Books.FirstOrDefault(x => x.Title == Title).BookID;
            ViewData["BookID"] = id;
            return PartialView(result);
        }

        public ActionResult SaveImage(string imageUrl, int bookID)
        {
            Book bookForSave = repository.Books.FirstOrDefault(x => x.BookID == bookID);
            bookForSave.Image_url = imageUrl;
            repository.SaveBook(bookForSave);

            return RedirectToAction("Edit", new { bookID });

        }


        [HttpPost]
        public ActionResult Create(BookViewModel book)
        {
            if (ModelState.IsValid)
            {
                Book bookForSave = new Book();
                Author author = repository.Authors.FirstOrDefault(x => x.Name == book.AuthorName);//should me somewhere in model
                if (author == null)
                {
                    author = new Author() { Name = book.AuthorName };
                    repository.SaveAuthor(author);
                }
                bookForSave.Author = author;
                bookForSave.AuthorID = author.AuthorID;
                bookForSave.Title = book.Title;
                currentBook = bookForSave;
                repository.SaveBook(bookForSave);
                TempData["message"] = string.Format("{0} has been saved", book.Title);
                return RedirectToAction("Edit", new { bookID = bookForSave.BookID });
            }
            else
            {
                // there is something wrong with the data values
                return View(book);
            }
        }

        [HttpPost]
        public ActionResult Edit(BookViewModel book)
        {
            if (ModelState.IsValid)
            {
                Book bookForSave = repository.Books.FirstOrDefault(x => x.Title == book.Title);
                bookForSave.Annotation = book.Annotation;
                bookForSave.Price = book.Price;
                bookForSave.Genre = book.Genre;
                repository.SaveBook(bookForSave);
                TempData["message"] = string.Format("{0} has been saved", bookForSave.Title);
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
