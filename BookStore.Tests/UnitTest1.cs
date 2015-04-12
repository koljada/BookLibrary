﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BookStore.BLL.RepositoryService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookStore.Controllers;
using BookStore.DAL.Abstract;
using BookStore.DAL.EntityFramework;
using BookStore.DLL.Abstract;
using BookStore.DO.Entities;
using BookStore.Models;
using Moq;
using Ninject;

namespace BookStore.Tests
{
    [TestClass]
    public class BookControllerTest
    {
        private List<Book> list = new List<Book>();
        private const int page = 1;
        [TestMethod]
        public void Paginate()
        {
            // Arrange
            Mock<IBookService> mockBook = new Mock<IBookService>();
            Mock<IAuthorService> mockAuthor = new Mock<IAuthorService>();
            BookController controller = new BookController(mockBook.Object,mockAuthor.Object);
            int PageSize = 10;
            for (int i = 1; i < 50; i++)
            {
                list.Add(new Book());
            }

            // Act
            ICollection<Book> result = controller.PaginateBooks(list, page);
            
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(PageSize,result.Count());
        }
        [TestMethod]
        public void List()
        {
            // Arrange
            Mock<IBookService> mockBook = new Mock<IBookService>();
            Mock<IAuthorService> mockAuthor = new Mock<IAuthorService>();
            //mock.Setup(m => m.GetAll()).Returns<IQueryable<Book>>(x => x);
            BookController controller = new BookController(mockBook.Object, mockAuthor.Object);
            // Act
            ViewResult result = controller.List(page);
            // Assert
            Assert.IsNotNull(result);
        }
    }
}