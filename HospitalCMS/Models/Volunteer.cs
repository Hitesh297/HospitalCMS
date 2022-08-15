using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HospitalCMS.Models
{
    public class Volunteer
    {

        [Key]
        public int VolunteerId { get; set; }
        public string VolunteerName { get; set; }
        [ForeignKey("Event")]
        public int EventId { get; set; }
        public virtual Event Event { get; set; }

        
    }
    public class VolunteerDto
    {
        public int VolunteerId { get; set; }
        public string VolunteerName { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }

    }

}
