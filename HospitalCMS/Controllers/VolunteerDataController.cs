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
    public class VolunteerDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [HttpGet]
        // GET: api/VolunteerData/ListVolunteers
        public IHttpActionResult ListVolunteers()
        {
            List<Volunteer> Volunteers = db.Volunteers.ToList();
            List<VolunteerDto> VolunteersDto = new List<VolunteerDto>();

            Volunteers.ForEach(a => VolunteersDto.Add(new VolunteerDto()
            {
                VolunteerId = a.VolunteerId,
                VolunteerName = a.VolunteerName,                
            }));
            return Ok(VolunteersDto);
        }

        // GET: api/VolunteerData/5
        [ResponseType(typeof(VolunteerDto))]
        public IHttpActionResult FindVolunteer(int id)
        {
            Volunteer volunteer = db.Volunteers.Find(id);
            VolunteerDto volunteerDto = new VolunteerDto()
            {
                VolunteerId = volunteer.VolunteerId,
                VolunteerName=volunteer.VolunteerName
              
            };
            if (volunteer == null)
            {
                return NotFound();
            }
            return Ok(volunteerDto);
        }

        // PUT: api/VolunteerData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVolunteer(int id, Volunteer volunteer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != volunteer.VolunteerId)
            {
                return BadRequest();
            }

            db.Entry(volunteer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VolunteerExists(id))
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

        // POST: api/VolunteerData
        [ResponseType(typeof(Volunteer))]
        public IHttpActionResult CreateVolunteer(Volunteer volunteer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Volunteers.Add(volunteer);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = volunteer.VolunteerId }, volunteer);
        }

        // DELETE: api/VolunteerData/5
        [ResponseType(typeof(Volunteer))]
        public IHttpActionResult DeleteVolunteer(int id)
        {
            Volunteer volunteer = db.Volunteers.Find(id);
            if (volunteer == null)
            {
                return NotFound();
            }

            db.Volunteers.Remove(volunteer);
            db.SaveChanges();

            return Ok(volunteer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VolunteerExists(int id)
        {
            return db.Volunteers.Count(e => e.VolunteerId == id) > 0;
        }
    }
}