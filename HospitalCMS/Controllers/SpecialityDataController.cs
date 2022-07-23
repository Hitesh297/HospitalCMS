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

        // GET: api/SpecialityData/ListSpecialities
        [HttpGet]
        public IHttpActionResult ListSpecialities()
        {
            List<Speciality> specialities = db.Specialities.ToList();
            List<SpecialityDto> specialityDtos = new List<SpecialityDto>();

            specialities.ForEach(a => specialityDtos.Add(new SpecialityDto()
            {
                SpecialityId = a.SpecialityId,
                Name = a.Name
            }));
            return Ok(specialityDtos);
        }

        // GET: api/SpecialityData/FindSpeciality/5
        [ResponseType(typeof(Speciality))]
        [HttpGet]
        public IHttpActionResult FindSpeciality(int id)
        {
            Speciality speciality = db.Specialities.Find(id);
            if (speciality == null)
            {
                return NotFound();
            }

            SpecialityDto specialityDto = new SpecialityDto()
            {
                SpecialityId = speciality.SpecialityId,
                Name = speciality.Name
            };

            return Ok(specialityDto);
        }

        // PUT: api/SpecialityData/UpdateSpeciality/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateSpeciality(int id, Speciality speciality)
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

        // POST: api/SpecialityData/CreateSpeciality
        [ResponseType(typeof(Speciality))]
        [HttpPost]
        public IHttpActionResult CreateSpeciality(Speciality speciality)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Specialities.Add(speciality);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = speciality.SpecialityId }, speciality);
        }

        // DELETE: api/SpecialityData/DeleteSpeciality/5
        [ResponseType(typeof(Speciality))]
        [HttpPost]
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