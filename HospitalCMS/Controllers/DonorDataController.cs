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
        /// Returns all Donor in the system
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Donor in the database
        /// </returns>
        /// <example>
        // GET: api/DonorData/ListDonors
        /// </example>
        
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
        /// Returns details of the Donor by Donor id
        /// </summary>
        /// <param name="id">Donor primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: Donor in the system matching up to the Donor ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        // GET: api/DonorData/FindDonor/1
        /// </example>
        
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
        /// Updates a particular Donor in the system with POST Data input
        /// </summary>
        /// <param name="id">Donor primary key</param>
        /// <param name="Donor">JSON form data of Donor</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        // PUT: api/DonorData/UpdateDonor/5
        /// FORM DATA: Donor JSON Object
        /// </example>
        [HttpPost]
        
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
        /// Add a Donor to the system
        /// </summary>
        /// <param name="Donor">JSON form data of Donor</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT:Donor ID
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
       // POST: api/DonorData/AddDonor
        /// </example>
        [HttpPost]
        
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
        /// Deletes a Donor from the system by it's ID.
        /// </summary>
        /// <param name="id">primary key of Donor</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        // DELETE: api/DonorData/DeleteDonor/5
        /// FORM DATA: (empty)
        /// </example>
        [HttpPost]
        
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