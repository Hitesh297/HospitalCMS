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
    public class PatientDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        [HttpGet]
        [ResponseType(typeof(PatientDto))]

        /// <summary>
        /// Returns all Patient in the system
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Patient in the database
        /// </returns>
        /// <example>
        // GET: api/PatientData/ListPatients
        /// </example>
        
        public IHttpActionResult ListPatients()
        {
            List<Patient> Patient = db.Patients.ToList();
            List<PatientDto> PatientDto = new List<PatientDto>();

            Patient.ForEach(a => PatientDto.Add(new PatientDto()
            {
                PatientId = a.PatientId,
                FirstName = a.FirstName,
                LastName = a.LastName,
                DOB = a.DOB,
                Gender = a.Gender,
                Mobile = a.Mobile,
                Email = a.Email,
                MaritalStatus = a.MaritalStatus,
                Address1 = a.Address1,
                Address2 = a.Address2,
                City = a.City,
                Country = a.Country,
                PostalCode = a.PostalCode
            }));
            return Ok(PatientDto);
        }

        [HttpGet]
        [ResponseType(typeof(Patient))]

        /// <summary>
        /// Returns details of the Patient by Patient id
        /// </summary>
        /// <param name="id">Patient primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: Patient in the system matching up to the Patient ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        // GET: api/PatientData/FindPatient/1
        /// </example>
        
        public IHttpActionResult FindPatient(int id)

        {
            Patient Patient = db.Patients.Find(id);
            PatientDto PatientDto = new PatientDto()
            {
                PatientId = Patient.PatientId,
                FirstName = Patient.FirstName,
                LastName = Patient.LastName,
                DOB = Patient.DOB,
                Gender = Patient.Gender,
                Mobile = Patient.Mobile,
                Email = Patient.Email,
                MaritalStatus = Patient.MaritalStatus,
                Address1 = Patient.Address1,
                Address2 = Patient.Address2,
                City = Patient.City,
                Country = Patient.Country,
                PostalCode = Patient.PostalCode
            };
            if (Patient == null)
            {
                return NotFound();
            }

            return Ok(PatientDto);
        }


        [HttpGet]
        [ResponseType(typeof(Patient))]

        /// <summary>
        /// Returns details of the Patient by Email id
        /// </summary>
        /// <param name="id">Patient email id</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: Patient in the system matching up to the email id
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        // GET: api/PatientData/FindPatientByEmail/hites.297@gmail.com
        /// </example>
        [Route("api/PatientData/FindPatientByEmail/{email}")]
        public IHttpActionResult FindPatientByEmail(string email)

        {
            Patient Patient = db.Patients.Where(x => x.Email == email).FirstOrDefault();
            if (Patient == null)
            {
                return NotFound();
            }

            PatientDto PatientDto = new PatientDto()
            {
                PatientId = Patient.PatientId,
                FirstName = Patient.FirstName,
                LastName = Patient.LastName,
                DOB = Patient.DOB,
                Gender = Patient.Gender,
                Mobile = Patient.Mobile,
                Email = Patient.Email,
                MaritalStatus = Patient.MaritalStatus,
                Address1 = Patient.Address1,
                Address2 = Patient.Address2,
                City = Patient.City,
                Country = Patient.Country,
                PostalCode = Patient.PostalCode
            };
           

            return Ok(PatientDto);
        }

        /// <summary>
        /// Edit a particular Patient in the system with POST Data input
        /// </summary>
        /// <param name="id">Patient primary key</param>
        /// <param name="Patient">JSON form data of Patient</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        // PUT: api/PatientData/EditPatient/5
        /// FORM DATA: Patient JSON Object
        /// </example>
        [HttpPost]
        
        [ResponseType(typeof(void))]
        public IHttpActionResult EditPatient(int id, Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != patient.PatientId)
            {
                return BadRequest();
            }

            db.Entry(patient).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
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
        /// Create Patient to the system
        /// </summary>
        /// <param name="Patient">JSON form data of Patient</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT:Patient ID
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        // POST: api/PatientData/AddPatient
        /// </example>
        [HttpPost]
        
        [ResponseType(typeof(Patient))]
        public IHttpActionResult AddPatient(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Patients.Add(patient);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = patient.PatientId }, patient);
        }

        /// <summary>
        /// Deletes an article from the system by it's ID.
        /// </summary>
        /// <param name="id">primary key of article</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        // DELETE: api/PatientData/DeletePatient/5
        /// FORM DATA: (empty)
        /// </example>
        [HttpPost]
        
        [ResponseType(typeof(Patient))]
        [Authorize]
        public IHttpActionResult DeletePatient(int id)
        {
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return NotFound();
            }

            db.Patients.Remove(patient);
            db.SaveChanges();

            return Ok(patient);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PatientExists(int id)
        {
            return db.Patients.Count(e => e.PatientId == id) > 0;

        }
    }
}
  


       