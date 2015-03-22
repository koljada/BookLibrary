using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BookStore
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(null, "", new
            {
                controller = "Book",
                action = "List",
               genre = (string)null,
                page = 1
            });

            routes.MapRoute(null, "Page{page}", new
            {
                controller = "Book",
                action = "List",
               genre = (string)null
            },
            new { page = @"\d+" });

            routes.MapRoute(null, "{genre}", new
            {
                controller = "Book",
                action = "List",               
                page = 1
            });

            routes.MapRoute(null, "{genre}/Page{page}", new
            {
                controller = "Book",
                action = "List"                
            },
            new { page = @"\d+" });           

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}"
            );
        }
    }
}