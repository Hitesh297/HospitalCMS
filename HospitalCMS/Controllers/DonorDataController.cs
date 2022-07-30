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

    public class DonorDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [HttpGet]
        [ResponseType(typeof(DonorDto))]

        /// <summary>
        /// Returns all article in the system
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all article in the database
        /// </returns>
        /// <example>
        /// GET: api/ArticleData/ListArticles
        /// </example>
        // GET: api/DonorData/ListDonors
        public IHttpActionResult ListDonors()
        {
            List<Donor> Donor = db.Donors.ToList();
            List<DonorDto> DonorDto = new List<DonorDto>();

            Donor.ForEach(a => DonorDto.Add(new DonorDto()
            {
                DonorId = a.DonorId,
                Name = a.Name,
                Email = a.Email,
                DepartmentId = a.DepartmentId,
                Phone = a.Phone,
                Amount = a.Amount,

            }));
            return Ok(DonorDto);
        }


        [HttpGet]
        [ResponseType(typeof(Donor))]

        /// <summary>
        /// Returns details of the article by article id
        /// </summary>
        /// <param name="id">Article primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An article in the system matching up to the article ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// GET: api/ArticleData/FindArticle/5
        /// </example>
        // GET: api/DonorData/FindDonor/1
        public IHttpActionResult FindDonor(int id)

        {
            Donor Donor = db.Donors.Find(id);
           

            if (Donor == null)
            {
                return NotFound();
            }

            DonorDto DonorDto = new DonorDto()
            {
                DonorId = Donor.DonorId,
                Name = Donor.Name,
                Email = Donor.Email,
                DepartmentId = Donor.DepartmentId,
                Phone = Donor.Phone,
                Amount = Donor.Amount,
                Department = new DepartmentDto() { Name = Donor.Department.Name}
            };

            return Ok(DonorDto);
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
        [HttpPost]
        // PUT: api/DonorData/UpdateDonor/5
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateDonor(int id, Donor donor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != donor.DonorId)
            {
                return BadRequest();
            }

            db.Entry(donor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonorExists(id))
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
        /// Create an appointment to the system
        /// </summary>
        /// <param name="article">JSON form data of article</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT:Article ID
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/ArticleData/CreateArticle
        /// </example>
        [HttpPost]
        // POST: api/DonorData/AddDonor
        [ResponseType(typeof(Donor))]
        public IHttpActionResult AddDonor(Donor donor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Donors.Add(donor);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = donor.DonorId }, donor);
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
        /// DELETE: api/ArticleData/DeleteArticle/5
        /// FORM DATA: (empty)
        /// </example>
        [HttpPost]
        // DELETE: api/DonorData/DeleteDonor/5
        [ResponseType(typeof(Donor))]
        [Authorize]
        public IHttpActionResult DeleteDonor(int id)
        {
            Donor donor = db.Donors.Find(id);
            if (donor == null)
            {
                return NotFound();
            }

            db.Donors.Remove(donor);
            db.SaveChanges();

            return Ok(donor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DonorExists(int id)
        {
            return db.Donors.Count(e => e.DonorId == id) > 0;
        }

    }

}