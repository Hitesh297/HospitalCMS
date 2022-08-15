using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HospitalCMS.Models
{
    public class Comment
    {
        [Key]

        public int Id { get; set; }

        public string CommentText { get; set; }

        [ForeignKey("Article")]
        public int ArticleId { get; set; }
        public virtual Article Article { get; set; }

        [ForeignKey("Doctor")]
        public int? DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }

        [ForeignKey("Patient")]
        public int? PatientId { get; set; }
        public virtual Patient Patient { get; set; }

    }

    public class CommentDto
    {

        public int Id { get; set; }

        public string CommentText { get; set; }

        public int ArticleId { get; set; }
        public  ArticleDto Article { get; set; }

        public int? DoctorId { get; set; }
        public  DoctorDto Doctor { get; set; }

        public int? PatientId { get; set; }
        public  PatientDto Patient { get; set; }

        public string DoctorEmail { get; set; }

        public string PatientEmail { get; set; }

    }


}