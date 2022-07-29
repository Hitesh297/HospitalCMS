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

        // GET: api/DepartmentData/ListDepartment
        [HttpGet]
        public IHttpActionResult ListDepartment()
        {
            List<Department> Departments = db.Departments.ToList();
            List<DepartmentDto> DepartmentDto = new List<DepartmentDto>();

            Departments.ForEach(a => DepartmentDto.Add(new DepartmentDto()
            {
               DepartmentId = a.DepartmentId,
               Name = a.Name
            }));
            return Ok(DepartmentDto);
        }

        // GET: api/DepartmentData/FindDepartment/5
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

        // PUT: api/DepartmentData/EditDepartment/5
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

        // POST: api/DepartmentData/CreateDepartment
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

        // DELETE: api/DepartmentData/DeleteDepartment/5
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