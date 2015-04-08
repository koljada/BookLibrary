﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
        public BookController(IBookService book_service, IGenreService genre_service)
        {
            _bookService = book_service;
            _genreService = genre_service;
        }

        public ViewResult BookDetails(int bookId)
        {
            return View(_bookService.GetById(bookId)); 
        }

        public ICollection<Book> PaginateBooks(IQueryable<Book> books,int page)
        {
            return books.Skip((page - 1)*PageSize).Take(PageSize).ToList();
        } 
        public ViewResult ListByTag(int tagId, int page = 1)
        {
            IQueryable<Book> books = _bookService.GetBooksByTag(tagId);
            BookListViewModel model = new BookListViewModel
            {
                Books = PaginateBooks(books,page),
                PagingInfo = new PagingInfo(page, PageSize, books.Count()),
                CurrentTag=tagId
            };
            ViewBag.Action = "ListByTag";
            
            return View("List", model);
        }
        public ViewResult ListByLetter(string selectedLetter, int page = 1)
        {
            IQueryable<Book> books = _bookService.GetBooksByLetter(selectedLetter);
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
            IQueryable<Book> books = _bookService.GetBooksByGenre(genre);
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
            IQueryable<Book> books = _bookService.GetAll();
            BookListViewModel model = new BookListViewModel
            {
                Books = PaginateBooks(books, page),
                PagingInfo = new PagingInfo(page, PageSize, books.Count()),
            };
            ViewBag.Action = "List";
            return View("List", model);
        }
    }
}
