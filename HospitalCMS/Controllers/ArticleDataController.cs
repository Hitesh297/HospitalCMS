﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HospitalCMS.Models;
using System.Web;
using System.IO;

namespace HospitalCMS.Controllers
{
    public class ArticleDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ArticleData/ListArticles
        [HttpGet]
        public IHttpActionResult ListArticles()
        {
            List<Article> Articles = db.Articles.ToList();
            List<ArticleDto> ArticlesDto = new List<ArticleDto>();

            Articles.ForEach(a => ArticlesDto.Add(new ArticleDto()
            {
                ArticleId = a.ArticleId,
                Description = a.Description,
                EventId = a.EventId,
                HasPic = a.HasPic,
                PicExtension = a.PicExtension,
                Title = a.Title

            }));
            return Ok(ArticlesDto);
        }

        // GET: api/ArticleData/FindArticle/5
        [ResponseType(typeof(Article))]
        [HttpGet]
        public IHttpActionResult FindArticle(int id)
        {
            Article article = db.Articles.Find(id);

            if (article == null)
            {
                return NotFound();
            }

            ArticleDto articleDto = new ArticleDto()
            {
                ArticleId = article.ArticleId,
                Description = article.Description,
                EventId = article.EventId,
                HasPic = article.HasPic,
                PicExtension = article.PicExtension,
                Title = article.Title,
                Event = new EventDto() { Title = article.Event.Title}
            };

            return Ok(articleDto);
        }

        // PUT: api/ArticleData/EditArticle/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult EditArticle(int id, Article article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != article.ArticleId)
            {
                return BadRequest();
            }

            db.Entry(article).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        // POST: api/ArticleData/UploadArticlePic/{id}
        [HttpPost]
        public IHttpActionResult UploadArticlePic(int id)
        {

            bool haspic = false;
            string picextension;
            if (Request.Content.IsMimeMultipartContent())
            {

                int numfiles = HttpContext.Current.Request.Files.Count;

                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var articlePic = HttpContext.Current.Request.Files[0];
                    if (articlePic.ContentLength > 0)
                    {
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(articlePic.FileName).Substring(1);
                        if (valtypes.Contains(extension, StringComparer.InvariantCultureIgnoreCase))
                        {
                            try
                            {
                                string fn = id + "." + extension;
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/Articles/"), fn);

                                articlePic.SaveAs(path);

                                haspic = true;
                                picextension = extension;

                                Article selectedArticle = db.Articles.Find(id);
                                selectedArticle.HasPic = haspic;
                                selectedArticle.PicExtension = extension;
                                db.Entry(selectedArticle).State = EntityState.Modified;

                                db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                return BadRequest();
                            }
                        }
                    }

                }
                else
                {
                    Doctor SelectedDoctor = db.Doctors.Find(id);
                    SelectedDoctor.DoctorHasPic = haspic;
                    SelectedDoctor.PicExtension = null;
                    db.Entry(SelectedDoctor).State = EntityState.Modified;

                    db.SaveChanges();
                }

                return Ok();
            }
            else
            {
                //not multipart form data
                return BadRequest();

            }

        }

        // POST: api/ArticleData/CreateArticle
        [ResponseType(typeof(Article))]
        [HttpPost]
        public IHttpActionResult CreateArticle(Article article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Articles.Add(article);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = article.ArticleId }, article);
        }

        // DELETE: api/ArticleData/DeleteArticle/5
        [ResponseType(typeof(Article))]
        [HttpPost]
        public IHttpActionResult DeleteArticle(int id)
        {
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return NotFound();
            }

            db.Articles.Remove(article);
            db.SaveChanges();

            return Ok(article);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ArticleExists(int id)
        {
            return db.Articles.Count(e => e.ArticleId == id) > 0;
        }
    }
}