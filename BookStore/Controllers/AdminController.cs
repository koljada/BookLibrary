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


using Google.Apis.Discovery;
using Google.Apis.Services;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


namespace BookStore.Controllers
{
    public class SearchResult
    {
        public string Title { get; set; }
        public string htmlTitle { get; set; }
        public string link { get; set; }
    }   
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
            //SearchResults result = Searcher.Search(SearchType.Image, Title + Author);
            var searchResults = getSearch(Title + Author, "&searchType=image");
            ViewData["BookID"] = repository.Books.FirstOrDefault(x => x.Title == Title).BookID;
            return PartialView(searchResults.Select(x=>x.link));
        }
        public ActionResult FindBookAnnotation(string Title, string Author,int bookID)
        {
            var searchResults = getSearch(Title+Author);
            var liveUrl = searchResults.FirstOrDefault(x => x.link.Contains("livelib"));

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(getRequest(liveUrl.link));
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
        public IList<SearchResult> getSearch(string searchText, string cfg="")
        {
            string key = "AIzaSyBzcXSZrtK15FFCX8v_Ob-Hcxnc-cVHc-Y";
            string cx = "015577388163479462430:16-o3xadmg4";
            string google = "https://www.googleapis.com/customsearch/v1?key=" + key + "&cx=" + cx + "&q=" + searchText + "&alt=json" + cfg;
            JObject googleSearch = JObject.Parse(getRequest(google));
            List<JToken> results = googleSearch["items"].Children().ToList();
            IList<SearchResult> searchResults = new List<SearchResult>();
            foreach (JToken result in results)
            {
                SearchResult searchResult = JsonConvert.DeserializeObject<SearchResult>(result.ToString());
                searchResults.Add(searchResult);
            }
            return searchResults;
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
