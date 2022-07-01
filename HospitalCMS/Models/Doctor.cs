using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HospitalCMS.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        public string Name { get; set; }

        public int Experience { get; set; }

        [ForeignKey("Speciality")]
        public string SpecialityId { get; set; }
        public virtual Speciality Speciality { get; set; }

        public int Phone { get; set; }

        public string Email { get; set; }
    }
    public class DoctorDto
    {
        public int DoctorId { get; set; }
        public string Name { get; set; }
        public int Experience { get; set; }
        public int SpecialityId { get; set; }
        public int Phone { get; set; }
        public String Email { get; set; }
    }

}