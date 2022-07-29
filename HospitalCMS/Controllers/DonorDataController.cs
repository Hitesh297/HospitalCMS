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