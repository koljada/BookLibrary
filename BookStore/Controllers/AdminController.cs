using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.DO.Entities;
using BookStore.Models;
using BookStore.DLL.Interface.Abstract;
using System.Net;
using System.IO;
using System.Web.Hosting;
using NLog;

namespace BookStore.Controllers
{

    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IBookService _bookService;
        private readonly IGenreService _genreService;
        private readonly IAuthorService _authorService;

        public AdminController(IBookService bookService, IGenreService genreService, IAuthorService authorService)
        {
            _bookService = bookService;
            _genreService = genreService;
            _authorService = authorService;
        }

        public ViewResult Edit(int bookId)
        {
            Book book = _bookService.GetById(bookId);
            return View(book);
        }

        public ViewResult EditAuthor(int authorId)
        {
            Author auth = _authorService.GetById(authorId);
            return View(auth);
        }

        public ViewResult Index()
        {
            return View(_bookService.GetAllWithDetails());
        }

        public ViewResult Create()
        {
            return View(new Book { BookAuthors = new List<Author> { new Author() },BookDetail = new BookDetail()});
        }

        [HttpPost]
        public ActionResult FindImage(string title, string lastName, string firstName, int Id)
        {
            string query;
            ViewData["ID"] = Id;
            if (title != null)
            {
                ViewData["Type"] = TypeSearch.BookCover;
                query = string.Format("{0} {1} {2}", title, lastName, firstName);
            }
            else
            {
                ViewData["Type"] = TypeSearch.AuthorPic;
                query = string.Format("author" + " {0} {1}", lastName, firstName);
            }

            logger.Info(query);
            IList<SearchResult> searchResults = SearchResult.GetSearch(query, "&searchType=image");
            
            return PartialView(searchResults.Select(x => x.Link));
        }

        [HttpPost]
        public string CopyImageToHost(string imageUrl, int id, string typesearch)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(imageUrl);
            httpWebRequest.AllowAutoRedirect = false;
            var ex = imageUrl.Split('.');
            string typePic;
            if (typesearch == TypeSearch.BookCover.ToString())
            {
                typePic = "imgBook";
            }
            else
            {
                typePic = "imgAuthor";
            }
            var imgName = string.Format("{0}{1}{2}{3}", typePic, id, '.', ex.Last());
            var dbUrl = "~/Content/Images/" + imgName;
            var svUrl = Server.MapPath("~/Content/Images/");
            var path = string.Format(svUrl + imgName);

            using (var fileStream = System.IO.File.Create(path))
            {
                using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (var stream = httpWebResponse.GetResponseStream())
                    {
                        stream.CopyTo(fileStream);
                    }
                }
            }
            return dbUrl;
        }
        [HttpPost]
        public ActionResult FindBookAnnotation(string title, string lastName, string firstName, int bookId)
        {
            string query = string.Format("{0} {1} {2}", title, lastName, firstName);
            logger.Info(query);
            List<string> links = SearchResult.GetSearch(query).Select(x => x.Link).ToList();
            ViewData["BookID"] = bookId;
            return PartialView(SearchResult.GetInnerText(links));
        }

        [HttpPost]
        public ActionResult Create(Book book)
        {
            //if (ModelState.IsValid)
            //{
                _bookService.Create(book);
                logger.Info(book.Title + " created");
                TempData["message"] = string.Format("{0} has been saved", book.Title);
                return RedirectToAction("Edit", new { bookID = book.Book_ID });
            //}
           // return View(book);
        }

        [HttpPost]
        public ActionResult Edit(Book book)
        {
            //if (ModelState.IsValid)
            //{
                _bookService.Save(book);
                logger.Info(book.Title + " edited");
                TempData["message"] = string.Format("{0} has been saved", book.Title);
                return RedirectToAction("Index");
            //}
            return View(book);
        }

        [HttpPost]
        public ActionResult Delete(int bookId)
        {
            Book deletedBook = _bookService.Delete(bookId);

            if (deletedBook != null)
            {
                logger.Info(deletedBook.Title + " deleted");
                TempData["message"] = string.Format("{0} was deleted", deletedBook.Title);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public string UploadFiles()
        {
            string name=null;
            string author = Request.Params[0];
            string title = Request.Params[1].Replace(" ", "_");
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
               // name=Request.Files[file]
                if (hpf.ContentLength == 0)
                    continue;
                name = string.Format("{0}_{1}.{2}", author, title, hpf.FileName.Split('.').Last());
                string savedFileName = Path.Combine(Server.MapPath("~/Content/Books"), name);
                hpf.SaveAs(savedFileName);
            }
            return "~/Content/Books/"+name ;
        }

        [HttpPost]
        public ActionResult EditAuthor(Author auth)
        {
            if (ModelState.IsValid)
            {
                _authorService.Save(auth);
                logger.Info(auth.Last_Name + " " + " edited");
                TempData["message"] = string.Format("{0} has been saved", auth.Last_Name + " ");
                return RedirectToAction("Index");
            }
            return View(auth);
        }
        [HttpGet]
        public ActionResult DeleteAuthor(int authId)
        {
            Author deletedAuth = _authorService.Delete(authId);

            if (deletedAuth != null)
            {
                logger.Info(deletedAuth.Last_Name + " dletedted");
                TempData["message"] = string.Format("{0} was deleted", deletedAuth.Last_Name);
            }
            return RedirectToAction("Index");
        }
    }
}
