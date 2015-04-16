using System;
using System.Collections.Generic;
using System.Linq;
using BookStore.DAL.Abstract;
using BookStore.DO.Entities;
using System.Data.Entity; 


namespace BookStore.DAL.EntityFramework
{
    public class EfUserRepository : EfStoreRepository<User>, IUserRepository
    {
        public IQueryable<Book> GetReccomendedBooks(int userId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Book> GetWishedBooks(int userId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Rate> GetRatedBooks(int userId)
        {
           return Context.Rates.Include(x => x.Book).Where(x => x.User_ID == userId);
        }

        public IQueryable<Author> GetFavAuthors(int userId)
        {
            throw new NotImplementedException();
        }

        public ICollection<Role> GetRoles(int userId)
        {
            return Context.Users.FirstOrDefault(u => u.User_ID == userId).Roles;
        }

        public IQueryable<Comment> GetComment(int userId)
        {
            throw new NotImplementedException();
        }

        public User GetUserByEmail(string email)
        {
            return Context.Users.Include(e=>e.Roles)
                .Include(x => x.RatedBooks)
                .Include(x => x.FavoriteAuthors)
                .FirstOrDefault(e => e.Email == email);
        }

        public void RateBook(float rate,int userId, int bookId,bool isSuggestion)
        {
            Book book = Context.Books.FirstOrDefault(x => x.Book_ID == bookId);
            Rate rating = Context.Rates.
                FirstOrDefault(x => x.User_ID == userId && x.Book.Book_ID == bookId) ??
                          new Rate() {User_ID = userId};
            rating.RateValue = rate;
            rating.IsSuggestion = isSuggestion;
            book.RatedUsers.Add(rating);
            Context.SaveChanges();
            //Resuggest();
        }

        public void WishBook(Book book)
        {
            throw new NotImplementedException();
        }

        public void AddComment(Book book)
        {
            throw new NotImplementedException();
        }

        public void LikeAuthor(Author author)
        {
            throw new NotImplementedException();
        }

        public override void Create(User obj)
        {
            Role user = Context.Roles.FirstOrDefault(x=>x.Name=="user");
            user.Users.Add(obj);
            Context.SaveChanges();
        }

        public override IQueryable<User> GetAll()
        {
            return Context.Users;
        }
        public void Suggest(float rate, int userId, int bookId, bool isSuggestion)
        {
            Book book = Context.Books.FirstOrDefault(x => x.Book_ID == bookId);
            Rate rating = Context.Rates.
                FirstOrDefault(x => x.User_ID == userId && x.Book.Book_ID == bookId) ??
                          new Rate() { User_ID = userId };
            rating.RateValue = rate;
            rating.IsSuggestion = isSuggestion;
            book.RatedUsers.Add(rating);
            Context.SaveChanges();

        }
        public void Resuggest()
        {
            var users = GetAll().ToList();
            //int maxUserId =users.Select(x => x.User_ID).Max();
            int maxBookId = Context.Books.Select(x => x.Book_ID).Max();
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
                foreach (var rate in GetRatedBooks(userId))
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
                            Suggest(average[i] + sum / sumCor, users[i].User_ID, j, true);
                            result[i, j] = average[i] + sum / sumCor;
                        }
                    }
                }
            }
        }
    }
}
