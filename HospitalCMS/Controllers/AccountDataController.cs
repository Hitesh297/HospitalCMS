using HospitalCMS.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HospitalCMS.Controllers
{
    public class AccountDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/AccountData/ListRoles
        [HttpGet]
        public IHttpActionResult ListRoles()
        {
            List<IdentityRole> roles = db.Roles.ToList();
            return Ok(roles);
        }
    }
}
