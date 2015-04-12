﻿using BookStore.DO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DLL.Abstract
{
    public interface IUserService:IStoreService<User>
    {
        IQueryable<Book> GetReccomendedBooks(int userId);
        IQueryable<Book> GetWishedBooks(int userId);
        IQueryable<Book> GetRatedBooks(int userId);
        IQueryable<Author> GetFavAuthors(int userId);
        ICollection<Role> GetRoles(int userId);
        IQueryable<Comment> GetComment(int userId);
        User GetUserByEmail(string email);
         void RateBook(int rate,int userId, int bookId);
         void WishBook(Book book);
         void AddComment(Book book);
         void LikeAuthor(Author author);
        
    }
}
