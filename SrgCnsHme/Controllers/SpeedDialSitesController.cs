using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SrgCnsHme.Models;

namespace SrgCnsHme.Controllers
{
  public class SpeedDialSitesController : Controller
  {
    private scContext db = new scContext();

    // GET: /SpeedDialSites/
    public ActionResult Index()
    {
      return View(db.SpeedDialSites.OrderBy(s => s.order).ToList());
    }

    // GET: /SpeedDialSites/Details/5
    public ActionResult Details(int id = 0)
    {
      var speeddialsite = db.SpeedDialSites.Find(id);
      if (speeddialsite == null)
      {
        return HttpNotFound();
      }
      return View(speeddialsite);
    }

    // GET: /SpeedDialSites/Create
    public ActionResult Create()
    {
      return View();
    }

    // POST: /SpeedDialSites/Create
    [HttpPost]
    public ActionResult Create(SpeedDialSite speeddialsite)
    {
      if (ModelState.IsValid)
      {
        db.SpeedDialSites.Add(speeddialsite);
        db.SaveChanges();
        return RedirectToAction("Index");
      }

      return View(speeddialsite);
    }

    // GET: /SpeedDialSites/Edit/5
    public ActionResult Edit(int id = 0)
    {
      var speeddialsite = db.SpeedDialSites.Find(id);
      if (speeddialsite == null)
      {
        return HttpNotFound();
      }
      return View(speeddialsite);
    }

    // POST: /SpeedDialSites/Edit/5
    [HttpPost]
    public ActionResult Edit(SpeedDialSite speeddialsite)
    {
      if (ModelState.IsValid)
      {
        db.Entry(speeddialsite).State = EntityState.Modified;
        db.SaveChanges();
        return RedirectToAction("Index");
      }
      return View(speeddialsite);
    }

    // GET: /SpeedDialSites/Delete/5
    public ActionResult Delete(int id = 0)
    {
      var speeddialsite = db.SpeedDialSites.Find(id);
      if (speeddialsite == null)
      {
        return HttpNotFound();
      }
      return View(speeddialsite);
    }

    // POST: /SpeedDialSites/Delete/5
    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var speeddialsite = db.SpeedDialSites.Find(id);
      db.SpeedDialSites.Remove(speeddialsite);
      db.SaveChanges();
      return RedirectToAction("Index");
    }

    protected override void Dispose(bool disposing)
    {
      db.Dispose();
      base.Dispose(disposing);
    }
  }
}