using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Infrastructure;
using BookStore.Models;
using System.Web.Security;
using BookStore.DO.Entities;
using BookStore.DLL.Abstract;

namespace BookStore.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private IUserService _userService;
        private IRoleService _roleService;
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public AccountController(IUserService user_service, IRoleService role_service)
        {
            _userService = user_service;
            _roleService = role_service;
        }

        public ViewResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    Roles.IsUserInRole(model.UserName, "admin");
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    return Redirect(returnUrl ?? Url.Action("List", "Book"));
                }
                ModelState.AddModelError("", "Incorrect username or password");
            }
            return View();
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("List", "Book");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                MembershipUser membershipUser = ((CustomMembershipProvider)Membership.Provider).CreateUser(model.First_Name, model.Last_Name, model.Password, model.Email, model.Avatar_Url, model.Birthday, model.Sex);

                if (membershipUser != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, false);
                    return RedirectToAction("List", "Book");
                }
                ModelState.AddModelError("", "Ошибка при регистрации");
            }
            return View(model);
        }
        /* var userName = System.Web.HttpContext.Current.User.Identity.Name;
           var user = _userService.GetUserByEmail(userName);
           Session["UserId"] = user.User_ID;*/

        public ActionResult UserLogin()
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                if ((string)Session["UserName"] != ticket.Name)
                {
                    var user = _userService.GetUserByEmail(ticket.Name);
                    Session["UserId"] = user.User_ID;
                    Session["UserName"] = ticket.Name;
                    Session["UserImage"] = user.Avatar_Url;
                }
                return PartialView(Membership.GetUser(ticket.Name, false));
            }
            return PartialView(Membership.GetUser());
        }

        [HttpGet]
        public ActionResult Ajax()
        {
            return PartialView(new LoginViewModel());
        }

        [HttpPost]
        public ActionResult Ajax(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    Roles.IsUserInRole(model.UserName, "admin");
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    return View("_Ok");
                }
                ModelState.AddModelError("", "Incorrect username or password");
            }
            return View(model);
        }
    }

}
