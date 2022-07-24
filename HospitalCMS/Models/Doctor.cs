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
        //[ForeignKey("Speciality")]
        //public string SpecialityId { get; set; }
        //public virtual Speciality Speciality { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public ICollection<Speciality> Specialities { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public bool DoctorHasPic { get; set; }
        public string PicExtension { get; set; }
    }
    public class DoctorDto
    {
        public int DoctorId { get; set; }
        [Display(Name = "Doctor Name")]
        public string Name { get; set; }
        public int Experience { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool DoctorHasPic { get; set; }
        public string PicExtension { get; set; }
        public List<SpecialityDto> Specialities { get; set; }
        public List<AppointmentDto> Appointments { get; set; }
    }

    public class DoctorVM
    {
        public DoctorDto Doctor { get; set; }
        public IEnumerable<SpecialityDto> SpecialitiesAssigned { get; set; }
        public IEnumerable<SpecialityDto> SpecialitiesNotAssigned { get; set; }
    }
}