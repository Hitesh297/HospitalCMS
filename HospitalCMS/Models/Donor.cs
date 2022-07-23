using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HospitalCMS.Models
{
    public class Donor
    {
        [Key]
        public int DonorId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        public string Phone { get; set; }
        public decimal Amount { get; set; }
    }

    public class DonorDto
    {
        public int DonorId { get; set; }
        [Display(Name = "Donor Name")]
        public string Name { get; set; }
        public string Email { get; set; }
        public int DepartmentId { get; set; }
        public string Phone { get; set; }
        public decimal Amount { get; set; }
    }


}