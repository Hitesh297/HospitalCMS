using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HospitalCMS.Models
{
    public class FAQ
    {
        [Key]
        public int FAQId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

    }

    public class FAQDto
    {
        public int FAQId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int DepartmentId { get; set; }
        public DepartmentDto Department { get; set; }

    }
}