﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
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
        public int PageSize = 10;

        private readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public BookController(IBookService book_service, IGenreService genre_service)
        {
            _bookService = book_service;
            _genreService = genre_service;
            // var id =  Membership.GetUser().;

        }

        public ViewResult BookDetails(int bookId)
        {
            return View(_bookService.GetById(bookId));
        }

        public ICollection<Book> PaginateBooks(List<Book> books, int page)
        {
            return books.Skip((page - 1)*PageSize).Take(PageSize).ToList();
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

        [HttpPost]
        public PartialViewResult AddComments(Comment comment)
        {
            
            _bookService.AddComment(comment);

            return PartialView("BookDetails", _bookService.GetById(comment.Book_ID));
        }
    }
}