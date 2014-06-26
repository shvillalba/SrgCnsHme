using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using SrgCnsHme.Infrastructure;
using SrgCnsHme.Infrastructure.WeightChrt;
using SrgCnsHme.Models;

namespace SrgCnsHme.Controllers
{
  [Authorize(Users = "Sergio, Consuelo")]
  public class WeightsController : Controller
  {
    private const int NumInTable = 10;
    private const int NumInChart = 30;
    readonly scContext _db = new scContext();
    private const string DateFrmt = "{0:d-MMM-yyyy}";
    readonly CultureInfo _provider = CultureInfo.InvariantCulture;
    readonly DateTime _defaultFromDate = DateTime.Now.AddDays(-NumInChart + 1);
    private readonly DateTime _defaultStartTableDate = DateTime.Now.AddDays(-NumInTable + 1);
    readonly DateTime _defaultToDate = DateTime.Now;

    List<Weight> WeightsInServerFormat(string person, DateTime? from = null, DateTime? to = null)
    {
      var fromDte = from ?? _defaultFromDate;
      var toDte = to ?? _defaultToDate;
      return _db.Weights.Where(w => w.Person.ShortName == person
        && EntityFunctions.TruncateTime(w.Date) >= EntityFunctions.TruncateTime(fromDte)
        && EntityFunctions.TruncateTime(w.Date) <= EntityFunctions.TruncateTime(toDte))
        .OrderByDescending(w => EntityFunctions.TruncateTime(w.Date)).ToList();
    }

    IEnumerable<object> WeightsInClientFormat(string name, DateTime? from = null, DateTime? to = null)
    {
      return WeightsInServerFormat(name, from, to).Select(w => new { id = w.WeightID, name, date = String.Format(DateFrmt, w.Date), weight = w.WeightInLb });
    }

    public JsonResult PageInfo()
    {
      return Json(new
        {
          GetWeightChartUrl = Url.Action("GetWeightChart"),
          AddNewWeightUrl = Url.Action("AddNewWeight"),
          DelWeightUrl = Url.Action("DelWeight"),
          GetWeightsUrl = Url.Action("GetWeights"),
          SaveWeightsUrl = Url.Action("SaveWeights"),
          DefaultFromDate = String.Format(DateFrmt, _defaultFromDate),
          DefaultToDate = String.Format(DateFrmt, DateTime.Now),
          OldestWtDate = String.Format(DateFrmt, _db.Weights.OrderBy(w => w.Date).Select(w => w.Date).First()),
          StartTableDate = String.Format(DateFrmt, _defaultStartTableDate),
          SrgWeights = WeightsInClientFormat("Sergio", _defaultStartTableDate),
          CslWeights = WeightsInClientFormat("Consuelo", _defaultStartTableDate)
        });
    }

    public ActionResult Index()
    {
      return View("WeightChart");
    }

    public ActionResult GetWeightChart(string person, string fromDate, string toDate)
    {
      //var weightList = db.Weights.Where(w => w.Person.ShortName == "Sergio").OrderByDescending(w => w.Date).Take(50).ToList();
      //weightList.Reverse(); //LEARN: ??? LINQ question: find out why if you just attach this "Reverse" to the line above, the retrieved set is empty.

      var fromDte = DateTime.ParseExact(fromDate, "d-MMM-yyyy", _provider);
      var toDte = DateTime.ParseExact(toDate, "d-MMM-yyyy", _provider);
      var wChrt = new WeightChrt();

      switch (person)
      {
        case "Sergio":
          wChrt.AddWeights(WeightsInServerFormat("Sergio", fromDte, toDte));
          break;
        case "Consuelo":
          wChrt.AddWeights(WeightsInServerFormat("Consuelo", fromDte, toDte));
          break;
        default:
          wChrt.AddWeights(WeightsInServerFormat("Sergio", fromDte, toDte));
          wChrt.AddWeights(WeightsInServerFormat("Consuelo", fromDte, toDte));
          break;
      }

      return File(wChrt.GenerateChart().ToArray(), "image/png", "mychart.png");
    }

    [HttpPost]
    public JsonResult AddNewWeight(string name, string date, float weight)
    {
      try
      {
        var personId = name.ToPersonId();
        if (personId < 0) return Json(new { status = "no such person" });
        var dt = DateTime.ParseExact(date, "d-MMM-yyyy", _provider);
        var wt = new Weight { PersonID = personId, Date = dt, WeightInLb = weight };
        ValidateModel(wt);
        _db.Weights.Add(wt);
        _db.SaveChanges();
        return Json(new { status = "success" });
      }
      catch (Exception)
      {
        return Json(new { status = "failure" });
      }
    }

    [HttpPost]
    public JsonResult DelWeight(string name, string date, float weight)
    {
      try
      {
        var personId = name.ToPersonId();
        var dt = DateTime.ParseExact(date, "d-MMM-yyyy", _provider);
        if (personId < 0) return Json(new { status = "no such person" });
        var wt = _db.Weights.Single(w => w.PersonID == personId && w.Date == dt && w.WeightInLb.CompareTo(weight) == 0);
        _db.Weights.Remove(wt);
        _db.SaveChanges();
        return Json(new { status = "success" });
      }
      catch (Exception)
      {
        return Json(new { status = "failure" });
      }
    }

    [HttpPost]
    public JsonResult GetWeights()
    {
      var srgWeights = WeightsInClientFormat("Sergio", DateTime.Now.AddDays(-10));
      var cslWeights = WeightsInClientFormat("Consuelo", DateTime.Now.AddDays(-10));
      return Json(new { srgWeights, cslWeights });
    }

    [HttpPost]
    public JsonResult SaveWeights([FromJson] Weight[] weightsToDel, [FromJson] Weight[] weightsToEdit)
    {
      try
      {
        int numDeleted = 0, numEdited = 0;
        foreach (var wt in weightsToDel.Select(weight => _db.Weights.Find(weight.WeightID)))
        {
          _db.Weights.Remove(wt);
          numDeleted++;
        }

        foreach (var weight in weightsToEdit)
        {
          var wt = _db.Weights.Find(weight.WeightID);
          wt.WeightInLb = weight.WeightInLb;
          numEdited++;
        }
        _db.SaveChanges();
        return Json(new { status = numEdited + " weight(s) changed, and " + numDeleted + " weight(s) deleted" });
      }
      catch (Exception)
      {
        return Json(new { status = "There were errors submitting weight changes." });
      }
    }
  }
}
