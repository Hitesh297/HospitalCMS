using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HospitalCMS.Models
{
    public class Article
    {
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public bool HasPic { get; set; }
        public string PicExtension { get; set; }
        public string Description { get; set; }
        [ForeignKey("Event")]
        public int EventId { get; set; }
        public virtual Event Event { get; set; }
    }
    public class ArticleDto
    {
        public int ArticleId { get; set; }
        [Display(Name = "Article Title")]
        public string Title { get; set; }
        public bool HasPic { get; set; }
        public string PicExtension { get; set; }
        public string Description { get; set; }
        public int EventId { get; set; }
    }
}