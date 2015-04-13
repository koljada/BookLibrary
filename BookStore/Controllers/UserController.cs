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
        public UserController(IUserService userService, IBookService bookService)
        {
            _userService = userService;
            _bookService = bookService;
        }

        public ActionResult Profile(string user)
        {
            return View(_userService.GetUserByEmail(user));
        }
        [Authorize(Roles = "user")]
        public void RateBook(float rate, int bookId,bool isSuggestion)
        {
           // int userId = _userService.GetUserByEmail(System.Web.HttpContext.Current.User.Identity.Name).User_ID;
            int userId = (int)Session["UserId"];
            _userService.RateBook(rate, userId, bookId,false);
        }
        public ActionResult FileUpload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                string pic = "User" + Session["UserId"] + "." + Path.GetFileName(file.FileName).Split('.').Last();
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

        public ActionResult Rating()
        {
            var users = _userService.GetAll().ToList();
            //int maxUserId =users.Select(x => x.User_ID).Max();
            int maxBookId = _bookService.GetAll().Select(x => x.Book_ID).Max();
            float[,] matrix = new float[users.Count, maxBookId];
            float[,] result = new float[users.Count, maxBookId];

            //int[,] korelation = new int[users.Count, users.Count];
            float[,] corelation = new float[users.Count, users.Count];
            //List<double> corelation = new List<double>();
            List<float> average = new List<float>();
            List<float> diff = new List<float>();
            foreach (var user in users.Select((x, i) => new { Value = x, Index = i }))
            {
                int userId = user.Value.User_ID;
                float averageRate = 0;
                int countNonZero = 0;
                foreach (var rate in _userService.GetRatedBooks(userId))
                {
                    int bookId = rate.Book.Book_ID;
                    matrix[user.Index, bookId] = rate.RateValue;
                    if (rate.RateValue > 0)
                    {
                        averageRate += rate.RateValue;
                        countNonZero++;
                    }
                }
                average.Add((float)averageRate / countNonZero);

                if (user.Index > 0)
                {
                    for (int i = 1; i <= user.Index; i++) //по юзерам
                    {
                        double kor = 0;
                        float sum = 0, sumSquare1 = 0, sumSquare2 = 0;
                        for (int j = 0; j < maxBookId; j++) //по книжкам
                        {
                            if (matrix[user.Index, j] > 0 && matrix[user.Index - i, j] > 0)
                            {
                                sum += (matrix[user.Index, j] - average.ElementAt(user.Index)) *
                                       (matrix[user.Index - i, j] - average.ElementAt(user.Index - i));
                                sumSquare1 += (matrix[user.Index, j] - average.ElementAt(user.Index)) *
                                              (matrix[user.Index, j] - average.ElementAt(user.Index));
                                sumSquare2 += (matrix[user.Index - i, j] - average.ElementAt(user.Index - i)) *
                                              (matrix[user.Index - i, j] - average.ElementAt(user.Index - i));
                            }
                        }
                        diff.Add(sumSquare1);
                        kor = sum / (Math.Sqrt(sumSquare1) * Math.Sqrt(sumSquare2));
                        //corelation.Add(kor);
                        corelation[user.Index, user.Index - i] = corelation[user.Index - i, user.Index] = (float)kor;
                    }
                }
            }
            float[,] SubMatrix = (float[,])matrix.Clone();
            for (int i = 0; i < SubMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < SubMatrix.GetLength(1); j++)
                {
                    if (SubMatrix[i, j] == 0)
                    {
                        float sum = 0;
                        float sumCor = 0;
                        for (int k = 0; k < SubMatrix.GetLength(0); k++)
                        {
                            if (SubMatrix[k, j] != 0)
                            {
                                sum += (SubMatrix[k, j] - average[k]) * corelation[k, i];
                                sumCor += Math.Abs(corelation[k, i]);
                            }
                        }
                        if (sum != 0)
                        {
                            _userService.RateBook(average[i] + sum / sumCor,users[i].User_ID,j,true);
                            result[i, j] = average[i] + sum / sumCor;
                        }
                    }
                }
            }
            ViewBag.Result = result;
            return View(matrix);
        }
    }
}