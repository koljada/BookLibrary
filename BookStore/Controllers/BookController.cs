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

        public ViewResult BookDetails(int bookId)//TODO:Different by id
        {
            Book book = _bookService.GetById(bookId);
            int userId = Session["UserId"] == null ? 0 : (int)Session["UserId"];
            var rated = book.BookDetail.RatedUsers.Where(x => x.IsSuggestion == false);
            List<CommentModel> commentModels = new List<CommentModel>();
            bool isRated = false;
            float average = !rated.Any() ? 0 : rated.Select(x => x.RateValue).Average();
            if (rated.Any() && userId != 0)
            {
                isRated = rated.Select(x => x.User.User_ID).Contains(userId) || book.BookDetail.WishedUsers.Select(x => x.User_ID).Contains(userId);
            }
            foreach (Comment com in book.BookDetail.Comments)
            {
                var user = _userService.GetById(com.User.User_ID);
                commentModels.Add(new CommentModel(user.Email, user.Profile.Avatar_Url, com.Context, com.DataCreate));
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
                WishedUsersCounter = book.BookDetail.WishedUsers.Count,
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
                ratesCount.Add(_bookService.GetById(bookId).BookDetail.RatedUsers.Select(x => x.RateValue).Count(x => x == (float)i));
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
            IList<Book> books = _bookService.GetAllWithDetails();
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
            return RedirectToAction("BookDetails", new { bookId = _bookService.GetById(comment.Book.Book_ID).Book_ID });
        }
        [HttpGet]
        public PartialViewResult AddComment(int bookId)
        {
            return PartialView("Comment", new Comment { Book = _bookService.GetById(bookId).BookDetail, User=_userService.GetById((int)Session["UserId"]).Profile, DataCreate = DateTime.Now });
        }

        public PartialViewResult BookRating(int bookId)
        {
            Rate rate = _bookService.GetRate(bookId, (int)Session["UserId"]);
            rate = rate ?? new Rate { Book = _bookService.GetById(bookId).BookDetail, IsSuggestion = false };
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

        public ViewResult Fb2Text(string path, int section = 0, int page = 1)
        {
            Fb2Parser fb2 = new Fb2Parser(Server.MapPath(path),section);
            TextViewModel model = new TextViewModel()
            {
                Text = fb2.Text.ToString((page - 1) * fb2.PageCharacters, fb2.PageCharacters)+" - ",
                Chapters = fb2.Chapters,
                CurrentChapter = section,
                PagingInfo = new PagingInfo(page, fb2.PageCharacters, fb2.Text.Length),
                CurrentPath = path
            };
            return View(model);
        }
    }
}