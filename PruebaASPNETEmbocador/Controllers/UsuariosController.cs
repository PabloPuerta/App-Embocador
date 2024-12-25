using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PruebaASPNETEmbocador.Models;

namespace PruebaASPNETEmbocador.Controllers
{
    public class UsuariosController : Controller
    {
        private EmbocadorEntities1 db = new EmbocadorEntities1();

        // GET: Usuarios
        public ActionResult Index()
        {
            if (Session["IsAdmin"] != null && (bool)Session["IsAdmin"])
            {
                return View(db.Usuarios.ToList());
            }
            else
            {
                return RedirectToAction("LoginTrabajadores", "InicioTrabajadores");
            }
        }

        // GET: Usuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuarios usuarios = db.Usuarios.Find(id);
            if (usuarios == null)
            {
                return HttpNotFound();
            }
            return View(usuarios);
        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDUsuario,Nombre,Contraseña,IsAdmin")] Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                db.Usuarios.Add(usuarios);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(usuarios);
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuarios usuarioExistente = db.Usuarios.Find(id);
            if (usuarioExistente == null)
            {
                return HttpNotFound();
            }
            return View(usuarioExistente);
        }

        // POST: Usuarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDUsuario,Nombre,Contraseña,IsAdmin")] Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                var usuarioExistente = db.Usuarios.Find(usuarios.IDUsuario);
                if (usuarioExistente == null)
                {
                    return HttpNotFound();
                }

                if (!(bool)Session["IsAdmin"] && usuarioExistente.IsAdmin != usuarios.IsAdmin)
                {
                    // Si el usuario no es administrador y ha intentado modificar la casilla IsAdmin, mostrar un mensaje de error
                    ViewBag.ErrorMessage = "Tu usuario no tiene privilegios de administrador, no puede modificar los permisos de los usuarios. Contacte con un administrador si necesita ayuda";
                    return View(usuarioExistente);
                }

                usuarioExistente.Nombre = usuarios.Nombre;
                usuarioExistente.Contraseña = usuarios.Contraseña;
                usuarioExistente.IsAdmin = usuarios.IsAdmin;

                db.Entry(usuarioExistente).State = EntityState.Modified;
                db.SaveChanges();

                // Actualizar la información del usuario en la sesión
                Session["NombreUsuario"] = usuarioExistente.Nombre;

                if (Session["IsAdmin"] != null && (bool)Session["IsAdmin"])
                {
                    return RedirectToAction("PanelAdmin", "InicioAdmins");
                }
                else
                {
                    return RedirectToAction("PanelTrabajador", "InicioTrabajadores", new { id = usuarioExistente.IDUsuario });
                }
            }
            return View(usuarios);
        }


        // GET: Usuarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuarios usuarios = db.Usuarios.Find(id);
            if (usuarios == null)
            {
                return HttpNotFound();
            }
            return View(usuarios);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuarios usuarios = db.Usuarios.Find(id);
            db.Usuarios.Remove(usuarios);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}


