using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.DLL.Abstract;

namespace BookStore.Controllers
{
    public class SuggestController : AsyncController
    {
        private readonly IUserService _userService;
        private readonly IBookService _bookService;
        private readonly IAuthorService _authorService;

        private readonly log4net.ILog logger =
           log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public SuggestController(IUserService userService, IBookService bookService, IAuthorService authorService)
        {
            _userService = userService;
            _bookService = bookService;
            _authorService = authorService;
        }

        [Authorize(Roles = "user")]
        public void RateAsync(float rate, int bookId, bool isSuggestion)
        {
            int userId = (int)Session["UserId"];
            _userService.RateBook(rate, userId, bookId, false);
            logger.Info("ReSuggest Start!");
        }

        public void RateComltete()
        {
            logger.Info("ReSuggest Done!");
        }
    }
}