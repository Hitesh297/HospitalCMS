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
            List<Appointment> appointments = db.Appointments.ToList();
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

        // PUT: api/AppointmentData/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutAppointment(int id, Appointment appointment)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != appointment.AppointmentId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(appointment).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!AppointmentExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

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