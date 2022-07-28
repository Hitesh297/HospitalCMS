using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HospitalCMS.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public ICollection<FAQ> FAQs { get; set; }

    }

    public class DepartmentDto
    {
        public int DepartmentId { get; set; }
        [Display(Name = "Department Name")]
        public string Name { get; set; }
        public List<FAQDto> FAQs { get; set; }

    }
}