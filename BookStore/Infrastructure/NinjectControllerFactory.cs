using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing; 
using Ninject;
using BookStore.Domain.Entities;
using BookStore.Domain.Abstract;
using Moq;
using BookStore.Domain.Concrete;


namespace BookStore.Infrastructure
{
    public class NinjectControllerFactory: DefaultControllerFactory
    {
        private IKernel ninjectKernel;
        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)ninjectKernel.Get(controllerType); 
        }

        private void AddBindings()
        {
            //throw new NotImplementedException();
            //Mock<IBookRepository> mock = new Mock<IBookRepository>();
            //mock.Setup(m => m.Books).Returns(new List<Book>{
            //    new Book {Title="1984",Price=234,Author="Georeg Orwell", Genre="Antiutopia",Annotation="A good book about today reality", Rate=9.9},
            //    new Book {Title="For whom the bell tolls",Price=350,Author="Hemongway Ernest", Genre="War novel",Annotation="A  book about spain civil war", Rate=9.4}
            //}.AsQueryable());

            //ninjectKernel.Bind<IBookRepository>().ToConstant(mock.Object);
            ninjectKernel.Bind<IBookRepository>().To<EFBookRepository>();
        }
    }
}