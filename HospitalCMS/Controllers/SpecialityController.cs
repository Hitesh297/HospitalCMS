﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HospitalCMS.Controllers
{
    public class SpecialityController : Controller
    {
        // GET: Speciality
        public ActionResult Index()
        {
            return View();
        }

        // GET: Speciality/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Speciality/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Speciality/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Speciality/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Speciality/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Speciality/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Speciality/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
