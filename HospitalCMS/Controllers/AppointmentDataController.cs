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

        /// <summary>
        /// Returns all appointments in the system
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all appointments in the database
        /// </returns>
        /// <example>
        /// GET: api/AppointmentData/ListAppointments
        /// </example>

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

        /// <summary>
        /// Returns details of the appointment by appointment id
        /// </summary>
        /// <param name="id">Appointment primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An appointment in the system matching up to the appointment ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// GET: api/AppointmentData/FindAppointmentsByEmail/5
        /// </example>

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

        /// <summary>
        /// Returns Appointment details based on id
        /// </summary>
        /// <param name="id">Appointment primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An appointment in the system matching up to the appointment ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        // GET: api/AppointmentData/FindAppointment/5

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

        /// <summary>
        /// Updates a particular appointment in the system with POST Data input
        /// </summary>
        /// <param name="id">Appointment primary key</param>
        /// <param name="appointment">JSON form data of an Appointment</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// PUT: api/AppointmentData/UpdateAppointment/5
        /// FORM DATA: Appointment JSON Object
        /// </example>
       
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

        /// <summary>
        /// Adds an appointment to the system
        /// </summary>
        /// <param name="appointment">JSON form data of appointment</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Appointment ID, Appointment Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/AppointmentData/AddAppointment
        /// </example>
        
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

        /// <summary>
        /// Deletes an appointment from the system by it's ID.
        /// </summary>
        /// <param name="id">primary key of appointment</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// DELETE: api/AppointmentData/DeleteAppointment/5
        /// FORM DATA: (empty)
        /// </example>
        
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