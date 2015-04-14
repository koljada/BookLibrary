using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.DO.Entities;
using BookStore.Models;
using BookStore.DLL.Abstract;
using System.Net;
using System.IO;
using System.Web.Hosting;
using BookStore.DAL.Abstract;
using NLog;

namespace BookStore.Controllers
{

    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IBookRepository _bookRepository;
        private readonly IGenreService _genreService;
        private IAuthorRepository _authorRepository;

        public AdminController(IBookRepository repo, IGenreService genreService, IAuthorRepository authorRepository)
        {
            _bookRepository = repo;
            _genreService = genreService;
            _authorRepository = authorRepository;
        }

        public ViewResult Edit(int bookId)
        {
            Book book = _bookRepository.GetById(bookId);
            return View(book);
        }

        public ViewResult EditAuthor(int authorId)
        {
            Author auth = _authorRepository.GetById(authorId);
            return View(auth);
        }

        public ViewResult Index()
        {
            return View(_bookRepository.GetAll());
        }

        public ViewResult Create()
        {
            return View(new Book { BookAuthors = new List<Author> { new Author() }, Genres = new List<Genre> { new Genre() } });
        }

        [HttpPost]
        public ActionResult FindBookImage(string title, string lastName, string firstName, int bookId)
        {
            string query = title + " " + lastName + " " + firstName;
            logger.Info(query);
            IList<SearchResult> searchResults = SearchResult.GetSearch(query, "&searchType=image");
            ViewData["BookID"] = bookId;
            return PartialView(searchResults.Select(x => x.link));
        }

        [HttpPost]
        public string CopyImageToHost(string imageUrl, int bookId)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(imageUrl);
            httpWebRequest.AllowAutoRedirect = false;
            var ex = imageUrl.Split('.');
            var imgName = "imgBook" + bookId.ToString() + '.' + ex.Last();
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
            string query = title + " " + lastName + " " + firstName + " " + "readrate" + "litres";
            logger.Info(query);
            List<string> links = SearchResult.GetSearch(query).Select(x => x.link).ToList();
            ViewData["BookID"] = bookId;
            return PartialView(SearchResult.GetInnerText(links));
        }

        [HttpPost]
        public ActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                _bookRepository.Create(book);
                logger.Info(book.Title + " created");
                TempData["message"] = string.Format("{0} has been saved", book.Title);
                return RedirectToAction("Edit", new { bookID = book.Book_ID });
            }
            return View(book);
        }

        [HttpPost]
        public ActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                _bookRepository.Save(book);
                logger.Info(book.Title + " edited");
                TempData["message"] = string.Format("{0} has been saved", book.Title);
                return RedirectToAction("Index");
            }
            return View(book);
        }

        [HttpPost]
        public ActionResult Delete(int bookId)
        {
            Book deletedBook = _bookRepository.Delete(bookId);

            if (deletedBook != null)
            {
                logger.Info(deletedBook.Title + " dletedted");
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
                name = author +"_"+title+"."+ hpf.FileName.Split('.').Last();
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
                _authorRepository.Save(auth);
                logger.Info(auth.Last_Name + " " + auth.First_Name.Substring(0,1) + " edited");
                TempData["message"] = string.Format("{0} has been saved", auth.Last_Name + " " + auth.First_Name.Substring(0, 1) + ".");
                return RedirectToAction("Index");
            }
            return View(auth);
        }
    }
}
