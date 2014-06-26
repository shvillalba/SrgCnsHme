using System;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using SrgCnsHme.Infrastructure;

namespace SrgCnsHme.Controllers
{
  [Authorize(Users = "Sergio")]
  public class MaintenanceController : Controller
  {
    public ActionResult Index()
    {
      var providers = new string[] { "System.Data.SqlClient", "System.Data.EntityClient" };
      ViewBag.providers = providers;
      var cs = new JavaScriptSerializer().Serialize(ScUtilities.ReadConnStringsFromWebConfig());
      ViewBag.connStrings = MvcHtmlString.Create(cs);
      ViewBag.connStringsEncrypted = ScUtilities.ConnStringsAreEncrypted();
      ViewBag.customErrorsMode = ScUtilities.GetCustomErrorsMode().ToString();
      return View();
    }

    public ContentResult AddConnectionString(string stringName, string connectionString, string providerName)
    {
      string status;
      switch (ScUtilities.AddConnectionString(stringName, connectionString, providerName))
      {
        case ScUtilities.OpStatus.Success:
          status = "The " + stringName + " connection string SUCCESSFULLY added. :-)";
          break;
        default:
          status = "There was a PROBLEM adding the " + stringName + " connection string. :-(";
          break;
      }
      return Content(status);
    }

    // Called via AjaxLink
    public JsonResult EncryptConnectionStrings()
    {
      string status;
      switch (ScUtilities.EncryptDecryptConnectionStrings(true))
      {
        case ScUtilities.OpStatus.Success:
          status = "Connection strings were SUCCESSFULLY encrypted :-).";
          break;
        case ScUtilities.OpStatus.NoAction:
          status = "Connection strings were ALREADY encrypted.";
          break;
        default:
          status = "There was a PROBLEM encrypting the connection strings.";
          break;
      }
      var areEncrypted = ScUtilities.ConnStringsAreEncrypted();
      return Json(new { status, areEncrypted }, JsonRequestBehavior.AllowGet);
    }

    // Called via AjaxLink
    public JsonResult DecryptConnectionStrings()
    {
      string status;
      switch (ScUtilities.EncryptDecryptConnectionStrings(false))
      {
        case ScUtilities.OpStatus.Success:
          status = "Connection strings were SUCCESSFULLY decrypted :-).";
          break;
        case ScUtilities.OpStatus.NoAction:
          status = "Connection strings were ALREADY decrypted.";
          break;
        default:
          status = "There was a PROBLEM decrypting the connection strings.";
          break;
      }
      var areEncrypted = ScUtilities.ConnStringsAreEncrypted();
      return Json(new { status, areEncrypted }, JsonRequestBehavior.AllowGet);
    }

    // called from javascript.
    public JsonResult GetConnStrings()
    {
      return Json(ScUtilities.ReadConnStringsFromWebConfig(), JsonRequestBehavior.AllowGet);
    }

    public struct ConnString
    {
      public bool isNew, _destroy;
      public string name, connString, provider;
    }

    [HttpPost]
    public JsonResult ChangeCustomErrorsMode(string newMode)
    {
      var mode = (CustomErrorsMode)Enum.Parse(typeof(CustomErrorsMode), newMode);
      var status = ScUtilities.ChangeCustomErrorsMode(mode).ToString();
      var currentMode = ScUtilities.CurrentCustomErrorsMode().ToString();
      //return Json(new { status, currentMode }, JsonRequestBehavior.AllowGet);
      return Json(new {status, currentMode});
    }

    [HttpPost]
    public JsonResult SaveConnectionStrings([FromJson] ConnString[] connStrings, bool updateInitTbl)
    {
      var opStatus = ScUtilities.OpStatus.NoAction;
      try
      {
        foreach (var cs in connStrings)
        {
          if (cs.isNew && !cs._destroy)  // add new cs
            opStatus = ScUtilities.AddConnectionString(cs.name, cs.connString, cs.provider, updateInitTbl);
          else if (cs._destroy && !cs.isNew) // delete old one
            opStatus = ScUtilities.DeleteConnectionString(cs.name, updateInitTbl);
          else // update cstrings if needed
            opStatus = ScUtilities.AddOrUpdateConnectionString(cs.name, cs.connString, cs.provider, updateInitTbl);
        }
      }
      catch
      {
        opStatus = ScUtilities.OpStatus.Error;
      }

      string status;
      switch (opStatus)
      {
        case ScUtilities.OpStatus.Success:
          status = "Connection Strings were successfully saved!";
          break;
        case ScUtilities.OpStatus.NoAction:
          status = "No Connection String needed modification.";
          break;
        default:
          status = "There was a problem modifying the connection strings.";
          break;
      }
      return Json(new { status });
    }
  }
}
