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

        /// <summary>
        /// Returns all FAQa in the system
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all FAQa in the database
        /// </returns>
        /// <example>
        // GET: api/FAQData/ListFAQs
        /// </example>
        
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

        /// <summary>
        /// Returns details of the FAQ by FAQ id
        /// </summary>
        /// <param name="id"FAQ primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: FAQe in the system matching up to the FAQ ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        // GET: api/FAQData/FindFAQ/5
        /// </example>
        
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

        /// <summary>
        /// Edit a particular FAQ in the system with POST Data input
        /// </summary>
        /// <param name="id">FAQ primary key</param>
        /// <param name="FAQ">JSON form data of FAQ</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        // PUT: api/FAQData/EditFAQ/5
        /// FORM DATA: FAQ JSON Object
        /// </example>
        
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

         /// <summary>
        /// Add FAQ to the system
        /// </summary>
        /// <param name="FAQ">JSON form data of FAQ</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT:FAQ ID
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        // POST: api/FAQData/CreateFAQ
        /// </example>
        
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

        /// <summary>
        /// Deletes FAQ from the system by it's ID.
        /// </summary>
        /// <param name="id">primary key of FAQ</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        // DELETE: api/FAQData/DeleteFAQ/5
        /// FORM DATA: (empty)
        /// </example>
        
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