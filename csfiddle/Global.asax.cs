using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace csfiddle
{
    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterMvcRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        public static void RegisterApiRoutes(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/sitescripts")
                .Include(
                    "~/Scripts/jquery-{version}.js",
                    "~/Scripts/jquery-ui-{version}.js",
                    "~/Scripts/jquery.layout.js",
                    "~/Scripts/codemirror-3.01/codemirror.js",
                    "~/Scripts/codemirror-3.0/mode/clike.js",
                    "~/Scripts/site.js"
                )
            );
            bundles.Add(new StyleBundle("~/sitecss")
                .Include(
                    "~/Content/jquery.ui.layout.css",
                    "~/Content/site.css",
                    "~/Content/codemirror-3.01/codemirror.css"
                )
            );

            //BundleTable.EnableOptimizations = true;
        }

        protected void Application_Start()
        {
            RegisterApiRoutes(GlobalConfiguration.Configuration);
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterMvcRoutes(RouteTable.Routes);
            RegisterBundles(BundleTable.Bundles);
        }
    }
}