namespace HospitalCMS.Migrations
{
    using HospitalCMS.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<HospitalCMS.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(HospitalCMS.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.


            context.Doctors.AddOrUpdate(x => x.DoctorId,
                    new Doctor() { DoctorId = 1, Name = "Jane Austen", Email = "jane@gmail.com", Experience = 5, Phone = "(123)654-789" },
                    new Doctor() { DoctorId = 2, Name = "Charles Dickens", Email = "charles@gmail.com", Experience = 10, Phone = "(123)654-789" },
                    new Doctor() { DoctorId = 3, Name = "Miguel de Cervantes", Email = "miguel@gmail.com", Experience = 15, Phone = "(123)654-789" }
                    );

            context.Patients.AddOrUpdate(x => x.PatientId,
                new Patient() { PatientId = 1, FirstName = "Roy", LastName = "Isek", Gender = "Male", MaritalStatus = "N/A", Mobile = "123456789", PostalCode = "M9V 3B8", Address1 = "Anabelle Dr.", Address2 = "Willow Street", City = "London", Country = "Canada", DOB = DateTime.Now, Email = "roy@gmail.com" },
                new Patient() { PatientId = 2, FirstName = "Rids", LastName = "Mave", Gender = "Female", MaritalStatus = "Married", Mobile = "123456789", PostalCode = "M9V 3B8", Address1 = "Arctic Dr.", Address2 = "Orange Street", City = "Toronto", Country = "Canada", DOB = DateTime.Now, Email = "roy@gmail.com" }
                );

            context.Departments.AddOrUpdate(x => x.DepartmentId,
                new Department() { DepartmentId = 1, Name = "Anaesthesia" },
                new Department() { DepartmentId = 1, Name = "Family Medicine" },
                new Department() { DepartmentId = 1, Name = "Lab Medicine & Pathobiology" },
                new Department() { DepartmentId = 1, Name = "Medical Imaging" },
                new Department() { DepartmentId = 1, Name = "Medicine" },
                new Department() { DepartmentId = 1, Name = "Obstetrics & Gynaecology" },
                new Department() { DepartmentId = 1, Name = "Ophthalmology & Vision Sciences" },
                new Department() { DepartmentId = 1, Name = "Otolaryngology Head & Neck" }
                );

            context.Appointments.AddOrUpdate(x => x.AppointmentId,
                new Appointment() { AppointmentId = 1, DoctorId = 1, DoctorNotes = "", PatientId = 1, Schedule = DateTime.Now.AddDays(1)},
                new Appointment() { AppointmentId = 2, DoctorId = 2, DoctorNotes = "", PatientId = 2, Schedule = DateTime.Now.AddDays(2) },
                new Appointment() { AppointmentId = 3, DoctorId = 3, DoctorNotes = "", PatientId = 1, Schedule = DateTime.Now.AddDays(3) },
                new Appointment() { AppointmentId = 4, DoctorId = 1, DoctorNotes = "", PatientId = 2, Schedule = DateTime.Now.AddDays(4) }
                );



        }
    }
}
