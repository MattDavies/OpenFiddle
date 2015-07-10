using System.Web;
using System.Web.Optimization;

namespace OpenFiddle
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bootstrap").Include(
                     "~/Scripts/bootstrap.js"));

            //Moved bootstrap css to own style tag in the _layout page.  This is to remove it from the optimizations which was breaking the fonts and icons.
            bundles.Add(new StyleBundle("~/styles").IncludeDirectory("~/Content", "*.css", true));


            bundles.Add(new ScriptBundle("~/ng").Include(
                        "~/Scripts/angular.js",
                        "~/Scripts/angular-route.js",
                        "~/Scripts/angular-cookies.js"));

            bundles.Add(new ScriptBundle("~/app").IncludeDirectory("~/Scripts/app", "*.js", true));
            bundles.Add(new ScriptBundle("~/ace").IncludeDirectory("~/Scripts/ace", "*.js", true));

            bundles.Add(new ScriptBundle("~/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/misc").IncludeDirectory("~/Scripts/misc", "*.js", true));

            bundles.Add(new ScriptBundle("~/highlight").IncludeDirectory("~/Scripts/misc", "*.js", true));

        }
    }
}
