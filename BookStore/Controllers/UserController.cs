using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BookStore.DLL.Abstract;
using BookStore.DO.Entities;

namespace BookStore.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IBookService _bookService;


        public UserController(IUserService userService,IBookService bookService )
        {
            _userService = userService;
            _bookService = bookService;
        }
 

        public ActionResult Index(string user)
        {
            return View(_userService.GetUserByEmail(user));
        }
        [Authorize(Roles = "user")]
        public void RateBook(int rate, int bookId)
        {
            int userId=_userService.GetUserByEmail(System.Web.HttpContext.Current.User.Identity.Name).User_ID;
           _userService.RateBook(rate,userId,bookId);
        }
    }
}