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
    public class FAQController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static FAQController()
        {
            client = new HttpClient();
            //client.BaseAddress = new Uri("https://localhost:44305/api/");
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiServer"]);
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
            return View();
        }

        // GET: FAQ/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FAQ/Create
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

        // GET: FAQ/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FAQ/Edit/5
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

        // GET: FAQ/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FAQ/Delete/5
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
