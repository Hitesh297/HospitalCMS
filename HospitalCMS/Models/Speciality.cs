using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HospitalCMS.Models
{
    public class Speciality
    {
        [Key]
        public int SpecialityId { get; set; }
        public string Name { get; set; }
        public ICollection<Doctor> Doctors { get; set; }
    }
    public class SpecialityDto { 
        public int SpecialityId { get; set; }
        public string Name { get; set; }
    }
}