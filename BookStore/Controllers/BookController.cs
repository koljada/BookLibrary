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
        public ViewResult ListByTag(int tagID, int page = 1)
        {
            BookListViewModel model = new BookListViewModel
            {
                Books = repository.Books
                        .Where(b => b.Tages.Any(t => t.Tag_ID == tagID))
                        .Include(a => a.Author)
                        .Include(t => t.Tages)
                        .Include(b => b.Genre)
                        .OrderBy(p => p.Book_ID)
                        .Skip((page - 1) * PageSize)
                        .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = repository.Tags.FirstOrDefault(t => t.Tag_ID == tagID).Books.Count()
                },
            };
            ViewBag.Action = "ListByTag";
            return View("List", model);
        }

        public ViewResult ListByLetter(string selectedLetter, int page = 1)
        {
            var num = Enumerable.Range(0, 10).Select(i => i.ToString());
            BookListViewModel model = new BookListViewModel
            {
                Books = repository.Books.Include(b => b.Author).Include(b => b.Tages).Include(b => b.Genre)
                .Where(p => selectedLetter == "All" || p.Title.StartsWith(selectedLetter) || (num.Contains(p.Title.Substring(0, 1)) && selectedLetter == "0-9"))
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
            ViewBag.Action = "ListByLetter";
            return View("List", model);

        }
        public ViewResult List(string genre, int page = 1)
        {
            BookListViewModel model = new BookListViewModel
            {
                Books = repository.Books.Include(b => b.Author).Include(T => T.Tages).Include(b=>b.Genre)
                  .Where(p => genre == null || p.Genre.Genre_Name== genre)
                  .OrderBy(p => p.Book_ID)
                  .Skip((page - 1) * PageSize)
                  .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = genre == null ? repository.Books.Count() : repository.Books.Where(e => e.Genre.Genre_Name == genre).Count()
                },
                CurrentGenre = genre
            };
            ViewBag.Action = "List";
            return View("List", model);

        }
    }
}
