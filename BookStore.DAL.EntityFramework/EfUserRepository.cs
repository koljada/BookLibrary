using System;
using System.Collections.Generic;
using System.Linq;
using BookStore.DO.Entities;
using System.Data.Entity;
using System.Data.SqlTypes;
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
                return context.Users.Include(x => x.Profile.Comments).FirstOrDefault(x => x.User_ID == id);
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
                return context.Rates.Include(x => x.Book).Where(x => x.User.User_ID == userId).ToList();
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
                return context.Users.Include(x=>x.Roles).FirstOrDefault(u => u.User_ID == userId).Roles.ToList();
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
                return context.Users.Include(x => x.Profile)
                    .Include(e => e.Roles)
                    .Include(x => x.Profile.WishedBooks.Select(c => c.Book.BookAuthors))
                    .Include(x => x.Profile.RatedBooks.Select(b => b.Book.Book.BookAuthors))
                    .Include(x => x.Profile.FavoriteAuthors.Select(c => c.Author))
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
                    FirstOrDefault(x => x.User.User_ID == userId && x.Book.Book_ID == bookId) ??
                              new Rate() { User = context.Users.FirstOrDefault(x => x.User_ID == userId).Profile };
                rating.RateValue = rate;
                rating.IsSuggestion = isSuggestion;
                book.BookDetail.RatedUsers.Add(rating);
                context.SaveChanges();
                //sw.Stop();
                //sw.Elapsed.Duration().Seconds;
                Resuggest1();
            }
        }

        public void WishBook(int bookId, int userId)
        {
            using (EfDbContext context = new EfDbContext())
            {
                context.Books.Find(bookId).BookDetail.WishedUsers.Add(context.Users.Find(userId).Profile);
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
                if (!auth.AuthorDetail.FavoriteUsers.Contains(user.Profile))
                {
                    auth.AuthorDetail.FavoriteUsers.Add(user.Profile);
                }
                else
                {
                    auth.AuthorDetail.FavoriteUsers.Remove(user.Profile);
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
                Rate rating = context.Rates.
                    FirstOrDefault(x => x.User.User_ID == userId && x.Book.Book_ID == bookId) ??
                              new Rate()
                              {
                                  User = context.Users.Include(x => x.Profile).FirstOrDefault(x => x.User_ID == userId).Profile,
                                  Book = context.Books.Include(x => x.BookDetail).FirstOrDefault(x => x.Book_ID == bookId).BookDetail
                              };
                rating.RateValue = rate;
                rating.IsSuggestion = isSuggestion;
                context.Rates.Add(rating);
                context.SaveChanges();
            }
        }
        public float PreditRating(float[] userFeatures, float[] bookFeatures, int features)
        {
            float sum = 0;
            for (int i = 0; i < features; i++)
            {
                sum += userFeatures[i] * bookFeatures[i];
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
                                (new Rate { User = context.Users.FirstOrDefault(x => x.User_ID == user.Value.User_ID).Profile, Book = new BookDetail() { Book_ID = books[j].Book_ID } });
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
                            Suggest(sug, rate.User.User_ID, rate.Book.Book_ID, true);
                        }
                    }
                }
            }
        }

        public void Resuggest1()
        {
            List<Rate> dbRates = new List<Rate>();
            using (EfDbContext context = new EfDbContext())
            {
                dbRates = context.Rates.Include(i => i.Book).Include(i => i.User).ToList();
            }
            HashSet<int> bookIds = new HashSet<int>();
            HashSet<int> userIds = new HashSet<int>();
            foreach (var dbRate in dbRates)
            {
                if (!bookIds.Contains(dbRate.Book.Book_ID))
                {
                    bookIds.Add(dbRate.Book.Book_ID);
                }
                if (!userIds.Contains(dbRate.User.User_ID))
                {
                    userIds.Add(dbRate.User.User_ID);
                }
            }
            float[,] result = new float[userIds.Count, bookIds.Count];
            Rate[,] matr = new Rate[userIds.Count, bookIds.Count];
            int rateCounter = 0;
            float[,] corelation = new float[userIds.Count, userIds.Count];
            List<float> average = new List<float>();
            List<float> diff = new List<float>();
            for (int i = 0; i < userIds.Count; i++)
            {
                float averageRate = 0;
                int countNonZero = 0;
                for (int j = 0; j < bookIds.Count; j++)
                {
                    var rate = dbRates.FirstOrDefault(x => x.IsSuggestion == false && x.Book.Book_ID == bookIds.ElementAt(j) && x.User.User_ID == userIds.ElementAt(i));
                    if (rate != null)
                    {
                        matr[i, j] = rate;
                        rateCounter++;
                        averageRate += rate.RateValue;
                        countNonZero++;
                        average.Add(averageRate / countNonZero);
                    }
                    else
                    {
                        matr[i, j] =
                            (new Rate { IsSuggestion = true });
                    }
                }
                if (i > 0)
                {
                    for (int k = 1; k <= i; k++) //�� ������
                    {
                        double kor = 0;
                        float sum = 0, sumSquare1 = 0, sumSquare2 = 0;
                        for (int j = 0; j < bookIds.Count; j++) //�� �������
                        {
                            if (!matr[i, j].IsSuggestion && !matr[i - k, j].IsSuggestion)
                            {
                                sum += (matr[i, j].RateValue - average.ElementAt(i)) *
                                       (matr[i - k, j].RateValue - average.ElementAt(i - k));
                                sumSquare1 += (matr[i, j].RateValue - average.ElementAt(i)) *
                                              (matr[i, j].RateValue - average.ElementAt(i));
                                sumSquare2 += (matr[i - k, j].RateValue - average.ElementAt(i - k)) *
                                              (matr[i - k, j].RateValue - average.ElementAt(i - k));
                            }
                        }
                        diff.Add(sumSquare1);
                        kor = sum / (Math.Sqrt(sumSquare1) * Math.Sqrt(sumSquare2));
                        corelation[i, i - k] = corelation[i - k, i] = (float)kor;
                    }
                }
            }
            Rate[,] subMatrix = (Rate[,])matr.Clone();
            for (int i = 0; i < subMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < subMatrix.GetLength(1); j++)
                {
                    if (subMatrix[i, j].IsSuggestion)
                    {
                        float sum = 0;
                        float sumCor = 0;
                        for (int k = 0; k < subMatrix.GetLength(0); k++)
                        {
                            if (!subMatrix[k, j].IsSuggestion && !float.IsNaN(average[k]))
                            {
                                sum += (subMatrix[k, j].RateValue - average[k]) * corelation[k, i];
                                sumCor += Math.Abs(corelation[k, i]);
                            }
                        }
                        if (sum != 0 && !float.IsNaN(sum))
                        {
                            Suggest((average[i] + sum / sumCor) > 10 ? 10 : (average[i] + sum / sumCor), userIds.ElementAt(i), bookIds.ElementAt(j), true);
                            result[i, j] = average[i] + sum / sumCor;
                        }
                    }
                }

            }
        }
    }

}
