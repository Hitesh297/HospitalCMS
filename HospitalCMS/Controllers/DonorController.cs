using HospitalCMS.Models;
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
            client = new HttpClient();
            //client.BaseAddress = new Uri("https://localhost:44305/api/");
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiServer"]);
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
            return View();
        }

        // POST: Donor/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Donor/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Donor/Edit/5
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

        // GET: Donor/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Donor/Delete/5
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
