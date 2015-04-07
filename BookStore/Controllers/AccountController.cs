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
            return View();//model
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

        public ActionResult UserLogin()
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {                
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                return PartialView(Membership.GetUser(ticket.Name, false));
            }
            return PartialView(Membership.GetUser());
        }
    }

}
