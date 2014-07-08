using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Chowlog.Entities;
using Chowlog.Web.DataContexts;
using Microsoft.AspNet.Identity;
using Chowlog.Web.ViewModels;
using System.Threading.Tasks;
using System.Web;
using System.Diagnostics;
using Chowlog.Web.App_Code;
using System.IO;

namespace Chowlog.Web.Api.Controllers
{
    public class PlateController : ApiController
    {
        private ChowlogDb db = new ChowlogDb();

        private IFileUploadService fileUploadService;

        public PlateController(IFileUploadService fileUploadService)
        {
            this.fileUploadService = fileUploadService;
        }

        // GET api/Plate
        [Authorize]
        public IQueryable<Plate> GetPlates()
        {
            return db.Plates;
        }

        // GET api/Plate/5
        [ResponseType(typeof(Plate))]
        public IHttpActionResult GetPlate(Guid id)
        {
            Plate plate = db.Plates.Find(id);
            if (plate == null)
            {
                return NotFound();
            }

            return Ok(plate);
        }

        // PUT api/Plate/5
        public IHttpActionResult PutPlate(Guid id, Plate plate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != plate.Id)
            {
                return BadRequest();
            }

            db.Entry(plate).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlateExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Plate


        public async Task<HttpResponseMessage> PostFormData()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                // This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData)
                {
                    Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                    Trace.WriteLine("Server file path: " + file.LocalFileName);
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpPost, Route("api/upload")]
        public async Task<IHttpActionResult> Upload()
        {
            if (!Request.Content.IsMimeMultipartContent())
                throw new Exception(); // divided by zero

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);
            foreach (var file in provider.Contents)
            {
                var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                var buffer = await file.ReadAsByteArrayAsync();

                var plateId = Guid.NewGuid();
                var fileName = fileUploadService.UploadFile(buffer, plateId.ToString() + Path.GetExtension(filename));
                var dateTaken = Utils.GetDateTaken(buffer);

                var plate = new Plate()
                {
                    HasPicture = true,
                    Extension = Path.GetExtension(fileName),
                    Id = plateId,
                    TimeEaten = dateTaken,
                    //Title = model.Title,
                    UserId = Guid.Parse(User.Identity.GetUserId())
                };
                //model.Id = Guid.NewGuid();
                db.Plates.Add(plate);
                db.SaveChanges();

            }

            return Ok();
        }

        //[ResponseType(typeof(Plate))]
        //public IHttpActionResult PostPlate(PlateCreateViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }


        //    var plate = new Plate()
        //    {
        //        //HasPicture = model.files.Count() > 0,
        //        //Extension = Path.GetExtension(fileName),
        //        //Id = plateId,
        //        TimeEaten = model.TimeEaten,
        //        Title = model.Title,
        //        UserId = Guid.Parse(User.Identity.GetUserId())
        //    };


        //    db.Plates.Add(plate);

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (PlateExists(plate.Id))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtRoute("DefaultApi", new { id = plate.Id }, plate);
        //}

        // DELETE api/Plate/5
        [ResponseType(typeof(Plate))]
        public IHttpActionResult DeletePlate(Guid id)
        {
            Plate plate = db.Plates.Find(id);
            if (plate == null)
            {
                return NotFound();
            }

            db.Plates.Remove(plate);
            db.SaveChanges();

            return Ok(plate);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlateExists(Guid id)
        {
            return db.Plates.Count(e => e.Id == id) > 0;
        }
    }
}