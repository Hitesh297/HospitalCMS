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

        /// <summary>
        /// Returns all specialities in the system
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all specialities in the database
        /// </returns>
        /// <example>
        // GET: api/SpecialityData/ListSpecialities
        /// </example>
        
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

        /// <summary>
        /// Returns details of the Speciality by Speciality id
        /// </summary>
        /// <param name="id">Speciality primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT:Speciality in the system matching up to the Speciality ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        // GET: api/SpecialityData/FindSpeciality/5
        /// </example>
        
        [ResponseType(typeof(Speciality))]
        [HttpGet]
        public IHttpActionResult FindSpeciality(int id)
        {
            Speciality speciality = db.Specialities.Include(x => x.Doctors).FirstOrDefault(y => y.SpecialityId == id);
            if (speciality == null)
            {
                return NotFound();
            }

            SpecialityDto specialityDto = new SpecialityDto()
            {
                SpecialityId = speciality.SpecialityId,
                Name = speciality.Name
            };

            if (speciality.Doctors != null && speciality.Doctors.Count() != 0)
            {
                specialityDto.Doctors = new List<DoctorDto>();
                foreach (var doctor in speciality.Doctors)
                {
                    specialityDto.Doctors.Add(new DoctorDto()
                    {
                        Name = doctor.Name,
                        DoctorHasPic = doctor.DoctorHasPic,
                        DoctorId = doctor.DoctorId,
                        Email = doctor.Email,
                        Experience = doctor.Experience,
                        Phone = doctor.Phone,
                        PicExtension = doctor.PicExtension
                    });
                }
            }

            return Ok(specialityDto);
        }

        /// <summary>
        /// Edit a particular Speciality in the system with POST Data input
        /// </summary>
        /// <param name="id">Speciality primary key</param>
        /// <param name="Speciality">JSON form data of Speciality</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
         // GET: api/SpecialityData/SpecialityAssignedToDoctor/2
        /// </example>
       
        [HttpGet]
        public IHttpActionResult SpecialityAssignedToDoctor(int id)
        {
            List<Speciality> specialities = db.Specialities.Where(x => x.Doctors.Any(y => y.DoctorId == id)).ToList();
            List<SpecialityDto> specialityDtos = new List<SpecialityDto>();

            specialities.ForEach(x => specialityDtos.Add(new SpecialityDto()
            {
                SpecialityId = x.SpecialityId,
                Name = x.Name
            }));
            return Ok(specialityDtos);
        }

        // GET: api/SpecialityData/SpecialityNotAssignedToDoctor/2
        [HttpGet]
        public IHttpActionResult SpecialityNotAssignedToDoctor(int id)
        {
            List<Speciality> specialities = db.Specialities.Where(x => !x.Doctors.Any(y => y.DoctorId == id)).ToList();
            List<SpecialityDto> specialityDtos = new List<SpecialityDto>();

            specialities.ForEach(x => specialityDtos.Add(new SpecialityDto()
            {
                SpecialityId = x.SpecialityId,
                Name = x.Name
            }));
            return Ok(specialityDtos);
        }

        /// <summary>
        /// Updates a particular Speciality in the system with POST Data input
        /// </summary>
        /// <param name="id">Speciality primary key</param>
        /// <param name="Speciality">JSON form data of Speciality</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        // PUT: api/SpecialityData/UpdateSpeciality/5
        /// </example>
        
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

         /// <summary>
        /// Create an Speciality to the system
        /// </summary>
        /// <param name="Speciality">JSON form data of Speciality</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT:Speciality ID
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        // POST: api/SpecialityData/CreateSpeciality
        /// </example>
        
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

        /// <summary>
        /// Deletes Speciality from the system by it's ID.
        /// </summary>
        /// <param name="id">primary key of Speciality</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        // DELETE: api/SpecialityData/DeleteSpeciality/5
        /// FORM DATA: (empty)
        /// </example>
        
        [ResponseType(typeof(Speciality))]
        [HttpPost]
        [Authorize]
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