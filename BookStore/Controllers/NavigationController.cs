using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Controllers;
using BookStore.DAL.Abstract;
using System.Data.Entity;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using BookStore.DLL.Abstract;
using BookStore.Models;

namespace BookStore.Controllers
{
    public class NavController : Controller
    {
        private readonly IBookService bookService;
        private readonly IGenreService genreService;
        private readonly IAuthorService authorService;
        private readonly log4net.ILog logger =
           log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public NavController(IBookService book_service, IGenreService genre_service, IAuthorService author_service)
        {
            this.bookService = book_service;
            this.genreService = genre_service;
            authorService = author_service;
        }
        public ActionResult Alphabet(string selectedLetter = null)
        {
            AlphabeticalPagingViewModel model = new AlphabeticalPagingViewModel(selectedLetter, bookService);
            return PartialView(model);
        }

        public PartialViewResult Menu()
        {
            //XmlTextReader reader = new XmlTextReader(Server.MapPath("~/Content/Books/Набоков_Катастрофа.fb2"));
            //reader.WhitespaceHandling = WhitespaceHandling.None;
            //StringBuilder str=new StringBuilder();
            //while (reader.Read())
            //{
            //    //if (reader.Name=="body")
            //    //{
            //        if (reader.NodeType==XmlNodeType.Text)
            //        {
            //           str.Append(reader.ReadContentAs(typeof(string),null));
            //        }
            //    //}
            //}
            //logger.Info(str);
            XDocument doc = XDocument.Load(Server.MapPath("~/Content/Books/Набоков_Катастрофа.fb2"));
            var nodes = doc.Root.Elements();
            var body = nodes.FirstOrDefault(x => x.Name.LocalName == "body");
            string html = null;
            using (var xReader = body.CreateReader())
            {
                xReader.MoveToContent();
                html = xReader.ReadInnerXml();
            }
           
            
            return PartialView();
        }
        public PartialViewResult Genres(string genre = null)
        {
            //ViewBag.SelectedGenre = genre == null ? null : genreService.Genres.FirstOrDefault(g => g.Genre_Name == genre).Genre_Name;
            ViewBag.SelectedGenre = genre;
            IEnumerable<string> genres = genreService.Genres.Where(c => c.Books.Any())
                .Select(x => x.Genre_Name);
            return PartialView(genres);
        }

        public PartialViewResult Authors(string author = null)
        {
            ViewBag.SelectedAuthor = author;
            IEnumerable<string> authors = authorService.GetAll().Select(x => x.Last_Name);
            return PartialView(authors);
        }
    }
}
