using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using BookStore.DO.Entities;
using BookStore.Models;
using BookStore.DLL.Interface.Abstract;

namespace BookStore.Controllers
{
    public class BookController : Controller//TODO: BookDetails
    {
        private readonly IBookService _bookService;

        private readonly IAuthorService _authorService;
        private readonly IUserService _userService;
        // private IGenreService _genreService;
        private const int PageSize = 10;

        private readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public BookController(IBookService bookService, IAuthorService authorService, IUserService userService)
        {
            _bookService = bookService;
            _authorService = authorService;
            _userService = userService;
            //_genreService = genre_service;
        }

        public ViewResult BookDetails(int bookId)
        {
            Book book = _bookService.GetById(bookId);
            int userId = Session["UserId"] == null ? 0 : (int)Session["UserId"];
            var rated = book.RatedUsers.Where(x => x.IsSuggestion == false);
            List<CommentModel> commentModels = new List<CommentModel>();
            bool isRated = false;
            float average = !rated.Any() ? 0 : rated.Select(x => x.RateValue).Average();
            if (rated.Any() && userId != 0)
            {
                isRated = rated.Select(x => x.User_ID).Contains(userId) || book.WishedUsers.Select(x => x.User_ID).Contains(userId);
            }
            foreach (Comment com in book.Comments)
            {
                var user = _userService.GetById(com.User_ID);
                commentModels.Add(new CommentModel(user.Email, user.Avatar_Url, com.Context, com.DataCreate));
            }
            int[] ratesCount = new int[10];
            for (int i = 1; i <= 10; i++)
            {
                ratesCount[i - 1] = (rated.Select(x => x.RateValue).Count(x => x == (float)i));
            }
            BookViewModel bookView = new BookViewModel()
            {
                Book = book,
                RatedUsersCount = rated.Count(),
                AverageMark = average,
                WishedUsersCounter = book.WishedUsers.Count,
                IsReadedOrWished = isRated,
                Comments = commentModels
            };
            return View("BookSummary", bookView);
        }
        public JsonResult BarGraph(int bookId)
        {
            List<int> ratesCount = new List<int>();
            for (int i = 1; i <= 10; i++)
            {
                ratesCount.Add(_bookService.GetById(bookId).RatedUsers.Select(x => x.RateValue).Count(x => x == (float)i));
            }
            return Json(ratesCount, JsonRequestBehavior.AllowGet);
        }

        public ICollection<Book> PaginateBooks(IList<Book> books, int page)
        {
            return books.Skip((page - 1) * PageSize).Take(PageSize).ToList();
        }

        public ViewResult ListByTag(int tagId, int page = 1)
        {
            IList<Book> books = _bookService.GetBooksByTag(tagId);
            BookListViewModel model = new BookListViewModel
            {
                Books = PaginateBooks(books, page),
                PagingInfo = new PagingInfo(page, PageSize, books.Count()),
                CurrentTag = tagId
            };
            ViewBag.Action = "ListByTag";
            return View("List", model);
        }

        public ViewResult ListByLetter(string selectedLetter, int page = 1)
        {
            IList<Book> books = _bookService.GetBooksByLetter(selectedLetter);
            BookListViewModel model = new BookListViewModel
            {
                Books = PaginateBooks(books, page),
                PagingInfo = new PagingInfo(page, PageSize, books.Count()),
                CurrentLetter = selectedLetter
            };
            ViewBag.Action = "ListByLetter";
            return View("List", model);
        }
        public ViewResult ListByAuthor(string author, int page = 1)
        {
            IList<Book> books = _authorService.GetBooks(author);
            BookListViewModel model = new BookListViewModel
            {
                Books = PaginateBooks(books, page),
                PagingInfo = new PagingInfo(page, PageSize, books.Count()),
                CurrentAuthor = author
            };
            ViewBag.Action = "ListByAuthor";
            return View("List", model);
        }

        public ViewResult ListByGenre(string genre, int page = 1)
        {
            IList<Book> books = _bookService.GetBooksByGenre(genre);
            logger.Info(books);
            BookListViewModel model = new BookListViewModel
            {
                Books = PaginateBooks(books, page),
                PagingInfo = new PagingInfo(page, PageSize, books.Count()),
                CurrentGenre = genre
            };
            ViewBag.Action = "ListByGenre";
            return View("List", model);
        }

        public ViewResult List(int page = 1)
        {
            IList<Book> books = _bookService.GetAll();
            BookListViewModel model = new BookListViewModel
            {
                Books = PaginateBooks(books, page),
                PagingInfo = new PagingInfo(page, PageSize, books.Count()),
            };
            ViewBag.Action = "List";
            return View("List", model);
        }

        [Authorize(Roles = "user")]
        [HttpPost]
        public ActionResult AddComments(Comment comment)
        {
            _bookService.AddComment(comment);
            return RedirectToAction("BookDetails", new { bookId = _bookService.GetById(comment.Book_ID).Book_ID });
        }
        [HttpGet]
        public PartialViewResult AddComment(int bookId)
        {
            return PartialView("Comment", new Comment { Book_ID = bookId, User_ID = (int)@Session["userId"], DataCreate = DateTime.Now });
        }

        public PartialViewResult BookRating(int bookId)
        {
            Rate rate = _bookService.GetRate(bookId, (int)Session["UserId"]);
            rate = rate ?? new Rate { Book = _bookService.GetById(bookId), IsSuggestion = false };
            return PartialView("BookRating", rate);
        }

        [HttpPost]
        public JsonResult GetNames()//TODO: Create class for search in DLL
        {
            var t = _bookService.GetAll().Select(x => new { id = x.Book_ID, name = x.Title, type = 1 }).ToList();
            var a = _authorService.GetAll().Select(x => new { id = x.Author_ID, name = x.Last_Name, type = 2 }).ToList();
            t.AddRange(a);
            var p = Json(t, JsonRequestBehavior.AllowGet);
            return p;
        }

        public ViewResult Fb2Text(string path, int section = 0, int page = 1)//TODO: Separate and move to DLL
        {
            XDocument doc = XDocument.Load(Server.MapPath(path));
            StringBuilder text = new StringBuilder();
            int pageCharacters = 15000;
            List<string> chapters = new List<string>();
            if (doc.Root != null)
            {
                var body = doc.Root.Elements().FirstOrDefault(x => x.Name.LocalName == "body");
                if (body != null)
                {
                    var sections = body.Elements().Where(x => x.Name.LocalName == "section");
                    var xElements = sections as IList<XElement> ?? sections.ToList();
                    foreach (var chapter in xElements)
                    {
                        var name = chapter.Elements().FirstOrDefault(x => x.Name.LocalName == "title");
                        if (name != null)
                        {
                            chapters.Add(name.Element(body.GetDefaultNamespace() + "p").Value);
                        }
                    }
                    var main = xElements.Count() > 2 ? xElements.ElementAt(section) : body;
                    using (var xReader = main.CreateReader())
                    {
                        xReader.MoveToContent();
                        text.Append(xReader.ReadInnerXml());
                    }
                }
            }
            if (text.Length >= pageCharacters)
            {
                text.Append(' ', pageCharacters - text.Length%pageCharacters);
            }
            else
            {
                pageCharacters = text.Length;
            }
            TextViewModel model = new TextViewModel()
            {
                Text = text.ToString((page - 1) * pageCharacters, pageCharacters),
                Chapters = chapters,
                CurrentChapter = section,
                PagingInfo = new PagingInfo(page, pageCharacters, text.Length),
                CurrentPath = path
            };
            return View(model);
        }
    }
}