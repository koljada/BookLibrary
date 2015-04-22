using System;
using System.Collections.Generic;
using System.Linq;
using BookStore.DO.Entities;
using System.Data.Entity;
using System.Diagnostics;
using System.Threading.Tasks;
using BookStore.DAL.Interface.Abstract;


namespace BookStore.DAL.EntityFramework
{
    public class EfUserRepository : EfStoreRepository<User>, IUserRepository
    {
        public override User GetById(int id)
        {
            using (EfDbContext context = new EfDbContext())
            {
                return context.Users.Include(x=>x.Comments).FirstOrDefault(x=>x.User_ID==id);
            }
        }

        public IList<Book> GetReccomendedBooks(int userId)
        {
            throw new NotImplementedException();
        }

        public IList<Book> GetWishedBooks(int userId)
        {
            throw new NotImplementedException();
        }

        public IList<Rate> GetRatedBooks(int userId)
        {
            using (EfDbContext context = new EfDbContext())
            {
                return context.Rates.Include(x => x.Book).Where(x => x.User_ID == userId).ToList();
            }
        }

        public IList<Author> GetFavAuthors(int userId)
        {
            throw new NotImplementedException();
        }

        public IList<Role> GetRoles(int userId)
        {
            using (EfDbContext context = new EfDbContext())
            {
                return context.Users.FirstOrDefault(u => u.User_ID == userId).Roles.ToList();
            }
        }

        public IList<Comment> GetComment(int userId)
        {
            throw new NotImplementedException();
        }

        public User GetUserByEmail(string email)
        {

            using (EfDbContext context = new EfDbContext())
            {
                return context.Users.Include(e => e.Roles)
                    .Include(x=>x.WishedBooks)
                    .Include(x => x.RatedBooks.Select(b=>b.Book.BookAuthors))
                    .Include(x => x.FavoriteAuthors)
                    .FirstOrDefault(e => e.Email == email);
            }
        }

        public async Task RateBook(float rate, int userId, int bookId, bool isSuggestion)
        {
            using (EfDbContext context = new EfDbContext())
            {
                //Stopwatch sw=new Stopwatch();
                //sw.Start();
                Book book = context.Books.FirstOrDefault(x => x.Book_ID == bookId);
                Rate rating = context.Rates.
                    FirstOrDefault(x => x.User_ID == userId && x.Book.Book_ID == bookId) ??
                              new Rate() { User_ID = userId };
                rating.RateValue = rate;
                rating.IsSuggestion = isSuggestion;
                book.RatedUsers.Add(rating);
                context.SaveChanges();
                //sw.Stop();
                //sw.Elapsed.Duration().Seconds;
                //Resuggest();
            }
        }

        public void WishBook(int bookId, int userId)
        {
            using (EfDbContext context = new EfDbContext())
            {
                context.Books.Find(bookId).WishedUsers.Add(context.Users.Find(userId));
                context.SaveChanges();
            }
        }

        public void AddComment(Book book)
        {
            throw new NotImplementedException();
        }

        public void LikeAuthor(int authorId, int userId)
        {

            using (EfDbContext context = new EfDbContext())
            {
                Author auth = context.Authors.FirstOrDefault(a => a.Author_ID == authorId);
                User user = context.Users.FirstOrDefault(a => a.User_ID == userId);
                if (!auth.FavoriteUsers.Contains(user))
                {
                    auth.FavoriteUsers.Add(user);
                }
                else
                {
                    auth.FavoriteUsers.Remove(user);
                }
                context.SaveChanges();
            }
        }

        public override void Create(User obj)
        {
            using (EfDbContext context = new EfDbContext())
            {
                Role user = context.Roles.FirstOrDefault(x => x.Name == "user");
                user.Users.Add(obj);
                context.SaveChanges();
            }
        }

