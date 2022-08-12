# Before starting
1. Create App_Data folder inside [HospitalCMS](HospitalCMS)
2. Run update-database


# Authors
## Hitesh
###### Appointment, Doctor, Speciality
- api for CRUD functionality of Appointments
- api for CRUD functionality of Doctor
- api for CRUD functionality of Speciality
- MVC controller for Appointment
- MVC controller for Doctor
- MVC controller for Speciality
- Added code for seeding data
- Worked on Register page for new user

## Saransh
###### Article
- api for CRUD functionality of Article
- MVC controller for Article
- View for CRUD operations on Article
- PatientDataController error fixes
- Change HTML helpers to HTML on view of Article(update & Create), Appointment(update & Create), Department(update & Create), FAQ(update & Create), Donor(update & Create), Patient(update & Create), Event(update & Create)

## Kamran
###### Department
- api for CRUD functionality of Department
- MVC controller for Department
- View for CRUD operations on Department
- Change HTML helpers to HTML on view(Delete conform, List, Detail) of all the modules.
- Created data for migrations on configuration.cs

## Vishwa
###### Event
- api for CRUD functionality of Events
- MVC controller for Event
- View for CRUD operations on Event
- Bug fix on DoctorData controller.

## Jinal
###### FAQ
- api for CRUD functionality of FAQ's
- MVC controller for FAQ
- View for CRUD operations on FAQ

## Hunny
###### Patient
- api for CRUD functionality of Patient
- MVC controller for Patient
- View for CRUD operations on Patient

# Application Overview
The application has 4 kinds of users; Doctor, Patient, Admin & Guest(not logged in)
## Patient
- User can register as a patient by selcting the role as Patient in the register form.
- On first login patient will be asked to input his/her details.
- Patient can click on 'My Details' link in navbar to view his/her details, and even edit it.
- Patient can click on 'Book Appointment' link in navbar to book an appointment with the doctor, this will be visible to the doctor when doctor logs in.
- Patient can click on 'My Appointments' link in navbar to view all the current and past appointments.
- Patient can click on 'Doctors' link in navbar to see the list of doctors and even search doctors by name or email.
- Patient can click on 'Donate' to do a donation.
- Patient can click on 'FAQs' link to view FAQ's per department.
- Patient can click on 


## Doctor

## Admin
