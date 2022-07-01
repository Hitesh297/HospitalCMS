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


        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }


        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public virtual Patient Patient{ get; set; }


        public DateTime Schedule { get; set; }

        public string Notes { get; set; }

    }
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }

        public int SpecialityId { get; set; }

        public int DoctorId { get; set; }

        public int PatientId { get; set; }

        public DateTime Schedule { get; set; }

        public string Notes { get; set; }
    }
}