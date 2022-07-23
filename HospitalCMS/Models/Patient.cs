using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HospitalCMS.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DOB { get; set; }

        public string Gender { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }

        public string MaritalStatus { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string PostalCode { get; set; }
    }


    public class PatientDto
    {
        public int PatientId { get; set; }

        [Display(Name = "Patient First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Patient Last Name")]
        public string LastName { get; set; }

        public DateTime DOB { get; set; }

        public string Gender { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }

        public string MaritalStatus { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string PostalCode { get; set; }
    }



}