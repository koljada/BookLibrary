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
using System.Data.Entity;
using System.Text.RegularExpressions;
using BookStore.HtmlHelpers;

namespace BookStore.Controllers
{
    public class BookController : Controller
    {
        //
        // GET: /Book/
        public BookController()
        {

        }

        private IBookRepository repository;
        public int PageSize = 4;
        AlphabeticalPagingViewModel model = new AlphabeticalPagingViewModel();
        public BookController(IBookRepository bookRepository)
        {
            this.repository = bookRepository;

        }
        public ActionResult Alphabet(string selectedLetter = null)
        {
            model.SelectedLetter = selectedLetter;
            model.FirstLetters = repository.Books
                .GroupBy(p => p.Title.Substring(0, 1))
                .Select(x => x.Key.ToUpper())
                .ToList();

            if (string.IsNullOrEmpty(selectedLetter) || selectedLetter == "All")
            {
                model.BookNames = repository.Books
                    .Select(p => p.Title)
                    .ToList();
            }
            else
            {
                if (selectedLetter == "0-9")
                {
                    var numbers = Enumerable.Range(0, 10).Select(i => i.ToString());
                    model.BookNames = repository.Books
                        .Where(p => numbers.Contains(p.Title.Substring(0, 1)))
                        .Select(p => p.Title)
                        .ToList();
                }
                else
                {
                    model.BookNames = repository.Books
                        .Where(p => p.Title.StartsWith(selectedLetter))
                        .Select(p => p.Title)
                        .ToList();
                }
            }

            return PartialView(model);
        }
       
        public ViewResult ListByLetter(string selectedLetter, int page = 1)
        {
            var num=Enumerable.Range(0, 10).Select(i => i.ToString());
            BookListViewModel model = new BookListViewModel
            {
                Books = repository.Books.Include(b => b.Author)
                .Where(p => selectedLetter == "All" || p.Title.StartsWith(selectedLetter) ||( num.Contains(p.Title.Substring(0,1))&&selectedLetter=="0-9") )
                  .OrderBy(p => p.Book_ID)
                  .Skip((page - 1) * PageSize)
                  .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = selectedLetter == "All" ? repository.Books.Count() : repository.Books.Where(e => e.Title.StartsWith(selectedLetter)).Count()
                },
                CurrentLetter = selectedLetter
            };
            return View(model);

        }
        public ViewResult List(string genre, int page = 1)
        {
            BookListViewModel model = new BookListViewModel
            {
                Books = repository.Books.Include(b => b.Author)
                .Where(p => genre == null || p.Genre == genre)
                  .OrderBy(p => p.Book_ID)
                  .Skip((page - 1) * PageSize)
                  .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = genre == null ? repository.Books.Count() : repository.Books.Where(e => e.Genre == genre).Count()
                },
                CurrentGenre = genre
            };
            return View(model);

        }
    }
}
