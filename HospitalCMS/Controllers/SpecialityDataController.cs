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

        // GET: api/SpecialityData/SpecialityAssignedToDoctor/2
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