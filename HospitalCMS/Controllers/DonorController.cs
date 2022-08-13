using HospitalCMS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HospitalCMS.Controllers
{
    public class DonorController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DonorController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };
            client = new HttpClient(handler);
            //client.BaseAddress = new Uri("https://localhost:44305/api/");
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiServer"]);
        }
        private void GetApplicationCookie()
        {
            string token = "";
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }
        // GET: Donor
        public ActionResult List()
        {
            string url = "DonorData/ListDonors";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DonorDto> donors = response.Content.ReadAsAsync<IEnumerable<DonorDto>>().Result;
            return View(donors);
        }

        // GET: Donor/Details/5
        public ActionResult Details(int id)
        {
            string url = "DonorData/FindDonor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DonorDto donor = response.Content.ReadAsAsync<DonorDto>().Result;
            return View(donor);
        }

        // GET: Donor/Create
        public ActionResult Create()
        {
            string url = "DepartmentData/ListDepartment";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> departments = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            ViewData["Departments"] = departments;
            return View();
        }

        // POST: Donor/Create
        [HttpPost]
        public ActionResult Create(Donor donor)
        {
            try
            {
                string url = "DonorData/AddDonor";
                string jsonpayload = JsonConvert.SerializeObject(donor);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                HttpResponseMessage response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    if (!User.IsInRole("Admin"))
                    {
                        return RedirectToAction("Thankyou", "Donor");
                    }
                    return RedirectToAction("list", "Donor");
                }
                else
                {
                    return View("Error");
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Donor/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            string url = "DonorData/FindDonor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DonorDto donor = response.Content.ReadAsAsync<DonorDto>().Result;

            url = "DepartmentData/ListDepartment";
            response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> departments = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            ViewData["Departments"] = departments;

            return View(donor);
        }

        // POST: Donor/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, Donor donor)
        {
            try
            {
                string url = "DonorData/UpdateDonor/" + id;
                string jsonpayload = JsonConvert.SerializeObject(donor);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                HttpResponseMessage response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("list", "Donor");
                }
                else
                {
                    return View("Error");
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Donor/Delete/5
        [Authorize]
        [Authorize(Roles = "Admin")]
        public ActionResult ConfirmDelete(int id)
        {
            string url = "DonorData/FindDonor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DonorDto donor = response.Content.ReadAsAsync<DonorDto>().Result;
            return View(donor);
        }

        // POST: Donor/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            try
            {
                GetApplicationCookie();
                string url = "DonorData/DeleteDonor/" + id;
                HttpContent content = new StringContent("");
                content.Headers.ContentType.MediaType = "application/json";
                HttpResponseMessage response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    return View("Error");
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Thankyou()
        {
            return View();
        }

    }
}
