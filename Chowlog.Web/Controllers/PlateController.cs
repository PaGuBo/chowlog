using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Chowlog.Web.ViewModels;
using Chowlog.Web.App_Code;
using Chowlog.Entities;
using Microsoft.AspNet.Identity;
using System.IO;
using ExifLib;
using Chowlog.Web.DataContexts;


namespace Chowlog.Web.Controllers
{
    public class PlateController : Controller
    {
        private ChowlogDb db = new ChowlogDb();
        private IFileUploadService fileUploadService;

        public PlateController(IFileUploadService fileUploadService)
        {
            this.fileUploadService = fileUploadService;
        }
        // GET: /Plate/
        public ActionResult Index()
        {
            return View(db.Plates.ToList());
        }

        // GET: /Plate/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var platecreateviewmodel = db.Plates.Find(id);
            if (platecreateviewmodel == null)
            {
                return HttpNotFound();
            }
            return View(platecreateviewmodel);
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
        public ActionResult Create(PlateCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                foreach(var file in model.files)
                {
                    var plateId = Guid.NewGuid();
                    var fileName = fileUploadService.UploadFile(file, plateId.ToString() + Path.GetExtension(file.FileName));

                    var dateTaken = Utils.GetDateTaken(file.InputStream);





                    var plate = new Plate()
                    {
                        HasPicture = model.files.Count() > 0,
                        Extension = Path.GetExtension(fileName),
                        Id = plateId,
                        TimeEaten = dateTaken,
                        Title = model.Title,
                        UserId = Guid.Parse(User.Identity.GetUserId())
                    };
                    db.Plates.Add(plate);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: /Plate/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var platecreateviewmodel = db.Plates.Find(id);
            if (platecreateviewmodel == null)
            {
                return HttpNotFound();
            }
            return View(platecreateviewmodel);
        }

        // POST: /Plate/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,TimeEaten,Extension")] Plate plate)
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
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var platecreateviewmodel = db.Plates.Find(id);
            if (platecreateviewmodel == null)
            {
                return HttpNotFound();
            }
            return View(platecreateviewmodel);
        }

        // POST: /Plate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            var platecreateviewmodel = db.Plates.Find(id);
            db.Plates.Remove(platecreateviewmodel);
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
