using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PruebaASPNETEmbocador.Controllers
{
    public class InfoAppController : Controller
    {
        // GET: InfoApp
        public ActionResult Index()
        {
            return View();
        }

        // GET: InfoApp/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: InfoApp/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InfoApp/Create
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

        // GET: InfoApp/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: InfoApp/Edit/5
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

        // GET: InfoApp/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: InfoApp/Delete/5
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
