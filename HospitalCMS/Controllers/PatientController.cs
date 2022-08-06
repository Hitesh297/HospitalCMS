using HospitalCMS.Models;
using Microsoft.AspNet.Identity;
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
    public class PatientController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PatientController()
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
        // GET: Patient
        public ActionResult List()
        {
            string url = "PatientData/ListPatients";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<PatientDto> patients = response.Content.ReadAsAsync<IEnumerable<PatientDto>>().Result;
            return View(patients);
        }

        // GET: Patient/Details/5
        public ActionResult Details(int id)
        {
            string url = "PatientData/FindPatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PatientDto patient = response.Content.ReadAsAsync<PatientDto>().Result;
            return View(patient);
        }

        // GET: Patient/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Patient/Create
        [HttpPost]
        public ActionResult Create(Patient patient)
        {
            try
            {
                string url = "PatientData/AddPatient";
                string jsonpayload = JsonConvert.SerializeObject(patient);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                HttpResponseMessage response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("list", "Patient");
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

        // GET: Patient/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if(User.IsInRole("Patient"))
            {
                string getbyemailurl = "PatientData/FindPatientByEmail/" + User.Identity.GetUserName() + "/";
                HttpResponseMessage getbyemailresponse = client.GetAsync(getbyemailurl).Result;
                if (!getbyemailresponse.IsSuccessStatusCode)
                {
                    return RedirectToAction("Create", "Patient");
                }
                else
                {
                    PatientDto patientbyemail = getbyemailresponse.Content.ReadAsAsync<PatientDto>().Result;
                    id = patientbyemail.PatientId;
                    //return RedirectToAction("Edit", "Patient", new { id = patientbyemail.PatientId });
                }
            }
            string url = "PatientData/FindPatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PatientDto patient = response.Content.ReadAsAsync<PatientDto>().Result;
            return View(patient);
        }

        // POST: Patient/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Patient patient)
        {
            try
            {
                string url = "PatientData/EditPatient/" + id;
                string jsonpayload = JsonConvert.SerializeObject(patient);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                HttpResponseMessage response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("list", "Patient");
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

        // GET: Patient/Delete/5
        [Authorize]
        public ActionResult ConfirmDelete(int id)
        {
            string url = "PatientData/FindPatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PatientDto patient = response.Content.ReadAsAsync<PatientDto>().Result;
            return View(patient);
        }

        // POST: Patient/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            try
            {
                GetApplicationCookie();
                string url = "PatientData/DeletePatient/" + id;
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
