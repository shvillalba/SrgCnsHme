using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SrgCnsHme
{
  public class RouteConfig
  {
    public static void RegisterRoutes(RouteCollection routes)
    {
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

      // Needed, to make sure the {folder}, which is added in the ActionLink in _Layout, is passed as route parameter (/mail), not as query string (?folder=mail). This is needed by sammy.js routing for default mailbox ("#Inbox")
      routes.MapRoute("koTut3", "Learn/KnockoutTraining3/{folder}", new { controller = "Learn", action = "KnockoutTraining3" });

      routes.MapRoute(
        name: "Default",
        url: "{controller}/{action}/{id}",
        defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
      );
    }
  }
}