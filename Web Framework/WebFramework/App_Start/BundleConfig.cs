using System.Web;
using System.Web.Optimization;

namespace WebFramework
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/home-page-layout-css")
                .Include("~/Content/Site/less/css/home-page-layout.css"));
            bundles.Add(new ScriptBundle("~/bundles/home-page-layout-js")
                .Include("~/Content/Site/js/error-handler.js",
                         "~/Content/Site/js/utils.js",
                         "~/Content/Site/js/common.js",
                         "~/Content/Site/js/home-page-layout.js"));

            bundles.Add(new StyleBundle("~/bundles/centered-layout-css")
                .Include("~/Content/Site/less/css/centered-layout.css"));
            bundles.Add(new ScriptBundle("~/bundles/centered-layout-js")
                .Include("~/Content/Site/js/error-handler.js",
                         "~/Content/Site/js/utils.js",
                         "~/Content/Site/js/common.js",
                         "~/Content/Site/js/centered-layout.js"));

            bundles.Add(new StyleBundle("~/bundles/submenu-layout-css")
                .Include("~/Content/Site/less/css/submenu-layout.css"));
            bundles.Add(new ScriptBundle("~/bundles/submenu-layout-js")
                .Include("~/Content/Site/js/error-handler.js",
                         "~/Content/Site/js/utils.js",
                         "~/Content/Site/js/common.js",
                         "~/Content/Site/js/submenu-layout.js"));

            bundles.Add(new StyleBundle("~/bundles/blog-layout-css")
                .Include("~/Content/Site/less/css/blog-layout.css"));
            bundles.Add(new ScriptBundle("~/bundles/blog-layout-js")
                .Include("~/Content/Site/js/error-handler.js",
                         "~/Content/Site/js/utils.js",
                         "~/Content/Site/js/common.js",
                         "~/Areas/Blog/Scripts/blog-layout.js"));

// Pages

            bundles.Add(new StyleBundle("~/bundles/fiddle-styles")
                .Include("~/Content/Site/less/css/fiddle-styles.css"));
            bundles.Add(new ScriptBundle("~/bundles/fiddle-scripts")
                .Include("~/Content/Site/js/fiddle-script.js"));

            bundles.Add(new StyleBundle("~/bundles/form-data-styles")
                .Include("~/Content/Site/less/css/form-data.css"));
            bundles.Add(new ScriptBundle("~/bundles/form-data-validation")
                .Include("~/Content/jQuery/jquery.unobtrusive-ajax.js",
                         "~/Content/jQuery/jquery.validate.js",
                         "~/Content/jQuery/jquery.validate.unobtrusive.js"));

            bundles.Add(new ScriptBundle("~/bundles/blog/all-posts")
                .Include("~/Areas/Blog/Scripts/AllPosts.Api.js",
                         "~/Areas/Blog/Scripts/AllPosts.VM.js",
                         "~/Areas/Blog/Scripts/AllPosts.Controller.js"));

#if DEBUG
            bundles.GetBundleFor("~/bundles/home-page-layout-js").Include("~/Content/Site/js/debug-only.js");
            bundles.GetBundleFor("~/bundles/centered-layout-js").Include("~/Content/Site/js/debug-only.js");
            bundles.GetBundleFor("~/bundles/submenu-layout-js").Include("~/Content/Site/js/debug-only.js");
            bundles.GetBundleFor("~/bundles/blog-layout-js").Include("~/Content/Site/js/debug-only.js");
#endif

#if LOCAL // && DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif

            bundles.Add(new StyleBundle("~/bundles/bootstrap-fontawesome-css")
                .Include("~/Content/Bootstrap/less/css/bootstrap.css",
                         "~/Content/Bootstrap/font-awesome/css/font-awesome.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/jQuery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Content/jQuery/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Content/jQuery/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/jQuery-js")
                .Include("~/Content/jQuery/jquery-{version}.js",
                         "~/Content/jQuery/jquery-migrate-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jQuery-ui-js")
                .Include("~/Content/jQuery/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/linq-js")
                .Include("~/Content/jQuery/linq.js"));

            bundles.Add(new ScriptBundle("~/bundles/knockout-js")
                .Include("~/Content/KnockoutJs/knockout-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-js")
                .Include("~/Content/Bootstrap/js/bootstrap.js"));

        }
    }
}
