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
        [Authorize]
        public ActionResult ConfirmDelete(int id)
        {
            string url = "ArticleData/FindArticle/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ArticleDto article = response.Content.ReadAsAsync<ArticleDto>().Result;
            return View(article);
           
        }

        // POST: Article/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {

            try
            {
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
