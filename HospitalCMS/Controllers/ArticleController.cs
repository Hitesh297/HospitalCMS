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
    public class ArticleController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ArticleController()
        {
            client = new HttpClient();
            //client.BaseAddress = new Uri("https://localhost:44305/api/");
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiServer"]);
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
            return View(article);
        }

        // GET: Article/Create
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Article/Edit/5
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

        // GET: Article/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Article/Delete/5
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
