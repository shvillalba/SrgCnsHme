using System.Web;
using System.Web.Optimization;

namespace SrgCnsHme
{
  public class BundleConfig
  {
    // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
    public static void RegisterBundles(BundleCollection bundles)
    {
      #region Style Bundles
      bundles.Add(new StyleBundle("~/content/css").Include(
                "~/Content/body.css",
                "~/Content/Sc.css"));
      bundles.Add(new StyleBundle("~/content/bootstrap").Include(
        "~/BootstrapSupport/css/bootstrap.css",
        "~/BootstrapSupport/css/bootstrap-responsive.css",
        "~/BootstrapSupport/css/datepicker.css",
        "~/BootstrapSupport/FontAwesome3.0/css/font-awesome.css"
        ));

      // Added in _Layout, right after the one before here
      //bundles.Add(new StyleBundle("~/Content/jQueryUi").Include(
      //            "~/Content/themes/base/jquery.ui.css",
      //            "~/Content/themes/base/jquery.ui.accordion.css",
      //            "~/Content/themes/base/jquery.ui.all.css",
      //            "~/Content/themes/base/jquery.ui.autocomplete.css",
      //            "~/Content/themes/base/jquery.ui.base.css",
      //            "~/Content/themes/base/jquery.ui.button.css",
      //            "~/Content/themes/base/jquery.ui.core.css",
      //            "~/Content/themes/base/jquery.ui.datepicker.css",
      //            "~/Content/themes/base/jquery.ui.dialog.css",
      //            "~/Content/themes/base/jquery.ui.menu.css",
      //            "~/Content/themes/base/jquery.ui.progressbar.css",
      //            "~/Content/themes/base/jquery.ui.resizable.css",
      //            "~/Content/themes/base/jquery.ui.selectable.css",
      //            "~/Content/themes/base/jquery.ui.slider.css",
      //            "~/Content/themes/base/jquery.ui.spinner.css",
      //            "~/Content/themes/base/jquery.ui.tabs.css",
      //            "~/Content/themes/base/jquery.ui.theme.css",
      //            "~/Content/themes/base/jquery.ui.tooltip.css"));

      bundles.Add(new StyleBundle("~/Content/learning").Include("~/Content/learning.css"));

      #endregion

      #region Script Bundles

      bundles.Add(new ScriptBundle("~/bundles/jsSupport").Include("~/Scripts/moment.js"));

      // Added to Edit views
      bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

      // Use the development version of Modernizr to develop with and learn from. Then, when you're
      // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
      // Added in _Layout, right after styles
      bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));

      //// Added in _Layout, at the bottom/end of file
      //bundles.Add(new ScriptBundle("~/js").Include(
      //          "~/Scripts/jquery-{version}.js",
      //          //"~/Scripts/jquery-migrate-{version}.js",
      //          "~/Scripts/bootstrap.js",
      //          "~/Scripts/jquery.unobtrusive*",
      //          "~/Scripts/jquery-ui-{version}.js",
      //          "~/Scripts/SrgCnsHme.js"));

      // It seems that bundling the jquery-migrate together with the rest of scripts was causing an error that made 
      // all scripts fail (when app was published to AppHarbor.com. Putting "jquery.migrate-{version}.js" in a
      //separate bundle seems to have fixed this.
      bundles.Add(new ScriptBundle("~/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

      bundles.Add(new ScriptBundle("~/jquery-migrate").Include(
                "~/Scripts/jquery-migrate-{version}.js"));

      bundles.Add(new ScriptBundle("~/js").Include(
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/knockout-{version}.js",
                "~/Scripts/jquery.linq.js"));

      bundles.Add(new ScriptBundle("~/scripts/bootstrap").Include(
               "~/BootstrapSupport/js/bootstrap.js",
               "~/BootstrapSupport/js/bootstrap-datepicker.js",
               "~/BootstrapSupport/js/bootstrap-dropdown.js"));

      //bundles.Add(new ScriptBundle("~/bundles/jQueryUi").Include("~/Scripts/jquery-ui-{version}.js"));

      bundles.Add(new ScriptBundle("~/scripts/sergio").Include(
               "~/Scripts/SrgCnsHme.js",
               "~/Scripts/date.format.js"));

      bundles.Add(new ScriptBundle("~/jsLearn").Include(
                "~/Scripts/jquery.xdomainajax.js",
                "~/Scripts/sammy.js"));

      #endregion
    }
  }
}