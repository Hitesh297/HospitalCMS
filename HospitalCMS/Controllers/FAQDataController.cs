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
        /// Returns all article in the system
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all article in the database
        /// </returns>
        /// <example>
        /// GET: api/ArticleData/ListArticles
        /// </example>
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

        /// <summary>
        /// Edit a particular article in the system with POST Data input
        /// </summary>
        /// <param name="id">Article primary key</param>
        /// <param name="article">JSON form data of an article</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// PUT: api/ArticleData/EditArticle/5
        /// FORM DATA: Article JSON Object
        /// </example>
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