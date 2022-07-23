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
        //removed speciality foreign key from the appointment because it
        //can be found inside Doctor object
        //[ForeignKey("Speciality")]
        //public int SpecialityId { get; set; }
        //public virtual Speciality Speciality { get; set; }


        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }


        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }


        public DateTime Schedule { get; set; }

        public string DoctorNotes { get; set; }

    }
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }

        //public int SpecialityId { get; set; }

        public int DoctorId { get; set; }

        public int PatientId { get; set; }

        public DateTime Schedule { get; set; }

        public string DoctorNotes { get; set; }
        public DoctorDto Doctor { get; set; }
        public PatientDto Patient { get; set; }
    }

    public class AppointmentVM
    {
        public List<SpecialityDto> Specialities { get; set; }
        public List<PatientDto> Patients { get; set; }
    }
}