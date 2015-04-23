using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BookStore.DLL.Interface.Abstract;
using BookStore.DO.Entities;
using NLog;

using System.Threading.Tasks;

namespace BookStore.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IBookService _bookService;
        private readonly IAuthorService _authorService;

        private readonly log4net.ILog logger =
           log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public UserController(IUserService userService, IBookService bookService, IAuthorService authorService)
        {
            _userService = userService;
            _bookService = bookService;
            _authorService = authorService;
        }

        public ActionResult Profile(string user)
        {
            return View(_userService.GetUserByEmail(user));
        }
        [Authorize(Roles = "user")]
        public async Task RateBook(float rate, int bookId, bool isSuggestion)
        {
            int userId = (int)Session["UserId"];
            await _userService.RateBook(rate, userId, bookId, false);
        }

        public int WishBook(int bookId)
        {
            _userService.WishBook(bookId, (int)Session["UserId"]);
            return _bookService.GetById(bookId).BookDetail.WishedUsers.Count;
        }
        [HttpPost]
        public string SaveAvatar()
        {
            string pic = null;
            string user = Request.Params[0].Replace("@", "_").Replace(".", "_");
            HttpPostedFileBase file = Request.Files[0] as HttpPostedFileBase;
            if (file != null)
            {
                pic = "User_" + user + "." + Path.GetFileName(file.FileName).Split('.').Last();
                string path = Path.Combine(Server.MapPath("~/Content/Images/User"), pic);
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
            return "~/Content/Images/User/" + pic;
        }

        [HttpPost]
        public int FavoriteAuthor(int userId, int authorId)
        {
            _userService.LikeAuthor(authorId, userId);
            return _authorService.GetById(authorId).AuthorDetail.FavoriteUsers.Count;
        }
       
    }
}