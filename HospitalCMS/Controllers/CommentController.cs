using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using HospitalCMS.Models;
using Newtonsoft.Json;

namespace HospitalCMS.Controllers
{
    public class CommentController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CommentController()
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
        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }

        // GET: Comment/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Comment/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Comment/Create
        [HttpPost]
        public ActionResult Create(CommentDto comment)
        {
            string url = "";
            HttpResponseMessage response;
            Comment comment1 = new Comment();



            try
            {
                if(!string.IsNullOrEmpty(comment.PatientEmail))
                {
                    url = "PatientData/FindPatientByEmail/"+ comment.PatientEmail + "/";
                    response = client.GetAsync(url).Result;
                    PatientDto patient = response.Content.ReadAsAsync<PatientDto>().Result;
                    comment1.PatientId = patient.PatientId;
                }

                if (!string.IsNullOrEmpty(comment.DoctorEmail))
                {
                    url = "DoctorData/FindDoctorByEmail/" + comment.DoctorEmail + "/";
                    response = client.GetAsync(url).Result;
                    DoctorDto doctor = response.Content.ReadAsAsync<DoctorDto>().Result;
                    comment1.DoctorId = doctor.DoctorId;

                }

                comment1.CommentText = comment.CommentText;
                comment1.ArticleId = comment.ArticleId;

               
                url = "CommentData/CreateComment";
                string jsonpayload = JsonConvert.SerializeObject(comment1);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                 response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Details", "Article", new { id = comment.ArticleId });
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

        // GET: Comment/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Comment/Edit/5
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

        // GET: Comment/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Comment/Delete/5
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
