using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.DAL.Abstract;
using BookStore.DO.Entities;
using BookStore.Models;
using BookStore.HtmlHelpers;
using BookStore.DLL.Abstract;

namespace BookStore.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private IGenreService _genreService;
        public int PageSize = 4;
        public BookController(IBookService book_service, IGenreService genre_service)
        {
            this._bookService = book_service;
            this._genreService = genre_service;
        }
        public ViewResult ListByTag(int tagId, int page = 1)
        {
            ICollection<Book> books = _bookService.GetBooksByTag(tagId).ToList();
            BookListViewModel model = new BookListViewModel
            {
                Books = books.Skip((page - 1) * PageSize).Take(PageSize).ToList(),
                PagingInfo = new PagingInfo(page, PageSize, books.Count()),
                CurrentTag=tagId
            };
            ViewBag.Action = "ListByTag";
            
            return View("List", model);
        }
        public ViewResult ListByLetter(string selectedLetter, int page = 1)
        {
            ICollection<Book> books = _bookService.GetBooksByLetter(selectedLetter).ToList();
            BookListViewModel model = new BookListViewModel
            {
                Books = books.Skip((page - 1) * PageSize).Take(PageSize).ToList(),
                PagingInfo = new PagingInfo(page, PageSize, books.Count()),
                CurrentLetter = selectedLetter
            };
            ViewBag.Action = "ListByLetter";
            return View("List", model);
        }
        public ViewResult ListByGenre(string genre, int page = 1)
        {
            ICollection<Book> books = _bookService.GetBooksByGenre(genre).ToList();
            BookListViewModel model = new BookListViewModel
            {
                Books = books.Skip((page - 1) * PageSize).Take(PageSize).ToList(),
                PagingInfo = new PagingInfo(page, PageSize, books.Count()),
                CurrentGenre = genre
            };
            ViewBag.Action = "ListByGenre";
            return View("List", model);
        }
        public ViewResult List(int page = 1)
        {
            ICollection<Book> books = _bookService.GetAll().ToList();
            BookListViewModel model = new BookListViewModel
            {
                Books = books.Skip((page - 1) * PageSize).Take(PageSize).ToList(),
                PagingInfo = new PagingInfo(page, PageSize, books.Count()),
            };
            ViewBag.Action = "List";
            return View("List", model);
        }
    }
}
