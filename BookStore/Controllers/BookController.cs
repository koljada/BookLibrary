using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using BookStore.Models;
using System.Net;
using System.IO;
using System.Xml;
using System.Text;
using HtmlAgilityPack;

namespace BookStore.Controllers
{
    public class BookController : Controller
    {
        //
        // GET: /Book/

        private IBookRepository repository;
        public int PageSize = 2;
        public BookController(IBookRepository bookRepository)
        {
            this.repository = bookRepository;

        }
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public string Wiki()
        {
            string url = "https://uk.wikipedia.org/wiki/Джек_Лондон";   
            string content = getRequest(url);
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(content);
             HtmlNode c = doc.DocumentNode.SelectSingleNode("//a[@class='image']/img");
             return c.Attributes["src"].Value;
            
            
        }
        public string getRequest(string url)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.AllowAutoRedirect = false;//Запрещаем автоматический редирект
            httpWebRequest.Method = "GET"; //Можно не указывать, по умолчанию используется GET.
            httpWebRequest.Referer = "http://google.com"; // Реферер. Тут можно указать любой URL
            using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                using (var stream = httpWebResponse.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream, Encoding.GetEncoding(httpWebResponse.CharacterSet)))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }
        public ViewResult List(string genre, int page = 1)
        {
            BookListViewModel model = new BookListViewModel
            {
                Books = repository.Books
                .Where(p=>genre==null||p.Genre==genre)
                  .OrderBy(p => p.BookID)
                  .Skip((page - 1) * PageSize)
                  .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems =genre==null? repository.Books.Count():repository.Books.Where(e=>e.Genre==genre).Count()
                },
                CurrentGenre=genre
            };
            return View(model);
        
        }
    }
}
