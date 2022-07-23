namespace HospitalCMS.Migrations
{
    using HospitalCMS.Models;
    using System;
    using System.Collections.Generic;
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

            context.Specialities.AddOrUpdate(x => x.SpecialityId,
                new Speciality() { SpecialityId = 1, Name = "Surgery" },
                new Speciality() { SpecialityId = 2, Name = "Internal Medicine" },
                new Speciality() { SpecialityId = 3, Name = "Pediatrics" },
                new Speciality() { SpecialityId = 4, Name = "Emergency Medicine" },
                new Speciality() { SpecialityId = 5, Name = "Family Medicine" },
                new Speciality() { SpecialityId = 6, Name = "Psychiatry" },
                new Speciality() { SpecialityId = 7, Name = "Dermatology" },
                new Speciality() { SpecialityId = 8, Name = "Neurology" },
                new Speciality() { SpecialityId = 9, Name = "Cardiology" },
                new Speciality() { SpecialityId = 10, Name = "Obstetrics & Gynaecology" },
                new Speciality() { SpecialityId = 11, Name = "Radiology" },
                new Speciality() { SpecialityId = 12, Name = "Orthopedics" },
                new Speciality() { SpecialityId = 13, Name = "Opthalmology" },
                new Speciality() { SpecialityId = 14, Name = "Otorhinolaryngology" },
                new Speciality() { SpecialityId = 15, Name = "General Surgery" },
                new Speciality() { SpecialityId = 16, Name = "Urology" },
                new Speciality() { SpecialityId = 17, Name = "Physical Therapy" },
                new Speciality() { SpecialityId = 18, Name = "Geriatrics" },
                new Speciality() { SpecialityId = 19, Name = "Gastroenterology" },
                new Speciality() { SpecialityId = 20, Name = "Oncology" }
                );


            context.Doctors.AddOrUpdate(x => x.DoctorId,
                    new Doctor() { DoctorId = 1, Name = "Jane Austen", Email = "jane@gmail.com", Experience = 5, Phone = "(123)654-789", Specialities = new List<Speciality>() { context.Specialities.Find(1) } },
                    new Doctor() { DoctorId = 2, Name = "Charles Dickens", Email = "charles@gmail.com", Experience = 10, Phone = "(123)654-789", Specialities = new List<Speciality>() { context.Specialities.Find(2) } },
                    new Doctor() { DoctorId = 3, Name = "Miguel de Cervantes", Email = "miguel@gmail.com", Experience = 15, Phone = "(123)654-789", Specialities = new List<Speciality>() { context.Specialities.Find(3) } },
                    new Doctor() { DoctorId = 4, Name = "Shaun Wilson", Email = "shaun@gmail.com", Experience = 7, Phone = "(123)654-789", Specialities = new List<Speciality>() { context.Specialities.Find(4) } },
                    new Doctor() { DoctorId = 5, Name = "Fouad Abaza", Email = "fouad@gmail.com", Experience = 10, Phone = "(123)654-789", Specialities = new List<Speciality>() { context.Specialities.Find(5) } },
                    new Doctor() { DoctorId = 6, Name = "Corrie Anderson", Email = "corrie@gmail.com", Experience = 12, Phone = "(123)654-789", Specialities = new List<Speciality>() { context.Specialities.Find(6) } },
                    new Doctor() { DoctorId = 7, Name = "Mark Trehan", Email = "mark@gmail.com", Experience = 5, Phone = "(123)654-789", Specialities = new List<Speciality>() { context.Specialities.Find(7) } },
                    new Doctor() { DoctorId = 8, Name = "Mona Abright", Email = "mona@gmail.com", Experience = 10, Phone = "(123)654-789", Specialities = new List<Speciality>() { context.Specialities.Find(8) } },
                    new Doctor() { DoctorId = 9, Name = "Arthur Abbed", Email = "arthur@gmail.com", Experience = 15, Phone = "(123)654-789", Specialities = new List<Speciality>() { context.Specialities.Find(9) } }
                    );

            context.Doctors.Include(x => x.Specialities).Where(y => y.DoctorId == 1).FirstOrDefault().Specialities.Add(context.Specialities.Find(18));
            context.Doctors.Include(x => x.Specialities).Where(y => y.DoctorId == 2).FirstOrDefault().Specialities.Add(context.Specialities.Find(19));
            context.Doctors.Include(x => x.Specialities).Where(y => y.DoctorId == 3).FirstOrDefault().Specialities.Add(context.Specialities.Find(20));
            context.Doctors.Include(x => x.Specialities).Where(y => y.DoctorId == 4).FirstOrDefault().Specialities.Add(context.Specialities.Find(17));
            context.Doctors.Include(x => x.Specialities).Where(y => y.DoctorId == 5).FirstOrDefault().Specialities.Add(context.Specialities.Find(16));
            context.Doctors.Include(x => x.Specialities).Where(y => y.DoctorId == 6).FirstOrDefault().Specialities.Add(context.Specialities.Find(15));
            context.Doctors.Include(x => x.Specialities).Where(y => y.DoctorId == 7).FirstOrDefault().Specialities.Add(context.Specialities.Find(14));
            context.Doctors.Include(x => x.Specialities).Where(y => y.DoctorId == 8).FirstOrDefault().Specialities.Add(context.Specialities.Find(13));
            context.Doctors.Include(x => x.Specialities).Where(y => y.DoctorId == 9).FirstOrDefault().Specialities.Add(context.Specialities.Find(12));

            context.Patients.AddOrUpdate(x => x.PatientId,
                new Patient() { PatientId = 1, FirstName = "Roy", LastName = "Isek", Gender = "Male", MaritalStatus = "N/A", Mobile = "123456789", PostalCode = "M9V 3B8", Address1 = "Anabelle Dr.", Address2 = "Willow Street", City = "London", Country = "Canada", DOB = DateTime.Now, Email = "roy@gmail.com" },
                new Patient() { PatientId = 2, FirstName = "Rids", LastName = "Mave", Gender = "Female", MaritalStatus = "Married", Mobile = "123456789", PostalCode = "M9V 3B8", Address1 = "Arctic Dr.", Address2 = "Orange Street", City = "Toronto", Country = "Canada", DOB = DateTime.Now, Email = "roy@gmail.com" },
                new Patient() { PatientId = 3, FirstName = "Olivia", LastName = "Abbed", Gender = "Female", MaritalStatus = "N/A", Mobile = "123456789", PostalCode = "M1V 3B8", Address1 = "Wilson Dr.", Address2 = "Wilson Street", City = "Milton", Country = "Canada", DOB = DateTime.Now, Email = "olivia@gmail.com" },
                new Patient() { PatientId = 4, FirstName = "Emma", LastName = "Lopez", Gender = "Female", MaritalStatus = "Married", Mobile = "123456789", PostalCode = "M2V 3B8", Address1 = "Red Sea Dr.", Address2 = "Red Sea Street", City = "Toronto", Country = "Canada", DOB = DateTime.Now, Email = "emma@gmail.com" },
                new Patient() { PatientId = 5, FirstName = "Amelia", LastName = "Taylor", Gender = "Female", MaritalStatus = "N/A", Mobile = "123456789", PostalCode = "M3V 3B8", Address1 = "Burk Dr.", Address2 = "Burk Street", City = "Oakvile", Country = "Canada", DOB = DateTime.Now, Email = "amelia@gmail.com" },
                new Patient() { PatientId = 6, FirstName = "Ava", LastName = "Lee", Gender = "Female", MaritalStatus = "Married", Mobile = "123456789", PostalCode = "M4V 3B8", Address1 = "Trees Dr.", Address2 = "Trees Street", City = "Brampton", Country = "Canada", DOB = DateTime.Now, Email = "ava@gmail.com" },
                new Patient() { PatientId = 7, FirstName = "Liam", LastName = "Perez", Gender = "Male", MaritalStatus = "N/A", Mobile = "123456789", PostalCode = "M5V 3B8", Address1 = "Rockland Dr.", Address2 = "Rockland Street", City = "London", Country = "Canada", DOB = DateTime.Now, Email = "liam@gmail.com" },
                new Patient() { PatientId = 8, FirstName = "Noah", LastName = "White", Gender = "Male", MaritalStatus = "Married", Mobile = "123456789", PostalCode = "M7V 3B8", Address1 = "Woodland Dr.", Address2 = "Woodland Street", City = "Berrie", Country = "Canada", DOB = DateTime.Now, Email = "noah@gmail.com" },
                new Patient() { PatientId = 9, FirstName = "Oliver", LastName = "Clark", Gender = "Male", MaritalStatus = "N/A", Mobile = "123456789", PostalCode = "M9V 3B8", Address1 = "Shore Dr.", Address2 = "Shore Street", City = "London", Country = "Canada", DOB = DateTime.Now, Email = "oliver@gmail.com" },
                new Patient() { PatientId = 10, FirstName = "James", LastName = "Lewis", Gender = "Male", MaritalStatus = "Married", Mobile = "123456789", PostalCode = "M0V 3B8", Address1 = "Heartland Dr.", Address2 = "Heartland Street", City = "Milton", Country = "Canada", DOB = DateTime.Now, Email = "james@gmail.com" },
                new Patient() { PatientId = 11, FirstName = "Lucas", LastName = "Walker", Gender = "Male", MaritalStatus = "N/A", Mobile = "123456789", PostalCode = "M6V 3B8", Address1 = "Maxwell Dr.", Address2 = "Maxwell Street", City = "Toronto", Country = "Canada", DOB = DateTime.Now, Email = "lucas@gmail.com" },
                new Patient() { PatientId = 12, FirstName = "Levi", LastName = "King", Gender = "Male", MaritalStatus = "Married", Mobile = "123456789", PostalCode = "B0V 3B8", Address1 = "Nutty Dr.", Address2 = "Nutty Street", City = "Toronto", Country = "Canada", DOB = DateTime.Now, Email = "levi@gmail.com" }
                );

            context.Departments.AddOrUpdate(x => x.DepartmentId,
                new Department() { DepartmentId = 1, Name = "Anaesthesia" },
                new Department() { DepartmentId = 2, Name = "Family Medicine" },
                new Department() { DepartmentId = 3, Name = "Lab Medicine & Pathobiology" },
                new Department() { DepartmentId = 4, Name = "Medical Imaging" },
                new Department() { DepartmentId = 5, Name = "Medicine" },
                new Department() { DepartmentId = 6, Name = "Obstetrics & Gynaecology" },
                new Department() { DepartmentId = 7, Name = "Ophthalmology & Vision Sciences" },
                new Department() { DepartmentId = 8, Name = "Otolaryngology Head & Neck" }
                );

            context.Appointments.AddOrUpdate(x => x.AppointmentId,
                new Appointment() { AppointmentId = 1, DoctorId = 1, DoctorNotes = "", PatientId = 1, Schedule = DateTime.Now.AddDays(1) },
                new Appointment() { AppointmentId = 2, DoctorId = 2, DoctorNotes = "", PatientId = 2, Schedule = DateTime.Now.AddDays(2) },
                new Appointment() { AppointmentId = 3, DoctorId = 3, DoctorNotes = "", PatientId = 3, Schedule = DateTime.Now.AddDays(3) },
                new Appointment() { AppointmentId = 4, DoctorId = 4, DoctorNotes = "", PatientId = 4, Schedule = DateTime.Now.AddDays(4) },
                new Appointment() { AppointmentId = 5, DoctorId = 5, DoctorNotes = "", PatientId = 5, Schedule = DateTime.Now.AddDays(5) },
                new Appointment() { AppointmentId = 6, DoctorId = 6, DoctorNotes = "", PatientId = 6, Schedule = DateTime.Now.AddDays(6) },
                new Appointment() { AppointmentId = 7, DoctorId = 7, DoctorNotes = "", PatientId = 7, Schedule = DateTime.Now.AddDays(7) },
                new Appointment() { AppointmentId = 8, DoctorId = 8, DoctorNotes = "", PatientId = 8, Schedule = DateTime.Now.AddDays(8) }
                );

            context.FAQs.AddOrUpdate(x => x.FAQId,
                new FAQ() { FAQId = 1, Question = "What is Aneasthesia?", Answer = "Anesthesia is a treatment using drugs called anesthetics. These drugs keep you from feeling pain during medical procedures. Anesthesiologists are medical doctors who administer anesthesia and manage pain. Some anesthesia numbs a small area of the body. General anesthesia makes you unconscious (asleep) during invasive surgical procedures.", DepartmentId = 1 },
                new FAQ() { FAQId = 2, Question = "How does anesthesia work?", Answer = "Anesthesia temporarily blocks sensory/pain signals from nerves to the centers in the brain. Your peripheral nerves connect the spinal cord to the rest of your body.", DepartmentId = 1 },
                new FAQ() { FAQId = 3, Question = "Who performs anesthesia?", Answer = "If you’re having a relatively simple procedure like a tooth extraction that requires numbing a small area, the person performing your procedure can administer the local anesthetic. For more complex and invasive procedures, your anesthetic will be administered by a physician anesthesiologist. This medical doctor manages your pain before, during and after surgery. In addition to your physician anesthesiologist, your anesthesia team can be comprised of physicians in training (fellows or residents), a certified registered nurse anesthetist (CRNA), or a certified anesthesiologist assistant (CAA).", DepartmentId = 1 },
                new FAQ() { FAQId = 4, Question = "How does the internal match work?", Answer = "After matching to the Family Medicine Program at the Hospital, a Virtual Open House is held approximately 1–2 weeks after the match results to provide you with more information about each of the different hospital teaching sites. Each resident will then be asked to complete a rank list of their preferred hospital teaching site. The internal match does not guarantee that residents will be matched to their preferred hospital teaching site. All those matched to the Family Medicine Residency Program will equally benefit from the strong curriculum and academic program offered as part of the experience at the Hospital. ", DepartmentId = 2 },
                new FAQ() { FAQId = 5, Question = "How much elective time does the program offer?", Answer = "Each hospital teaching site varies with respect to the amount of time allotted to electives.  The minimum would be two or three months over the two years. Many sites include selectives in the curriculum, allowing residents to select a clinical rotation from a focused list of options. There is a diverse range of electives given the tremendous opportunities for clinical work in various specialties at the Hospital.", DepartmentId = 2 },
                new FAQ() { FAQId = 6, Question = "How much elective time does the program offer?", Answer = "Each hospital teaching site varies with respect to the amount of time allotted to electives. The minimum would be two or three months over the two years. Many sites include selectives in the curriculum, allowing residents to select a clinical rotation from a focused list of options. There is a diverse range of electives given the tremendous opportunities for clinical work in various specialties at the Hospital.", DepartmentId = 2 }

                );

            context.Donors.AddOrUpdate(x => x.DonorId,
                new Donor() { DonorId = 1, Name = "Jack Ham", DepartmentId = 1, Amount = 2000.00M, Email = "jack@gmail.com", Phone = "(123)123-458" },
                new Donor() { DonorId = 2, Name = "Max Ruth", DepartmentId = 2, Amount = 3000.00M, Email = "max@gmail.com", Phone = "(123)123-458" },
                new Donor() { DonorId = 3, Name = "Samantha Obrian", DepartmentId = 3, Amount = 4000.00M, Email = "samantha@gmail.com", Phone = "(123)123-458" },
                new Donor() { DonorId = 4, Name = "Ella Flores", DepartmentId = 4, Amount = 5000.00M, Email = "Ella@gmail.com", Phone = "(123)123-458" },
                new Donor() { DonorId = 5, Name = "Avery Green", DepartmentId = 5, Amount = 3000.00M, Email = "Avery@gmail.com", Phone = "(123)123-458" },
                new Donor() { DonorId = 6, Name = "Sofia Adams", DepartmentId = 6, Amount = 4000.00M, Email = "Sofia@gmail.com", Phone = "(123)123-458" },
                new Donor() { DonorId = 7, Name = "Luna Carter", DepartmentId = 7, Amount = 1000.00M, Email = "Luna@gmail.com", Phone = "(123)123-458" },
                new Donor() { DonorId = 8, Name = "Max Evans", DepartmentId = 8, Amount = 7000.00M, Email = "Max@gmail.com", Phone = "(123)123-458" },
                new Donor() { DonorId = 9, Name = "Jack Diaz", DepartmentId = 1, Amount = 8000.00M, Email = "Jack@gmail.com", Phone = "(123)123-458" }
                );

            context.Events.AddOrUpdate(x => x.EventId,
                new Event() { EventId = 1, Date = DateTime.Now.AddDays(3), Description = "This interactive program will provide an intensive year-long introduction to current approaches and techniques to help physicians increase their knowledge and clinical skills in office counselling and psychotherapy.", HasPic = false, Title = "Counselling and Psychotherapy in Family", DepartmentId = 1 }
                );

            context.Articles.AddOrUpdate(x => x.ArticleId,
                new Article() { ArticleId = 1, Description = "This interactive program will provide an intensive year-long introduction to current approaches and techniques to help physicians increase their knowledge and clinical skills in office counselling and psychotherapy.", EventId = 1, Title = "All you need to know about Psychotherapy" }
                );



        }
    }
}
