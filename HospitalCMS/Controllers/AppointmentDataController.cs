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
    public class AppointmentDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/AppointmentData/ListAppointments
        [HttpGet]
        public IEnumerable<AppointmentDto> ListAppointments()
        {
            List<Appointment> appointments = db.Appointments.Include(x=>x.Doctor).Include(y=>y.Patient).ToList();
            List<AppointmentDto> appointmentDtos = new List<AppointmentDto>();

            appointments.ForEach(x => appointmentDtos.Add(new AppointmentDto()
            {
                AppointmentId = x.AppointmentId,
                DoctorId = x.DoctorId,
                PatientId = x.PatientId,
                Schedule = x.Schedule,
                DoctorNotes = x.DoctorNotes,
                Doctor = x.Doctor != null ? new DoctorDto() { Name = x.Doctor.Name }: null,
                Patient = x.Patient != null ? new PatientDto() { FirstName = x.Patient.FirstName}: null
            })); ;

            return appointmentDtos;
        }

        // GET: api/AppointmentData/FindAppointmentsByEmail/5
        [ResponseType(typeof(Appointment))]
        [HttpGet]
        public IEnumerable<AppointmentDto> FindAppointmentsByEmail(string email)
        {
            List<Appointment> appointments = db.Appointments.Include(x => x.Doctor).Include(y=>y.Patient).Where(x => x.Patient.Email == email).ToList();
            //Appointment appointment = db.Appointments.Find(id);
            List<AppointmentDto> appointmentDtos = new List<AppointmentDto>();

            appointments.ForEach(x => appointmentDtos.Add(new AppointmentDto()
            {
                AppointmentId = x.AppointmentId,
                DoctorId = x.DoctorId,
                PatientId = x.PatientId,
                Schedule = x.Schedule,
                DoctorNotes = x.DoctorNotes

            }));

            return appointmentDtos;
        }

        [ResponseType(typeof(Appointment))]
        [HttpGet]
        public IHttpActionResult FindAppointment(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            //Appointment appointment = db.Appointments.Find(id);
            AppointmentDto appointmentDto = new AppointmentDto();

            appointmentDto = new AppointmentDto()
            {
                AppointmentId = appointment.AppointmentId,
                DoctorId = appointment.DoctorId,
                PatientId = appointment.PatientId,
                Schedule = appointment.Schedule,
                DoctorNotes = appointment.DoctorNotes,
                Doctor = appointment.Doctor != null ? new DoctorDto() { Name = appointment.Doctor.Name } : null,
                Patient = appointment.Patient != null ? new PatientDto() {
                    FirstName = appointment.Patient.FirstName,
                    MaritalStatus = appointment.Patient.MaritalStatus,
                    Address1 = appointment.Patient.Address1,
                    Address2 = appointment.Patient.Address2,
                    City = appointment.Patient.City,
                    Country = appointment.Patient.Country,
                    DOB = appointment.Patient.DOB,
                    Email = appointment.Patient.Email,
                    Gender = appointment.Patient.Gender,
                    LastName = appointment.Patient.LastName,
                    Mobile = appointment.Patient.Mobile,
                    PatientId = appointment.Patient.PatientId,
                    PostalCode = appointment.Patient.PostalCode
                } : null

            };

            return Ok(appointmentDto);
        }

        // PUT: api/AppointmentData/UpdateAppointment/5
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateAppointment(int id, Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != appointment.AppointmentId)
            {
                return BadRequest();
            }

            db.Entry(appointment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
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

        // POST: api/AppointmentData/AddAppointment
        [ResponseType(typeof(Appointment))]
        [HttpPost]
        public IHttpActionResult AddAppointment(Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Appointments.Add(appointment);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = appointment.AppointmentId }, appointment);
        }

        // DELETE: api/AppointmentData/DeleteAppointment/5
        [ResponseType(typeof(Appointment))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult DeleteAppointment(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return NotFound();
            }

            db.Appointments.Remove(appointment);
            db.SaveChanges();

            return Ok(appointment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AppointmentExists(int id)
        {
            return db.Appointments.Count(e => e.AppointmentId == id) > 0;
        }
    }
}