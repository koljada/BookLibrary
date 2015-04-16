using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BookStore.DLL.Abstract;
using BookStore.DO.Entities;
using NLog;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Double;

namespace BookStore.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IBookService _bookService;
        private readonly log4net.ILog logger =
           log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
        public void RateBook(float rate, int bookId, bool isSuggestion)
        {
            // int userId = _userService.GetUserByEmail(System.Web.HttpContext.Current.User.Identity.Name).User_ID;
            int userId = (int)Session["UserId"];
            _userService.RateBook(rate, userId, bookId, false);
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
        public float SclarProduct(float[] u_f, float[] v_f, int dim)
        {
            float res = 0;
            for (int i = 0; i < dim; i++)
            {
                res += u_f[i] * v_f[i];
            }
            return res;
        }

        public float PreditRating(float[] UserFeatures, float[] BookFeatures, int features)
        {
            float sum = 0;
            for (int i = 0; i < features; i++)
            {
                sum += UserFeatures[i] * BookFeatures[i];
            }
            return sum;
        }

        public ActionResult Rating()
        {
            var books = _bookService.GetAll().ToList();
            var users = _userService.GetAll().ToList();
            int MaxBookId = books.Count;
            int MaxUserId = users.Count;
            float init = 0.1f;
            int features = 5, RateCounter = 0;
            Rate[][] matr = new Rate[MaxUserId][];
            foreach (var user in users.Select((x, i) => new { Value = x, Index = i }))
            {
                var rates = _userService.GetRatedBooks(user.Value.User_ID).Where(x=>x.IsSuggestion==false).ToList();
                matr[user.Index] = new Rate[MaxBookId];
                int i = user.Index;
                for (int j = 0; j < MaxBookId; j++)
                {
                    var rate = rates.FirstOrDefault(x=>x.Book.Book_ID==books[j].Book_ID);
                    if (rate != null)
                    {
                        matr[i][j] = rate;
                        RateCounter++;
                    }
                    else
                    {
                        matr[i][j] = (new Rate() { User_ID = user.Value.User_ID, Book = new Book() { Book_ID = books[j].Book_ID } });
                    }
                }
            }
            float[] b_u = new float[MaxUserId];
            float[] b_v = new float[MaxBookId];
            float[][] UserFeatures = new float[MaxUserId][];
            float[][] BookFeatures = new float[MaxBookId][];
            for (int i = 0; i < MaxUserId; i++)
            {
                UserFeatures[i] = new float[features];
                for (int j = 0; j < features; j++)
                {
                    UserFeatures[i][j] = init;
                }
            }
            for (int i = 0; i < MaxBookId; i++)
            {
                BookFeatures[i] = new float[features];
                for (int j = 0; j < features; j++)
                {
                    BookFeatures[i][j] = init;
                }
            }

            int iter_no = 0;
            float err = 0;
            float rmse = 1f;
            float old_rmse = 0;
            float threshold = 0.01f;
            float lRate = 0.001f;
            float K = 0.015f;
            float mu = 0, eta = 0.1f, lambda2 = 0.015f, min_improvement = 0.0001f;
            while (Math.Abs(old_rmse - rmse) > 0.00001)
            {
                old_rmse = rmse;
                rmse = 0;
                for (int i = 0; i < matr.GetLength(0); i++)
                {
                    for (int j = 0; j < matr[i].Length; j++)
                    {
                        if (matr[i][j].RateValue != 0)
                        {
                            err = matr[i][j].RateValue -
                              (mu + b_u[i] + b_v[j] + PreditRating(UserFeatures[i], BookFeatures[j], features));
                            rmse += err * err;
                            mu += eta * err;
                            b_v[j] += eta * (err - lambda2 * b_v[j]);
                            b_u[i] += eta * (err - lambda2 * b_u[i]);
                            for (int k = 0; k < features; k++)
                            {
                                var uprev = UserFeatures[i][k];
                                var bprev = BookFeatures[j][k];
                                UserFeatures[i][k] += eta * (err * bprev - lambda2 * uprev);
                                BookFeatures[j][k] += eta * (err * uprev - lambda2 * bprev);
                            }
                        }
                    }
                }
                ++iter_no;
                rmse = (float)Math.Sqrt(rmse / RateCounter);
                logger.Info("Iteration: " + iter_no + " RMSE:" + rmse.ToString("F"));

                if (rmse > old_rmse - threshold)
                {
                    eta = eta * 0.66f;
                    threshold = threshold * 0.5f;
                }
            }

            for (int i = 0; i < MaxUserId; i++)
            {
                for (int j = 0; j < MaxBookId; j++)
                {
                    if (matr[i][j].RateValue == 0)
                    {
                        matr[i][j].RateValue = mu + b_u[i] + b_v[j] + PreditRating(UserFeatures[i], BookFeatures[j], features);
                    }
                }
            }
            return View(matr);

        }
    }
}