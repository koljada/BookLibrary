using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using BookStore.DAL.Abstract;
using Moq;
using System.Web;
using BookStore.DLL.Abstract;
using System.Web.Security;
using BookStore.BLL.RepositoryService;
using BookStore.DAL.EntityFramework;
using Ninject.Modules;


namespace BookStore.Infrastructure
{

    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private class SiteServices : NinjectModule
        {
            public override void Load()
            {
                Bind<IRoleService>().To<RoleService>();
                Bind<IUserService>().To<UserService>();
                Bind(typeof(IStoreRepository<>)).To(typeof(EfStoreRepository<>));
            }
        }
        private readonly IKernel _ninjectKernel ;

        public NinjectControllerFactory()
        {
            _ninjectKernel = new StandardKernel(new SiteServices());
            AddBindings();
        }
/*
        public NinjectControllerFactory(IKernel kernel)
        {
            _ninjectKernel = kernel;
            AddBindings();
        }*/
        public void InjectMembership(MembershipProvider provider)
        {
            _ninjectKernel.Inject(provider);
        }
        public void InjectRoleProvider(RoleProvider provider)
        {
            _ninjectKernel.Inject(provider);
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)_ninjectKernel.Get(controllerType);
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

            //_ninjectKernel.Bind(typeof(IStoreService<>)).To(typeof(StoreService<>));

            _ninjectKernel.Bind<IGenreService>().To<GenreService>();
            _ninjectKernel.Bind<IAuthorService>().To<AuthorService>();
            _ninjectKernel.Bind<IBookService>().To<BookService>();
            //_ninjectKernel.Bind<IRoleService>().To<RoleService>();
            //_ninjectKernel.Bind<IUserService>().To<UserService>();

            //_ninjectKernel.Bind(typeof(IStoreRepository<>)).To(typeof(EfStoreRepository<>));

            _ninjectKernel.Bind<IBookRepository>().To<EfBookRepository>();
            _ninjectKernel.Bind<IGenreRepository>().To<EfGenreRepository>();
            _ninjectKernel.Bind<IAuthorRepository>().To<EfAuthorRepository>();
            _ninjectKernel.Bind<IRoleRepository>().To<EfRoleRepository>();
            _ninjectKernel.Bind<IUserRepository>().To<EfUserRepository>();

            //_ninjectKernel.Bind<MembershipProvider>().To<CustomMembershipProvider>();
            //_ninjectKernel.Bind<RoleProvider>().To<CustomRoleProvider>();
        }
    }
}