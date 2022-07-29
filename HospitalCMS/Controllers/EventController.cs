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
    public class EventController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static EventController()
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
        // GET: Event
        public ActionResult List()
        {
            string url = "EventData/ListEvents";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<EventDto> events = response.Content.ReadAsAsync<IEnumerable<EventDto>>().Result;
            return View(events);
        }

        // GET: Event/Details/5
        public ActionResult Details(int id)
        {
            string url = "EventData/FindEvent/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            EventDto eventDto = response.Content.ReadAsAsync<EventDto>().Result;
            return View(eventDto);
        }

        // GET: Event/Create
        public ActionResult Create()
        {
            string url = "DepartmentData/ListDepartment";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> departments = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            ViewData["Departments"] = departments;
            return View();
        }

        // POST: Event/Create
        [HttpPost]
        public ActionResult Create(Event @event)
        {
            try
            {
                string url = "EventData/AddEvent";
                string jsonpayload = JsonConvert.SerializeObject(@event);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                HttpResponseMessage response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("list", "Event");
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

        // GET: Event/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Event/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Event/Delete/5
        [Authorize]
        public ActionResult ConfirmDelete(int id)
        {
            string url = "EventData/FindEvent/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            EventDto Event = response.Content.ReadAsAsync<EventDto>().Result;
            return View(Event);
        }

        // POST: Event/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            try
            {
                GetApplicationCookie();
                string url = "EventData/DeleteEvent/" + id;
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
