using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Chowlog.Entities;
using Chowlog.Web.DataContexts;
using Microsoft.AspNet.Identity;

namespace Chowlog.Web.Controllers
{
    [Authorize]
    public class PlateController : Controller
    {
        private ChowlogDb db = new ChowlogDb();

        // GET: /Plate/
        public ActionResult Index()
        {
            var uid = new Guid(User.Identity.GetUserId());
            return View(db.Plates.Where(p => p.UserId == uid).ToList());
        }

        // GET: /Plate/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plate plate = db.Plates.Find(id);
            if (plate == null)
            {
                return HttpNotFound();
            }
            return View(plate);
        }

        // GET: /Plate/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Plate/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Title,TimeEaten")] Plate plate)
        {
            if (ModelState.IsValid)
            {
                plate.UserId = new Guid(User.Identity.GetUserId());
                db.Plates.Add(plate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(plate);
        }

        // GET: /Plate/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plate plate = db.Plates.Find(id);
            if (plate == null)
            {
                return HttpNotFound();
            }
            return View(plate);
        }

        // POST: /Plate/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Title,TimeEaten")] Plate plate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(plate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(plate);
        }

        // GET: /Plate/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plate plate = db.Plates.Find(id);
            if (plate == null)
            {
                return HttpNotFound();
            }
            return View(plate);
        }

        // POST: /Plate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Plate plate = db.Plates.Find(id);
            db.Plates.Remove(plate);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
