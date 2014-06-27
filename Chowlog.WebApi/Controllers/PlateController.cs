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

namespace Chowlog.WebApi.Controllers
{
    public class PlateController : ApiController
    {
        private ChowlogDb db = new ChowlogDb();

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
        [ResponseType(typeof(Plate))]
        [Authorize]
        public IHttpActionResult PostPlate(Plate plate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Plates.Add(plate);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PlateExists(plate.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = plate.Id }, plate);
        }

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