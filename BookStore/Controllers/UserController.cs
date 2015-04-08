using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.DLL.Abstract;

namespace BookStore.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService service )
        {
            _userService = service;
        }
 

        public ActionResult Index(string user)
        {
            return View(_userService.GetUserByEmail(user));
        }
    }
}