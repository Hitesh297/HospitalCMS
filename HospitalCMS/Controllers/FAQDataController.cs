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

        // GET: api/FAQData/ListFAQs
        [HttpGet]
        public IHttpActionResult ListFAQs()
        {
            List<FAQ> fAQs = db.FAQs.ToList();
            List<FAQDto> fAQDtos = new List<FAQDto>();

            fAQs.ForEach(a => fAQDtos.Add(new FAQDto()
            {
                FAQId = a.FAQId,
                DepartmentId = a.DepartmentId,
                Answer = a.Answer,
                Question = a.Question
            }));
            return Ok(fAQDtos);
        }

        // GET: api/FAQData/FindFAQ/5
        [ResponseType(typeof(FAQ))]
        [HttpGet]
        public IHttpActionResult FindFAQ(int id)
        {
            FAQ fAQ = db.FAQs.Find(id);
            if (fAQ == null)
            {
                return NotFound();
            }

            FAQDto fAQDto = new FAQDto()
            {
                FAQId = fAQ.FAQId,
                DepartmentId = fAQ.DepartmentId,
                Answer = fAQ.Answer,
                Question = fAQ.Question,
                Department = new DepartmentDto() { Name = fAQ.Department.Name }
            };

            return Ok(fAQDto);
        }

        // PUT: api/FAQData/EditFAQ/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult EditFAQ(int id, FAQ fAQ)
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

        // POST: api/FAQData/CreateFAQ
        [ResponseType(typeof(FAQ))]
        [HttpPost]
        public IHttpActionResult CreateFAQ(FAQ fAQ)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FAQs.Add(fAQ);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = fAQ.FAQId }, fAQ);
        }

        // DELETE: api/FAQData/DeleteFAQ/5
        [ResponseType(typeof(FAQ))]
        [HttpPost]
        [Authorize]
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