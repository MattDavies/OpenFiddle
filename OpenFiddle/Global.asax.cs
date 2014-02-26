using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace OpenFiddle
{
    public class MvcApplication : HttpApplication
    {
        public static void RegisterMvcRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(name: "Home", url: "", defaults: new { controller = "Home", action = "Index" });
            routes.MapRoute(name: "Run", url: "Run", defaults: new { controller = "Home", action = "Run" });
            routes.MapRoute(name: "Save", url: "Save", defaults: new { controller = "Home", action = "Save" });
            routes.MapRoute(name: "Show", url: "{id}", defaults: new { controller = "Home", action = "Show" }, constraints: new { id = @"^\w{8}$" });
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
            bundles.Add(new ScriptBundle("~/editor")
                .Include(
                    "~/Scripts/editor.js"
                )
            );

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

            BundleTable.EnableOptimizations = false;
        }

        protected void Application_Start()
        {
            RegisterApiRoutes(GlobalConfiguration.Configuration);
            RegisterMvcRoutes(RouteTable.Routes);
            RegisterBundles(BundleTable.Bundles);
        }
    }
}