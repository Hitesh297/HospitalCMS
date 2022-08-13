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
    public class DepartmentDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Department in the system
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Department in the database
        /// </returns>
        /// <example>
        // GET: api/DepartmentData/ListDepartment
        /// </example>

        [HttpGet]
        [Route("api/DepartmentData/ListDepartment/{SearchKey?}")]
        [ResponseType(typeof(DepartmentDto))]
        public IHttpActionResult ListDepartment(string SearchKey = null)
        {
            List<Department> Departments = new List<Department>();
            if (SearchKey != null)
            {
                Departments = db.Departments.Where(x => x.Name.ToLower().Contains(SearchKey.ToLower())).ToList();
            }
            else
            { 
                Departments = db.Departments.ToList();
            }

            List<DepartmentDto> DepartmentDto = new List<DepartmentDto>();

            Departments.ForEach(a => DepartmentDto.Add(new DepartmentDto()
            {
               DepartmentId = a.DepartmentId,
               Name = a.Name
            }));
            return Ok(DepartmentDto);
        }

        /// <summary>
        /// Returns details of the Department by Department id
        /// </summary>
        /// <param name="id">Department primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: Department in the system matching up to the Department ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        // GET: api/DepartmentData/FindDepartment/5
        /// </example>
        
        [ResponseType(typeof(Department))]
        [HttpGet]
        public IHttpActionResult FindDepartment(int id)
        {
            Department department = db.Departments.Include(x=>x.FAQs).Where(y=>y.DepartmentId == id).FirstOrDefault();
            if (department == null)
            {
                return NotFound();
            }

            DepartmentDto departmentDto = new DepartmentDto()
            {
                DepartmentId = department.DepartmentId,
                Name = department.Name
            };

            if (department.FAQs != null && department.FAQs.Count() != 0)
            {
                departmentDto.FAQs = new List<FAQDto>();
                foreach (var faq in department.FAQs)
                {
                    departmentDto.FAQs.Add(new FAQDto() { FAQId = faq.FAQId, Question = faq.Question, Answer = faq.Answer });
                }
            }

            return Ok(departmentDto);
        }

        /// <summary>
        /// Edit a particular Department in the system with POST Data input
        /// </summary>
        /// <param name="id">Department primary key</param>
        /// <param name=Department">JSON form data of Department</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        // PUT: api/DepartmentData/EditDepartment/5
        /// FORM DATA: Article JSON Object
        /// </example>
        
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult EditDepartment(int id, Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != department.DepartmentId)
            {
                return BadRequest();
            }

            db.Entry(department).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
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
        /// Add Departmentt to the system
        /// </summary>
        /// <param name="Department">JSON form data of Department</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT:Department ID
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        // POST: api/DepartmentData/CreateDepartment
        /// </example>
        
        [ResponseType(typeof(Department))]
        [HttpPost]
        public IHttpActionResult CreateDepartment(Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Departments.Add(department);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = department.DepartmentId }, department);
        }

        /// <summary>
        /// Deletes Department from the system by it's ID.
        /// </summary>
        /// <param name="id">primary key of Department</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        // DELETE: api/DepartmentData/DeleteDepartment/5
        /// FORM DATA: (empty)
        /// </example>
        
        [ResponseType(typeof(Department))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult DeleteDepartment(int id)
        {
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }

            db.Departments.Remove(department);
            db.SaveChanges();

            return Ok(department);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DepartmentExists(int id)
        {
            return db.Departments.Count(e => e.DepartmentId == id) > 0;
        }
    }
}