using System.Web;
using System.Web.Optimization;

namespace UrlShortenerWebAPI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Add Popper.js (required for Bootstrap 4+ tooltips, popovers, and dropdowns)
            bundles.Add(new ScriptBundle("~/bundles/popper").Include(
                      "~/Scripts/umd/popper.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.bundle.js")); // Bootstrap 4+ includes Popper.js
            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                        "~/Scripts/kendo/js/angular.min.js",                        
                        "~/Scripts/kendo/customJSBundle/admin/kendo.custom.min-2016.1.112.js",
                        "~/Scripts/kendo/js/angular-cookies.min.js",
                        "~/Scripts/app/app.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/home/index").Include(
               //"~/Scripts/app/factories/countyHttp.js",
               "~/Scripts/app/factories/shortenedUrlsHttp.js",
               "~/Scripts/app/directives/validateShortKeyword.js", 



                      "~/Scripts/app/controllers/home/index/urlShortenerCtrl.js",
                      "~/Scripts/app/controllers/home/index/shortenedUrlSchema.js",
                      "~/Scripts/app/controllers/home/index/index/gridEditorCtrl.js"
                    ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/bootstrap-reboot.css",
                      "~/Content/bootstrap-grid.css"));
            bundles.Add(new StyleBundle("~/Content/kendo/styles/kendo-bootstrap-default").Include(
                    "~/Content/kendo/styles/kendo.common-bootstrap.min.css",
                    "~/Content/kendo/styles/kendo.default.min.css",
                    "~/Content/kendo/styles/kendo.dataviz.min.css",
                    "~/Content/kendo/styles/kendo.dataviz.bootstrap.min.css"));
        }
    }
}
