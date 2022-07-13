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
    public class DoctorDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //  GET: api/DoctorData/ListDoctors
        [HttpGet]
        [ResponseType(typeof(DoctorDto))]
        public IHttpActionResult ListDoctors()
        {
            List<Doctor> Doctors = db.Doctors.ToList();
            List<DoctorDto> doctorDtos = new List<DoctorDto>();

            Doctors.ForEach(a => doctorDtos.Add(new DoctorDto()
            {
                DoctorId = a.DoctorId,
                Name = a.Name,
                Experience=a.Experience,
                Phone=a.Phone,
                Email=a.Email,
            }));
            return Ok(doctorDtos);
        }

        /// POST api/DoctorData/unassociatespecialityewithDoctor/1/2
        /// </example>
        [HttpPost]
        [Authorize]
        [Route("api/DoctorData/unassociatespecialityewithDoctor/{DoctorId}/{SpecialityId}")]
        public IHttpActionResult unassociatespecialityewithDoctor(int doctorId, int specialityId)
        {
            Doctor doctor = db.Doctors.Include(x => x.Specialities).Where(y => y.DoctorId == doctorId).FirstOrDefault();
            Speciality speciality = db.Specialities.Find(specialityId);

            if (doctor == null || speciality == null)
            {
                return NotFound();
            }

            doctor.Specialities.Remove(speciality);
            db.SaveChanges();

            return Ok();
        }

        /// POST api/DoctorData/associatespecialitywithDoctor/1/2
        /// </example>
        [HttpPost]
        [Authorize]
        [Route("api/DoctorData/associatespecialitywithDoctor/{DoctorId}/{SpecialityId}")]
        public IHttpActionResult associatespecialitywithDoctor(int doctorId, int specialityId)
        {
            Doctor doctor = db.Doctors.Include(x => x.Specialities).Where(y => y.DoctorId == doctorId).FirstOrDefault();
            Speciality speciality = db.Specialities.Find(specialityId);

            if (doctor == null || speciality == null)
            {
                return NotFound();
            }


            doctor.Specialities.Add(speciality);
            db.SaveChanges();

            return Ok();
        }


        /// GET: api/docotrData/FindDocotr/5
        /// </example>
        // GET: api/DoctorData/5
        [ResponseType(typeof(Doctor))]
        [HttpGet]

        public IHttpActionResult FindDoctor(int id)
        {
            Doctor doctor = db.Doctors.Find(id);
            DoctorDto DoctorDto = new DoctorDto()
            {
                DoctorId = doctor.DoctorId, 
                Name = doctor.Name,
                Experience= doctor.Experience,
                Phone=doctor.Phone,
                Email=doctor.Email
                

            };
            if (doctor == null)
            {
                return NotFound();
            }

            return Ok(doctor);
        }

        /// POST: api/DoctorData/UpdateDoctor/5
        /// FORM DATA: doctor JSON Object
        /// </example>
        // PUT: api/doctorData/5
        [ResponseType(typeof(void))]
        [HttpPost]
        /* [Authorize]*/
        public IHttpActionResult UpdateDoctor(int id, Doctor doctor)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != doctor.DoctorId)
            {
                return BadRequest();
            }

            db.Entry(doctor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(id))
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

        /// POST: api/doctorData/AddDoctor
        /// FORM DATA: Doctor JSON Object
        /// </example>

        // POST: api/DoctorData
        [ResponseType(typeof(Doctor))]
        [HttpPost]
        /* [Authorize]*/
        public IHttpActionResult AddDoctor(Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Doctors.Add(doctor);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = doctor.DoctorId }, doctor);
        }


        // GET: api/DoctorData
        public IQueryable<Doctor> GetDoctors()
        {
            return db.Doctors;
        }

        // GET: api/DoctorData/5
        [ResponseType(typeof(Doctor))]
        public IHttpActionResult GetDoctor(int id)
        {
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return NotFound();
            }

            return Ok(doctor);
        }

        // PUT: api/DoctorData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDoctor(int id, Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != doctor.DoctorId)
            {
                return BadRequest();
            }

            db.Entry(doctor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(id))
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

        // POST: api/DoctorData
        [ResponseType(typeof(Doctor))]
        public IHttpActionResult PostDoctor(Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Doctors.Add(doctor);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = doctor.DoctorId }, doctor);
        }

        // DELETE: api/DoctorData/5
        [ResponseType(typeof(Doctor))]
        [HttpPost]
        public IHttpActionResult DeleteDoctor(int id)
        {
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return NotFound();
            }

            db.Doctors.Remove(doctor);
            db.SaveChanges();

            return Ok(doctor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DoctorExists(int id)
        {
            return db.Doctors.Count(e => e.DoctorId == id) > 0;
        }
    }
}