using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.DLL.Abstract;
using BookStore.DO.Entities;

namespace BookStore.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IAuthorService _authorService;
        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }
        public ActionResult Index(int authorId)
        {
            Author author=_authorService.GetById(authorId);
            return View(author);
        }
    }
}