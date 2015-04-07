using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BookStore.Infrastructure;
using BookStore.HtmlHelpers;
using Ninject;
using System.Web.Security;
using BookStore.DO.Entities;

namespace BookStore
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            System.Net.ServicePointManager.Expect100Continue = false;
            NinjectControllerFactory controllerFactory = new NinjectControllerFactory();
            controllerFactory.InjectMembership(Membership.Provider);
            controllerFactory.InjectRoleProvider(Roles.Provider);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
            
        }
    }
}