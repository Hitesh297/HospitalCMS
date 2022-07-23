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
    public class SpecialityController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static SpecialityController()
        {
            client = new HttpClient();
            //client.BaseAddress = new Uri("https://localhost:44305/api/");
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiServer"]);
        }
        // GET: Speciality
        public ActionResult List()
        {
            string url = "SpecialityData/ListSpecialities";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<SpecialityDto> specialities = response.Content.ReadAsAsync<IEnumerable<SpecialityDto>>().Result;
            return View(specialities);
        }

        // GET: Speciality/Details/5
        public ActionResult Details(int id)
        {
            string url = "SpecialityData/FindSpeciality/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            SpecialityDto speciality = response.Content.ReadAsAsync<SpecialityDto>().Result;
            return View(speciality);
        }

        // GET: Speciality/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Speciality/Create
        [HttpPost]
        public ActionResult Create(Speciality speciality)
        {
            try
            {
                string url = "SpecialityData/CreateSpeciality";
                string jsonpayload = JsonConvert.SerializeObject(speciality);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                HttpResponseMessage response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("list", "Speciality");
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

        // GET: Speciality/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Speciality/Edit/5
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

        // GET: Speciality/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Speciality/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
