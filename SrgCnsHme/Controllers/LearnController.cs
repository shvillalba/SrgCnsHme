using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using SrgCnsHme.Infrastructure;
using SrgCnsHme.Models;

namespace SrgCnsHme.Controllers
{
  public class LearnController : Controller
  {
    readonly DefaultEfContext _dec = new DefaultEfContext();

    public ActionResult Index()
    {
      return View();
    }

    public ActionResult LinqSelectMany()
    {
      return View();
    }

    public ActionResult KnockoutTraining1()
    {
      return View();
    }

    public ActionResult KnockoutTraining2()
    {
      return View();
    }

    #region Chart Sample

    public ActionResult MakeChartSample()
    {
      var cg = new ChartGen();
      const int id = 1; //dummy var for now.
      var ms = cg.GenerateChart(id);

      return File(ms.ToArray(), "image/png", "mychart.png");
    }

    public ActionResult ChartSample()
    {
      return View();
    }

    #endregion

    public ActionResult KnockoutTraining3()
    {
      return View();
    }

    public ActionResult KnockoutTraining4()
    {
      return View();
    }

    public ActionResult KnockoutTraining5()
    {
      //LEARN: Passes test data to the client page. Notice use of MvcHtmlString to avoid encoding the json.
      //  var tasks = new[] { new LrnKoTask { title = "task1", isDone = false }, new LrnKoTask { title = "task2", isDone = true } };
      //  //ViewBag.tasks = MvcHtmlString.Create("[{ title: 'first task', isDone: false }, { title: 'second task', isDone: true }]"); //tasks;
      //  ViewBag.tasks = MvcHtmlString.Create(new JavaScriptSerializer().Serialize(tasks));
      return View();
    }

    public JsonResult GetTasks()
    {
      //var tasks = new[] { new LrnKoTask { title = "task1", isDone = false }, new LrnKoTask { title = "task2", isDone = true } }; //test-data
      var tasks = _dec.LrnKoTasks;
      return Json(tasks, JsonRequestBehavior.AllowGet);
    }

    public ActionResult SaveTasksToDb([FromJson] LrnKoTask[] tasks)
    {
      TempData["OpStatus"] = "Tasks were successfully saved via form-post!";
      try
      {
        ModifyTasksInDb(tasks);
        _dec.SaveChanges();
      }
      catch
      {
        TempData["OpStatus"] = "There was a problem saving the tasks via form-post!!!!";
      }
      return RedirectToAction("KnockoutTraining5");
    }

    [HttpPost]
    public JsonResult SaveTasksToDbViaAjax([FromJson] LrnKoTask[] tasks)
    {
      var rslt = "Tasks were successfully saved via Ajax!";
      try
      {
        ModifyTasksInDb(tasks);
        _dec.SaveChanges();
      }
      catch (Exception ex)
      {
        rslt = "There was a problem saving the tasks via Ajax!!!!: " + ex.Message;
      }
      return Json(rslt, JsonRequestBehavior.AllowGet);
    }

    void ModifyTasksInDb(IEnumerable<LrnKoTask> tasks)
    {
      foreach (var tsk in tasks)
      {
        if (tsk._destroy) // "delete"; 
        {
          if (tsk.id > 0) //don't try to delete from db a new task that was created & destroyed right away at the client-side (i.e.: a mistake)
          {
            var t = _dec.LrnKoTasks.Find(tsk.id);
            _dec.LrnKoTasks.Remove(t);
          }
        }
        else // "edit" and "add"
        {
          ValidateModel(tsk);
          if (tsk.id < 0)
            _dec.LrnKoTasks.Add(tsk);
          else
            _dec.Entry(tsk).State = EntityState.Modified;
        }
      }
    }

    public ActionResult CarmenLibretto()
    {
      return View();
    }
  }
}