        public void Suggest(float rate, int userId, int bookId, bool isSuggestion)
        {
            using (EfDbContext context = new EfDbContext())
            {
                Book book = context.Books.FirstOrDefault(x => x.Book_ID == bookId);
                Rate rating = context.Rates.
                    FirstOrDefault(x => x.User_ID == userId && x.Book.Book_ID == bookId) ??
                              new Rate() { User_ID = userId };
                rating.RateValue = rate;
                rating.IsSuggestion = isSuggestion;
                book.RatedUsers.Add(rating);
                context.SaveChanges();
            }
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

        public void Resuggest()
        {
            using (EfDbContext context = new EfDbContext())
            {
                var books = context.Books.ToList();
                var users = context.Users.ToList();
                int MaxBookId = books.Count;
                int MaxUserId = users.Count;
                float init = 0.1f;
                int features = MaxUserId < 15 ? MaxUserId : 15;
                int RateCounter = 0;
                Rate[][] matr = new Rate[MaxUserId][];
                foreach (var user in users.Select((x, i) => new { Value = x, Index = i }))
                {
                    var rates = GetRatedBooks(user.Value.User_ID).Where(x => x.IsSuggestion == false).ToList();
                    matr[user.Index] = new Rate[MaxBookId];
                    int i = user.Index;
                    for (int j = 0; j < MaxBookId; j++)
                    {
                        var rate = rates.FirstOrDefault(x => x.Book.Book_ID == books[j].Book_ID);
                        if (rate != null)
                        {
                            matr[i][j] = rate;
                            RateCounter++;
                        }
                        else
                        {
                            matr[i][j] =
                                (new Rate { User_ID = user.Value.User_ID, Book = new Book { Book_ID = books[j].Book_ID } });
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
                    //logger.Info("Iteration: " + iter_no + " RMSE:" + rmse.ToString("F"));

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
                            Rate rate = matr[i][j];
                            float sug = mu + b_u[i] + b_v[j] + PreditRating(UserFeatures[i], BookFeatures[j], features);
                            rate.RateValue = sug;
                            Suggest(sug, rate.User_ID, rate.Book.Book_ID, true);
                        }
                    }
                }
                int a = 0;
                //var users = GetAll().ToList();
                ////int maxUserId =users.Select(x => x.User_ID).Max();
                //int maxBookId = Context.Books.Select(x => x.Book_ID).Max();
                //float[,] matrix = new float[users.Count, maxBookId];
                //float[,] result = new float[users.Count, maxBookId];

                ////int[,] korelation = new int[users.Count, users.Count];
                //float[,] corelation = new float[users.Count, users.Count];
                ////List<double> corelation = new List<double>();
                //List<float> average = new List<float>();
                //List<float> diff = new List<float>();
                //foreach (var user in users.Select((x, i) => new { Value = x, Index = i }))
                //{
                //    int userId = user.Value.User_ID;
                //    float averageRate = 0;
                //    int countNonZero = 0;
                //    foreach (var rate in GetRatedBooks(userId))
                //    {
                //        int bookId = rate.Book.Book_ID;
                //        matrix[user.Index, bookId] = rate.RateValue;
                //        if (rate.RateValue > 0)
                //        {
                //            averageRate += rate.RateValue;
                //            countNonZero++;
                //        }
                //    }
                //    average.Add((float)averageRate / countNonZero);

                //    if (user.Index > 0)
                //    {
                //        for (int i = 1; i <= user.Index; i++) //по юзерам
                //        {
                //            double kor = 0;
                //            float sum = 0, sumSquare1 = 0, sumSquare2 = 0;
                //            for (int j = 0; j < maxBookId; j++) //по книжкам
                //            {
                //                if (matrix[user.Index, j] > 0 && matrix[user.Index - i, j] > 0)
                //                {
                //                    sum += (matrix[user.Index, j] - average.ElementAt(user.Index)) *
                //                           (matrix[user.Index - i, j] - average.ElementAt(user.Index - i));
                //                    sumSquare1 += (matrix[user.Index, j] - average.ElementAt(user.Index)) *
                //                                  (matrix[user.Index, j] - average.ElementAt(user.Index));
                //                    sumSquare2 += (matrix[user.Index - i, j] - average.ElementAt(user.Index - i)) *
                //                                  (matrix[user.Index - i, j] - average.ElementAt(user.Index - i));
                //                }
                //            }
                //            diff.Add(sumSquare1);
                //            kor = sum / (Math.Sqrt(sumSquare1) * Math.Sqrt(sumSquare2));
                //            //corelation.Add(kor);
                //            corelation[user.Index, user.Index - i] = corelation[user.Index - i, user.Index] = (float)kor;
                //        }
                //    }
                //}
                //float[,] SubMatrix = (float[,])matrix.Clone();
                //for (int i = 0; i < SubMatrix.GetLength(0); i++)
                //{
                //    for (int j = 0; j < SubMatrix.GetLength(1); j++)
                //    {
                //        if (SubMatrix[i, j] == 0)
                //        {
                //            float sum = 0;
                //            float sumCor = 0;
                //            for (int k = 0; k < SubMatrix.GetLength(0); k++)
                //            {
                //                if (SubMatrix[k, j] != 0)
                //                {
                //                    sum += (SubMatrix[k, j] - average[k]) * corelation[k, i];
                //                    sumCor += Math.Abs(corelation[k, i]);
                //                }
                //            }
                //            if (sum != 0)
                //            {
                //                Suggest(average[i] + sum / sumCor, users[i].User_ID, j, true);
                //                result[i, j] = average[i] + sum / sumCor;
                //            }
                //        }
                //    }
                //}
            }
        }
    }
}
