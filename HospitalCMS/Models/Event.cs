using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HospitalCMS.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public bool HasPic { get; set; }
        public string PicExtension { get; set; }
        public string Description { get; set; }
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        public ICollection<Article> Articles { get; set; }
    }

    public class EventDto
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public bool HasPic { get; set; }
        public string PicExtension { get; set; }
        public string Description { get; set; }
        public int DepartmentId { get; set; }
        public ICollection<Article> Articles { get; set; }
    }


}