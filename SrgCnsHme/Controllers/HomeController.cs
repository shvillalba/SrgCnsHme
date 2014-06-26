using System;
using System.Web.Mvc;

namespace SrgCnsHme.Controllers
{
  public class HomeController : Controller
  {
    public ActionResult Index()
    {
      ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

      return View();
    }

    public ActionResult About()
    {
      ViewBag.Message = "Sergio H. V. play site.";

      return View();
    }

    public ActionResult Contact()
    {
      ViewBag.Message = "May change, but for now use these:";

      return View();
    }
  }
}
