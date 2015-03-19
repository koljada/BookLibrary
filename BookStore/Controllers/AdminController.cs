using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;

namespace BookStore.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        private IBookRepository repository;
        public AdminController(IBookRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index()
        {
            return View(repository.Books);
        }

        public ViewResult Create()
        {

            return View();
        }

        public ViewResult Edit(int bookId)
        {
            Book book = repository.Books
              .FirstOrDefault(p => p.BookID == bookId);
            return View(book);
        }


    }
}
