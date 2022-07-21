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
        //------------------------------
        [HttpGet]
        [ResponseType(typeof(DonorDto))]
        // GET: api/DonorData
        public IHttpActionResult ListDonors() // Ihttpaction result
        {
            List<Donor> Donor = db.Donors.ToList();
            List<DonorDto> DonorDto = new List<DonorDto>();

            Donor.ForEach(a => DonorDto.Add(new DonorDto()
            {
                DonorId = a.DonorId,
                Email = a.Email,
                DepartmentId = a.DepartmentId,
               // Department = a.Department, do we need to make this (it is having a foreign key)
                Phone = a.Phone,
                Amount = a.Amount,

            })) ;
            return Ok(DonorDto);
        }




        // ----------------------------- Get ------------------
        // GET: api/DonorData
        public IQueryable<Donor> GetDonors()
        {
            return db.Donors;
        }

        // GET: api/DonorData/5
        [ResponseType(typeof(Donor))]
        public IHttpActionResult GetDonor(int id)
        {
            Donor donor = db.Donors.Find(id);
            if (donor == null)
            {
                return NotFound();
            }

            return Ok(donor);
       }

        //-------------------------put patient-----------------------
       //  PUT: api/DonorData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDonor(int id, Donor donor)
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

        private bool DonorExists(int id)
        {
            throw new NotImplementedException();
        }

        // -------------------------------------------------------------


        [HttpGet]
        [ResponseType(typeof(Donor))]
        // GET: api/DonorData/1
        public IHttpActionResult FindDonor(int id)

        {
            Donor Donor = db.Donors.Find(id);
            DonorDto DonorDto = new DonorDto()
            {
                DonorId = Donor.DonorId,
                Email = Donor.Email,
                DepartmentId = Donor.DepartmentId,
                Phone = Donor.Phone,
                Amount = Donor.Amount
            };
            if (Donor == null)
            {
                return NotFound();
            }

            return Ok(DonorDto);
        }


        [HttpPost]
        // PUT: api/DonorData/5
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
        // POST: api/DonorData
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



        // POST: api/DonorData
        //[ResponseType(typeof(Donor))]
        //public IHttpActionResult PostDonor(Donor donor)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Donors.Add(donor);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = donor.DonorId }, donor);
        //}

        // DELETE: api/DonorData/5

        [HttpPost]
        // DELETE: api/DonorData/5
        [ResponseType(typeof(Donor))]
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

        private bool PatientExists(int id)
        {
            return db.Patients.Count(e => e.PatientId == id) > 0;

        }
    }

}