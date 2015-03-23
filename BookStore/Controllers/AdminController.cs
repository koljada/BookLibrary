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
        public ViewResult Edit(int bookId)
        {
            Book book = repository.Books.Include(x => x.Author)
              .FirstOrDefault(p => p.BookID == bookId);
            return View(book);
        }

        public ViewResult AuthorsView(int authorID)
        {

            return View(repository.Authors.FirstOrDefault(x => x.AuthorID == authorID));
        }
        public ViewResult Index()
        {
            return View(repository.Books.Include(x => x.Author));
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

        public ActionResult FindBookAnnotation(string Title, string Author,int bookID)
        {
           // user=koljadar&key=03.310576775:d008fbd56ba762a577119ddb1524a8e1
            


           /// GwebSearchClient client = new GwebSearchClient("http://www.yandex.ua");
            //IList<IWebResult> results = client.Search(Title + Author + "livelib.ru/book", 32);
            

            //WebQuery query = new WebQuery(Title+Author+"livelib.ru/book");
            //query.StartIndex.Value = 1;
            //query.HostLangauge.Value = Languages.Russian;
            //IGoogleResultSet<GoogleWebResult> resultSet = GoogleService.Instance.Search<GoogleWebResult>(query);
            //string url = resultSet.Results.First().Url;

            //SearchResults result = Searcher.Search(SearchType.Web, Title + Author +"livelib" );//TODO
            //string url = result.Items.First().Url;
           // string url = results.First().Url;
            string findedUrl = YandexSearch.Search(Title + Author + "livelib.ru/book").First().DisplayUrl;
            string content = getRequest(findedUrl);
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(content);
            HtmlNode c = doc.DocumentNode.SelectSingleNode("//p[@itemprop='about']");
            if (c != null)
            {
                Book book = repository.Books.FirstOrDefault(x => x.BookID == bookID);
                book.Annotation = c.InnerText;
                repository.SaveBook(book);
            }
            return RedirectToAction("Edit", new { bookID });

        }
        public string getRequest(string url)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.AllowAutoRedirect = false;//Запрещаем автоматический редирект
            httpWebRequest.Method = "GET"; //Можно не указывать, по умолчанию используется GET.
            //httpWebRequest.Referer = "http://google.com"; // Реферер. Тут можно указать любой URL
            //httpWebRequest.ContentType=
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
