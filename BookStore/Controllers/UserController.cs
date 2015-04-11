using System;
using System.Collections.Generic;
using System.IO;
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

        public ActionResult Profile(string user)
        {
            return View(_userService.GetUserByEmail(user));
        }
        [Authorize(Roles = "user")]
        public void RateBook(int rate, int bookId)
        {
            int userId=_userService.GetUserByEmail(System.Web.HttpContext.Current.User.Identity.Name).User_ID;
           _userService.RateBook(rate,userId,bookId);
        }
        public ActionResult FileUpload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                string pic = "User"+Session["UserId"]+"."+Path.GetFileName(file.FileName).Split('.').Last();
                string path = System.IO.Path.Combine(Server.MapPath("~/Content/Images/User"), pic);
                // file is uploaded
                file.SaveAs(path);

                // save the image path path to the database or you can send image
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                    //using (MemoryStream ms = new MemoryStream())
                    //{
                    //    file.InputStream.CopyTo(ms);
                    //    byte[] array = ms.GetBuffer();
                    //}

            }
            // after successfully uploading redirect the user
            return RedirectToAction("List", "Book");
        }
    }
}