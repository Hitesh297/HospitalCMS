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
    public class HomeController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static HomeController()
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
        public ActionResult Index()
        {
            string url = "EventData/ListEvents";
            HttpResponseMessage response = client.GetAsync(url).Result;
            TempData["Events"] = response.Content.ReadAsAsync<IEnumerable<EventDto>>().Result;

            url = "ArticleData/ListArticles";
            response = client.GetAsync(url).Result;
            IEnumerable<ArticleDto> articles = response.Content.ReadAsAsync<IEnumerable<ArticleDto>>().Result;

            TempData["Articles"] = articles;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}