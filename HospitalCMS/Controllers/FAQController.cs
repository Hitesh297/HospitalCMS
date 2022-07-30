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
    public class FAQController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static FAQController()
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
        /// <summary>
        /// Grabs the authentication cookie sent to this controller.
        /// </summary>
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
        // GET: FAQ
        public ActionResult List()
        {
            string url = "FAQData/ListFAQs";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<FAQDto> fAQs = response.Content.ReadAsAsync<IEnumerable<FAQDto>>().Result;
            return View(fAQs);
        }

        // GET: FAQ/Details/5
        public ActionResult Details(int id)
        {
            string url = "FAQData/FindFAQ/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            FAQDto fAQ = response.Content.ReadAsAsync<FAQDto>().Result;
            return View(fAQ);
        }

        // GET: FAQ/Create
        public ActionResult Create()
        {
            string url = "DepartmentData/ListDepartment";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> departments = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            ViewData["Departments"] = departments;
            return View();
        }

        // POST: FAQ/Create
        [HttpPost]
        public ActionResult Create(FAQ fAQ)
        {
            try
            {
                string url = "FAQData/CreateFAQ";
                string jsonpayload = JsonConvert.SerializeObject(fAQ);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                HttpResponseMessage response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("list", "FAQ");
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

        // GET: FAQ/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "FAQData/FindFAQ/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            FAQDto fAQ = response.Content.ReadAsAsync<FAQDto>().Result;

            url = "DepartmentData/ListDepartment";
            response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> departments = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            ViewData["Departments"] = departments;
            return View(fAQ);
        }

        // POST: FAQ/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FAQ faq)
        {
            try
            {
                string url = "FAQData/EditFAQ/"+ id;
                string jsonpayload = JsonConvert.SerializeObject(faq);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                HttpResponseMessage response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("list", "FAQ");
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

        // GET: FAQ/Delete/5
        [Authorize]
        public ActionResult ConfirmDelete(int id)
        {
            string url = "FAQData/FindFAQ/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            FAQDto FAQ = response.Content.ReadAsAsync<FAQDto>().Result;
            return View(FAQ);
        }

        // POST: FAQ/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            try
            {
                GetApplicationCookie();
                string url = "FAQData/DeleteFAQ/" + id;
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
    }
}
