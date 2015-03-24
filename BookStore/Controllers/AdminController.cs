using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using BookStore.Models;
using HtmlAgilityPack;
using Gapi;
using Gapi.Search;
using System.Data.Entity;
using System.Net;
using System.IO;
using System.Text;
using Google.Apis;
using GoogleSearchAPI.Query;
using GoogleSearchAPI.Resources;
using GoogleSearchAPI;
using Google.API.Search;

using Google.Apis.Discovery;
using Google.Apis.Services;
using System.Xml;
using System.Xml.Linq;


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
        public ViewResult Edit(int bookID,string image_url=null)
        {
            Book book = repository.Books.Include(x => x.Author)                
              .FirstOrDefault(p => p.Book_ID == bookID);
            if (image_url!=null) book.Image_url = image_url;
            return View(book);
        }

        public ViewResult AuthorsView(int authorID)
        {

            return View(repository.Authors.FirstOrDefault(x => x.Author_ID == authorID));
        }
        public ViewResult Index()
        {
            return View(repository.Books.Include(x => x.Author));
           // return View(repository.Books);
        }

        public ViewResult Create()
        {
            return View(new Book());
        }
        public ActionResult FindBookImage(string title, string last_name, string first_name,int book_ID)
        {
            SearchResults result = Searcher.Search(SearchType.Image, title.Trim()+last_name.Trim()+first_name.Trim());
            ViewData["BookID"] = book_ID;
            return PartialView(result);
        }
       
        public string getRequest(string url)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.AllowAutoRedirect = false;//Запрещаем автоматический редирект
            httpWebRequest.Method = "GET"; //Можно не указывать, по умолчанию используется GET.
            using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                using (var stream = httpWebResponse.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        [HttpPost]
        public ActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                Author author = repository.Authors.FirstOrDefault(x => x.Last_Name == book.Author.Last_Name&&x.First_Name==book.Author.First_Name);
                if (author == null)
                {
                    author = new Author() { Last_Name = book.Author.Last_Name,First_Name=book.Author.First_Name,Middle_Name=book.Author.Middle_Name };
                }
                book.Author = author;
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
