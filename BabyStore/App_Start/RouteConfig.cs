using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BabyStore
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "ProductDetails",
                url: "Product/Details",
                defaults: new { controller = "Product", action = "Details" }
            );

            routes.MapRoute(
                name: "ProductCreate",
                url: "Product/Create",
                defaults: new { controller = "Product", action = "Create" }
            );

            routes.MapRoute(
                name: "ProductbyCategorybyPage",
                url: "Product/{category}/Page{page}",
                defaults: new { controller = "Product", action = "Index" }
            );

            routes.MapRoute(
                name: "ProductbyPage",
                url: "Product/Page{page}",
                defaults: new { controller = "Product", action = "Index" }
            );

            routes.MapRoute(
                name: "ProductbyCategory",
                url: "Product/{category}",
                defaults: new { controller = "Product", action = "Index" }
            );

            routes.MapRoute(
                name: "ProductIndex",
                url: "Product",
                defaults: new { controller = "Product", action = "Index" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
