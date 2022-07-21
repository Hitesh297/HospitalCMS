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
    public class FAQDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/FAQData
        public IQueryable<FAQ> GetFAQs()
        {
            return db.FAQs;
        }

        // GET: api/FAQData/5
        [ResponseType(typeof(FAQ))]
        public IHttpActionResult GetFAQ(int id)
        {
            FAQ fAQ = db.FAQs.Find(id);
            if (fAQ == null)
            {
                return NotFound();
            }

            return Ok(fAQ);
        }

        // PUT: api/FAQData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFAQ(int id, FAQ fAQ)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fAQ.FAQId)
            {
                return BadRequest();
            }

            db.Entry(fAQ).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FAQExists(id))
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

        // POST: api/FAQData
        [ResponseType(typeof(FAQ))]
        public IHttpActionResult PostFAQ(FAQ fAQ)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FAQs.Add(fAQ);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = fAQ.FAQId }, fAQ);
        }

        // DELETE: api/FAQData/5
        [ResponseType(typeof(FAQ))]
        public IHttpActionResult DeleteFAQ(int id)
        {
            FAQ fAQ = db.FAQs.Find(id);
            if (fAQ == null)
            {
                return NotFound();
            }

            db.FAQs.Remove(fAQ);
            db.SaveChanges();

            return Ok(fAQ);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FAQExists(int id)
        {
            return db.FAQs.Count(e => e.FAQId == id) > 0;
        }
    }
}