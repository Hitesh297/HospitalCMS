using HospitalCMS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HospitalCMS.Controllers
{
    public class AppointmentController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static AppointmentController()
        {
            client = new HttpClient();
            //client.BaseAddress = new Uri("https://localhost:44305/api/");
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiServer"]);
        }

        // GET: Appointment/List
        public ActionResult List()
        {
            string url = "AppointmentData/ListAppointments";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<AppointmentDto> appointments = response.Content.ReadAsAsync<IEnumerable<AppointmentDto>>().Result;
            return View(appointments);
        }

        // GET: Appointment/Details/5
        public ActionResult Details(int id)
        {
            string url = "AppointmentData/FindAppointment/"+ id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AppointmentDto appointment = response.Content.ReadAsAsync<AppointmentDto>().Result;
            return View(appointment);
        }

        // GET: Appointment/Create
        public ActionResult Create()
        {
            AppointmentVM appointmentVM = new AppointmentVM();
            string url = "SpecialityData/ListSpecialities";
            HttpResponseMessage response = client.GetAsync(url).Result;
            List<SpecialityDto> specialitys = response.Content.ReadAsAsync<List<SpecialityDto>>().Result;
            appointmentVM.Specialities = specialitys;

            url = "PatientData/ListPatients";
            response = client.GetAsync(url).Result;
            List<PatientDto> patients = response.Content.ReadAsAsync<List<PatientDto>>().Result;
            appointmentVM.Patients = patients;

            return View(appointmentVM);
        }

        // POST: Appointment/Create
        [HttpPost]
        public ActionResult Create(Appointment  appointment)
        {
            try
            {
                string url = "AppointmentData/AddAppointment";
                string jsonpayload = JsonConvert.SerializeObject(appointment);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                HttpResponseMessage response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("list", "Appointment");
                }
                else
                {
                    return View("Error");
                }
            }
            catch
            {
                return View();
            }
        }

        //// GET: Appointment/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Appointment/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: Appointment/Delete/5
        public ActionResult ConfirmDelete(int id)
        {
            string url = "AppointmentData/FindAppointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AppointmentDto appointment = response.Content.ReadAsAsync<AppointmentDto>().Result;
            return View(appointment);
        }

        // POST: Appointment/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                string url = "AppointmentData/DeleteAppointment/" + id;
                HttpContent content = new StringContent("");
                content.Headers.ContentType.MediaType = "application/json";
                HttpResponseMessage response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    return View("Error");
                }
            }
            catch
            {
                return View();
            }
        }
    }
}
