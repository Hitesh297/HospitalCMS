using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using HospitalCMS.Models;

namespace HospitalCMS.Controllers
{
    public class DoctorDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Doctors in the system
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Doctors in the database
        /// </returns>
        /// <example>
        //  GET: api/DoctorData/ListDoctors
        /// </example>
        
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

        /// <summary>
        /// Returns all Doctors by speciality in the system
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Doctors by speciality in the database
        /// </returns>
        /// <example>
        //  GET: api/DoctorData/ListDoctorsBySpeciality/1
        /// </example>
        
        [HttpGet]
        [ResponseType(typeof(DoctorDto))]
        public IHttpActionResult ListDoctorsBySpeciality(int id)
        {
            List<Doctor> Doctors = db.Doctors.Include(x=>x.Specialities).Where(y=>y.Specialities.Any(x=>x.SpecialityId == id)).ToList();
            List<DoctorDto> doctorDtos = new List<DoctorDto>();

            Doctors.ForEach(a => doctorDtos.Add(new DoctorDto()
            {
                DoctorId = a.DoctorId,
                Name = a.Name,
                Experience = a.Experience,
                Phone = a.Phone,
                Email = a.Email,
            }));
            return Ok(doctorDtos);
        }

        /// POST api/DoctorData/unassociatespecialityewithDoctor/1/2
        /// </example>
        [HttpPost]
        [Route("api/DoctorData/UnassociatespecialityewithDoctor/{doctorId}/{specialityId}")]
        public IHttpActionResult UnassociatespecialityewithDoctor(int doctorId, int specialityId)
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
        [Route("api/DoctorData/AssociatespecialitywithDoctor/{doctorId}/{specialityId}")]
        public IHttpActionResult AssociatespecialitywithDoctor(int doctorId, int specialityId)
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


        /// <summary>
        /// Returns details of the doctors by doctor id
        /// </summary>
        /// <param name="id">doctor primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: doctor in the system matching up to the doctors ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// GET: api/DoctorData/FindDoctor/5
        /// </example>
        
        [ResponseType(typeof(Doctor))]
        [HttpGet]

        public IHttpActionResult FindDoctor(int id)
        {
            Doctor doctor = db.Doctors.Include(x=>x.Specialities).Include(x => x.Appointments).FirstOrDefault(y=>y.DoctorId == id);
            DoctorDto DoctorDto = new DoctorDto()
            {
                DoctorId = doctor.DoctorId, 
                Name = doctor.Name,
                Experience= doctor.Experience,
                Phone=doctor.Phone,
                Email=doctor.Email,
                DoctorHasPic = doctor.DoctorHasPic,
                PicExtension = doctor.PicExtension
            };

            if (doctor.Specialities != null && doctor.Specialities.Count() != 0)
            {
                DoctorDto.Specialities = new List<SpecialityDto>();
                foreach (var speciality in doctor.Specialities)
                {
                    DoctorDto.Specialities.Add(new SpecialityDto() { SpecialityId = speciality.SpecialityId, Name = speciality.Name });
                }
            }

            if (doctor.Appointments != null && doctor.Appointments.Count() != 0)
            {
                DoctorDto.Appointments = new List<AppointmentDto>();
                foreach (var appointment in doctor.Appointments)
                {
                    DoctorDto.Appointments.Add(new AppointmentDto() { 
                        Schedule = appointment.Schedule, 
                        AppointmentId = appointment.AppointmentId, 
                        Patient = new PatientDto() { 
                            FirstName = appointment.Patient.FirstName, 
                            LastName = appointment.Patient.LastName 
                        } 
                    });
                }
            }

            if (doctor == null)
            {
                return NotFound();
            }

            return Ok(DoctorDto);
        }

        /// <summary>
        /// Upload a doctor picture to the system
        /// </summary>
        /// <param name="doctor">JSON form data of doctor</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: doctor ID
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/DoctorData/UpdateDoctor/5
        /// FORM DATA: doctor JSON Object
        /// </example>
        
        
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

        /// <summary>
        /// Add a doctor to the system
        /// </summary>
        /// <param name="doctor">JSON form data of doctor</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT:Article ID
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
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

        /// <summary>
        /// Upload a doctor picture to the system
        /// </summary>
        /// <param name="doctor">JSON form data of doctor</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: doctor ID
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
       // POST: api/doctorData/UploadDoctorPic/{id}
        /// </example>
        
        [HttpPost]
        public IHttpActionResult UploadDoctorPic(int id)
        {

            bool haspic = false;
            string picextension;
            if (Request.Content.IsMimeMultipartContent())
            {

                int numfiles = HttpContext.Current.Request.Files.Count;

                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var employeePic = HttpContext.Current.Request.Files[0];
                    if (employeePic.ContentLength > 0)
                    {
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(employeePic.FileName).Substring(1);
                        if (valtypes.Contains(extension, StringComparer.InvariantCultureIgnoreCase))
                        {
                            try
                            {
                                string fn = id + "." + extension;
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/Doctors/"), fn);

                                employeePic.SaveAs(path);

                                haspic = true;
                                picextension = extension;

                                Doctor Selecteddoctor = db.Doctors.Find(id);
                                Selecteddoctor.DoctorHasPic = haspic;
                                Selecteddoctor.PicExtension = extension;
                                db.Entry(Selecteddoctor).State = EntityState.Modified;

                                db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                return BadRequest();
                            }
                        }
                    }

                }
                else
                {
                    Doctor SelectedDoctor = db.Doctors.Find(id);
                    SelectedDoctor.DoctorHasPic = haspic;
                    SelectedDoctor.PicExtension = null;
                    db.Entry(SelectedDoctor).State = EntityState.Modified;

                    db.SaveChanges();
                }

                return Ok();
            }
            else
            {
                //not multipart form data
                return BadRequest();

            }

        }

         /// <summary>
        /// Deletes a doctor from the system by it's ID.
        /// </summary>
        /// <param name="id">primary key of doctor</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        // DELETE: api/DoctorData/DeleteDoctor/5
        /// FORM DATA: (empty)
        /// </example>
        
        [ResponseType(typeof(Doctor))]
        [HttpPost]
        [Authorize]
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