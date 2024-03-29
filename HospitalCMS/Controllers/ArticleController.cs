﻿using HospitalCMS.Models;
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
    public class ArticleController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ArticleController()
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


        // GET: Article
        public ActionResult List()
        {
            string url = "ArticleData/ListArticles";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<ArticleDto> articles = response.Content.ReadAsAsync<IEnumerable<ArticleDto>>().Result;
            return View(articles);
        }

        // GET: Article/Details/5
        public ActionResult Details(int id)
        {
            string url = "ArticleData/FindArticle/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ArticleDto article = response.Content.ReadAsAsync<ArticleDto>().Result;
            if (article.Comments != null || article.Comments.Count() != 0)
            {
                foreach (var comment in article.Comments)
                {
                    if(comment.PatientId != null)
                    {
                        url = "PatientData/FindPatient/" + comment.PatientId;
                        response = client.GetAsync(url).Result;
                        PatientDto patient = response.Content.ReadAsAsync<PatientDto>().Result;
                        comment.Patient = patient;
                    }


                    if (comment.DoctorId != null)
                    {
                        url = "DoctorData/FindDoctor/" + comment.DoctorId;
                        response = client.GetAsync(url).Result;
                        DoctorDto doctor = response.Content.ReadAsAsync<DoctorDto>().Result;
                        comment.Doctor = doctor;
                    }
                }
            }
            return View(article);
        }

        // GET: Article/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            string url = "EventData/ListEvents";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<EventDto> events = response.Content.ReadAsAsync<IEnumerable<EventDto>>().Result;
            ViewData["Events"] = events;
            return View();
        }

        // POST: Article/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Article article)
        {
            try
            {
                string url = "ArticleData/CreateArticle";
                string jsonpayload = JsonConvert.SerializeObject(article);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                HttpResponseMessage response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("list", "Article");
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

        // GET: Article/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            string url = "ArticleData/FindArticle/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ArticleDto article = response.Content.ReadAsAsync<ArticleDto>().Result;

            url = "EventData/ListEvents";
            response = client.GetAsync(url).Result;
            IEnumerable<EventDto> events = response.Content.ReadAsAsync<IEnumerable<EventDto>>().Result;
            ViewData["Events"] = events;

            return View(article);
        }

        // POST: Article/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, Article article, HttpPostedFileBase articlePic)
        {
            try
            {
                string url = "ArticleData/EditArticle/"+id;
                string jsonpayload = JsonConvert.SerializeObject(article);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                HttpResponseMessage response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode && articlePic != null)
                {
                    url = "ArticleData/UploadArticlePic/" + id;
                    MultipartFormDataContent requestcontent = new MultipartFormDataContent();

                    HttpContent imagecontent = new StreamContent(articlePic.InputStream);
                    requestcontent.Add(imagecontent, "ArticlePic", articlePic.FileName);

                    response = client.PostAsync(url, requestcontent).Result;

                    return RedirectToAction("List");
                }
                else if (response.IsSuccessStatusCode)
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

        // GET: Article/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult ConfirmDelete(int id)
        {
            string url = "ArticleData/FindArticle/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ArticleDto article = response.Content.ReadAsAsync<ArticleDto>().Result;
            return View(article);
           
        }

        // POST: Article/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {

            try
            {
                GetApplicationCookie();
                string url = "ArticleData/DeleteArticle/" + id;
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
