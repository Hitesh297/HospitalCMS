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
using HospitalCMS.Models;

namespace HospitalCMS.Controllers
{
    public class SpecialityDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/SpecialityData
        public IQueryable<Speciality> GetSpecialities()
        {
            return db.Specialities;
        }

        // GET: api/SpecialityData/5
        [ResponseType(typeof(Speciality))]
        public IHttpActionResult GetSpeciality(int id)
        {
            Speciality speciality = db.Specialities.Find(id);
            if (speciality == null)
            {
                return NotFound();
            }

            return Ok(speciality);
        }

        // PUT: api/SpecialityData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSpeciality(int id, Speciality speciality)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != speciality.SpecialityId)
            {
                return BadRequest();
            }

            db.Entry(speciality).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpecialityExists(id))
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

        // POST: api/SpecialityData
        [ResponseType(typeof(Speciality))]
        public IHttpActionResult PostSpeciality(Speciality speciality)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Specialities.Add(speciality);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = speciality.SpecialityId }, speciality);
        }

        // DELETE: api/SpecialityData/5
        [ResponseType(typeof(Speciality))]
        public IHttpActionResult DeleteSpeciality(int id)
        {
            Speciality speciality = db.Specialities.Find(id);
            if (speciality == null)
            {
                return NotFound();
            }

            db.Specialities.Remove(speciality);
            db.SaveChanges();

            return Ok(speciality);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SpecialityExists(int id)
        {
            return db.Specialities.Count(e => e.SpecialityId == id) > 0;
        }
    }
}