﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using BookStore.DAL.Abstract;
using BookStore.DO.Entities;
using BookStore.Models;
using BookStore.HtmlHelpers;
using BookStore.DLL.Abstract;

namespace BookStore.Controllers
{
    public class BookController : Controller//TODO: BookDetails
    {
        private readonly IBookService _bookService;

        private readonly IAuthorService _authorService;
        private readonly IUserService _userService;
        // private IGenreService _genreService;
        public int PageSize = 10;

        private readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public BookController(IBookService book_service, IAuthorService author_service, IUserService user_service)
        {
            _bookService = book_service;
            _authorService = author_service;
            _userService = user_service;
            //_genreService = genre_service;
        }

        public ViewResult BookDetails(int bookId)
        {
            return View(_bookService.GetById(bookId));
        }

        public ICollection<Book> PaginateBooks(List<Book> books, int page)
        {
            return books.Skip((page - 1) * PageSize).Take(PageSize).ToList();
        }

        public ViewResult ListByTag(int tagId, int page = 1)
        {
            List<Book> books = _bookService.GetBooksByTag(tagId).ToList();
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
            List<Book> books = _bookService.GetBooksByLetter(selectedLetter).ToList();
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
            List<Book> books = _authorService.GetBooks(author).ToList();
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
            List<Book> books = _bookService.GetBooksByGenre(genre).ToList();
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
            List<Book> books = _bookService.GetAll().ToList();
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
            // return PartialView("BookDetails", _bookService.GetById(comment.Book_ID));
            return RedirectToAction("BookDetails", new { bookId = _bookService.GetById(comment.Book_ID).Book_ID });
        }
        [HttpGet]
        public PartialViewResult AddComment(int bookId)
        {

            var com = new Comment { Book_ID = bookId, User_ID = (int)@Session["userId"], DataCreate = DateTime.Now };
            return PartialView("Comment", com);
        }

        public PartialViewResult BookRating(int bookId)
        {
            Rate rate = _bookService.GetRate(bookId, (int)Session["UserId"]);
            rate = rate ?? new Rate { Book = _bookService.GetById(bookId),IsSuggestion = false};
            return PartialView("BookRating", rate);
        }

        public PartialViewResult GetComment(ICollection<Comment> comments)
        {
            List<CommentModel> commentModels = new List<CommentModel>();
            foreach (Comment com in comments)
            {
                var user = _userService.GetById(com.User_ID);
                commentModels.Add(new CommentModel(user.Email,user.Avatar_Url,com.Context,com.DataCreate));
            }
            return PartialView(commentModels);
        }

        [HttpPost]
        public JsonResult GetNames()
        {
            var t = _bookService.GetAll().Select(x => new {id=x.Book_ID,name=x.Title,type=1}).ToList();
            var a = _authorService.GetAll().Select(x => new {id = x.Author_ID, name = x.Last_Name, type = 2}).ToList();
            t.AddRange(a);
            var p = Json(t, JsonRequestBehavior.AllowGet);
            return p ;
        }

        public ViewResult Fb2Text(string path,int section=0, int page = 1)
        {
            XDocument doc = XDocument.Load(Server.MapPath(path));
            StringBuilder text = new StringBuilder();
            int PageCharacters = 15000;
            List<string> chapters = new List<string>();
            var body = doc.Root.Elements().FirstOrDefault(x => x.Name.LocalName == "body");
            var sections = body.Elements().Where(x => x.Name.LocalName == "section");
            foreach (var chapter in sections)
            {
                var name = chapter.Elements().FirstOrDefault(x => x.Name.LocalName == "title");
                if (name!=null)
                {
                    
                    chapters.Add(name.Element(body.GetDefaultNamespace() + "p").Value);
                }
            }
            var main = sections.Count() > 2 ? sections.ElementAt(section) : body;
            using (var xReader = main.CreateReader())
            {
                xReader.MoveToContent();
                text.Append( xReader.ReadInnerXml());
            }
            if (text.Length<PageCharacters)
            {
                PageCharacters = text.Length;
            }
            else
            {
            text.Append(' ',PageCharacters-text.Length%PageCharacters);
            }
            TextViewModel model = new TextViewModel()
            {
                Text = text.ToString((page - 1) * PageCharacters, PageCharacters),
                Chapters=chapters,
                CurrentChapter=section,
                PagingInfo = new PagingInfo(page, PageCharacters, text.Length),
                CurrentPath = path
            };
            return View(model);
        }
    }
}