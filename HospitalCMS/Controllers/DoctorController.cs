using HospitalCMS.Models;
using Microsoft.AspNet.Identity;
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
    public class DoctorController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DoctorController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };
            client = new HttpClient(handler);
            //client.BaseAddress = new Uri("https://localhost:44305/api/");
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiServer"]);
        }

        /// <summary>
        /// Grabs the authentication cookie sent to this controller.
        /// </summary>
        private void GetApplicationCookie()
        {
            string token = "";
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }

        // GET: Doctor
        [HttpGet]
        public ActionResult List(string SearchKey = null)
        {
            string url = string.Empty;
            if (!string.IsNullOrWhiteSpace(SearchKey))
            {
                url = "DoctorData/ListDoctors/" + SearchKey + "/";
            } else
            {
                url = "DoctorData/listDoctors";
            }
            
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DoctorDto> doctors = response.Content.ReadAsAsync<IEnumerable<DoctorDto>>().Result;
            return View(doctors);
        }

        // GET: Doctor/Details/5
        public ActionResult Details(int id)
        {
            if (User.IsInRole("Doctor"))
            {
                string getbyemailurl = "DoctorData/FindDoctorByEmail/" + User.Identity.GetUserName() + "/";
                HttpResponseMessage getbyemailresponse = client.GetAsync(getbyemailurl).Result;
                if (!getbyemailresponse.IsSuccessStatusCode)
                {
                    return RedirectToAction("Create", "Doctor");
                }
                else
                {
                    DoctorDto doctorbyemail = getbyemailresponse.Content.ReadAsAsync<DoctorDto>().Result;
                    id = doctorbyemail.DoctorId;
                    return View(doctorbyemail);
                    //return RedirectToAction("Edit", "Patient", new { id = patientbyemail.PatientId });
                }
            }
            string url = "DoctorData/FindDoctor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DoctorDto doctor = response.Content.ReadAsAsync<DoctorDto>().Result;
            return View(doctor);
        }

        // GET: Doctor/Create
        [Authorize(Roles = "Admin,Doctor")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Doctor/Create
        [HttpPost]
        [Authorize(Roles = "Admin,Doctor")]
        public ActionResult Create(Doctor doctor)
        {
            try
            {
                string url = "doctorData/AddDoctor";
                string jsonpayload = JsonConvert.SerializeObject(doctor);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                HttpResponseMessage response = client.PostAsync(url, content).Result;
                
                if (response.IsSuccessStatusCode)
                {
                    DoctorDto createdDoctor = response.Content.ReadAsAsync<DoctorDto>().Result;
                    if (User.IsInRole("Doctor"))
                    {
                        return RedirectToAction("Edit", "Doctor", new { id = createdDoctor.DoctorId });
                    }
                    return RedirectToAction("list", "Doctor");
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

        // GET: Doctor/Edit/5
        [Authorize(Roles = "Admin,Doctor")]
        public ActionResult Edit(int id)
        {
            DoctorVM viewModel = new DoctorVM();
            string url = string.Empty;
            HttpResponseMessage response;
            DoctorDto selectedDoctor = new DoctorDto();

            if (User.IsInRole("Doctor"))
            {
                string getbyemailurl = "DoctorData/FindDoctorByEmail/" + User.Identity.GetUserName() + "/";
                HttpResponseMessage getbyemailresponse = client.GetAsync(getbyemailurl).Result;
                if (!getbyemailresponse.IsSuccessStatusCode)
                {
                    return RedirectToAction("Create", "Doctor");
                }
                else
                {
                    selectedDoctor = getbyemailresponse.Content.ReadAsAsync<DoctorDto>().Result;
                    id = selectedDoctor.DoctorId;
                }
            }
            else
            {
                url = "DoctorData/FindDoctor/" + id;
                response = client.GetAsync(url).Result;
                selectedDoctor = response.Content.ReadAsAsync<DoctorDto>().Result;
            }
            

            url = "SpecialityData/SpecialityAssignedToDoctor/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<SpecialityDto> specialitiesAssignedToDoctor = response.Content.ReadAsAsync<IEnumerable<SpecialityDto>>().Result;

            url = "SpecialityData/SpecialityNotAssignedToDoctor/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<SpecialityDto> specialitiesNotAssignedToDoctor = response.Content.ReadAsAsync<IEnumerable<SpecialityDto>>().Result;

            viewModel.Doctor = selectedDoctor;
            viewModel.SpecialitiesAssigned = specialitiesAssignedToDoctor;
            viewModel.SpecialitiesNotAssigned = specialitiesNotAssignedToDoctor;
            return View(viewModel);
        }

        // POST: Doctor/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin,Doctor")]
        public ActionResult Edit(int id, Doctor doctor, HttpPostedFileBase doctorPic)
        {
            try
            {
                string url = "DoctorData/UpdateDoctor/" + id;
                string jsonpayload = jss.Serialize(doctor);
                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";
                HttpResponseMessage response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode && doctorPic != null)
                {
                    url = "doctorData/UploadDoctorPic/" + id;
                    MultipartFormDataContent requestcontent = new MultipartFormDataContent();

                    HttpContent imagecontent = new StreamContent(doctorPic.InputStream);
                    requestcontent.Add(imagecontent, "EmployeePic", doctorPic.FileName);

                    response = client.PostAsync(url, requestcontent).Result;

                    if (User.IsInRole("Doctor"))
                    {
                        return RedirectToAction("Details", "Doctor", new { id = doctor.DoctorId });
                    }

                    return RedirectToAction("List");
                }
                else if (response.IsSuccessStatusCode)
                {
                    if (User.IsInRole("Doctor"))
                    {
                        return RedirectToAction("Details", "Doctor", new { id = doctor.DoctorId });
                    }
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

        //GET: /Employee/UnassociateSpeciality?id={DoctorId}&serviceId={SpecialityId}
        [HttpGet]
        [Authorize(Roles = "Admin,Doctor")]
        public ActionResult UnassociateSpeciality(int id, int specialityId)
        {
            try
            {
                string url = "DoctorData/unassociatespecialityewithDoctor/" + id + "/" + specialityId;
                HttpContent content = new StringContent("");
                content.Headers.ContentType.MediaType = "application/json";
                HttpResponseMessage response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Edit/" + id);
                }
                else
                {
                    return View("Error");
                }

            }
            catch
            {

                return View("Error");
            }
        }

        //POST: Doctor/AssociateSpeciality/{DoctorId}
        [HttpPost]
        [Authorize(Roles = "Admin,Doctor")]
        public ActionResult AssociateSpeciality(int id, int specialityId)
        {
            try
            {
                string url = "DoctorData/AssociatespecialitywithDoctor/" + id + "/" + specialityId;
                HttpContent content = new StringContent("");
                content.Headers.ContentType.MediaType = "application/json";
                HttpResponseMessage response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Edit/" + id);
                    //return Redirect($"{Url.Action("Edit", new { id = id })}#associate-form");
                }
                else
                {
                    return View("Error");
                }

            }
            catch
            {

                return View("Error");
            }
        }

        // GET: Doctor/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult ConfirmDelete(int id)
        {
            string url = "DoctorData/FindDoctor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DoctorDto doctor = response.Content.ReadAsAsync<DoctorDto>().Result;
            return View(doctor);

        } // POST: Doctor/Delete/5
            [HttpPost]
            [Authorize]
        public ActionResult Delete(int id)
        {
            try
                {
                    GetApplicationCookie();
                    string url = "DoctorData/DeleteDoctor/" + id;
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
