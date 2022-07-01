using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HospitalCMS.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }
        [ForeignKey("Speciality")]
        public int SpecialityId { get; set; }
        public virtual Speciality Speciality { get; set; }
        [ForeignKey("Speciality")]
        public int DoctorId { get; set; }

    }
}