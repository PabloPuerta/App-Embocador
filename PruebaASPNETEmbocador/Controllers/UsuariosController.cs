using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDUsuario,Nombre,Contraseña,IsAdmin")] Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                usuarios.Contraseña = HashPassword(usuarios.Contraseña);
                db.Usuarios.Add(usuarios);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(usuarios);
        }

       
        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(int? id, string loginPanel)
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
            ViewBag.LoginPanel = loginPanel; // Pasar la variable loginPanel a la vista
            return View(usuarioExistente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDUsuario,Nombre,Contraseña,IsAdmin")] Usuarios usuarios, string loginPanel)
        {
            if (ModelState.IsValid)
            {
                var usuarioExistente = db.Usuarios.Find(usuarios.IDUsuario);
                if (usuarioExistente == null)
                {
                    return HttpNotFound();
                }

                // Mantener el valor original de IsAdmin si no se ha modificado manualmente
                if (usuarioExistente.IsAdmin != usuarios.IsAdmin && !(bool)Session["IsAdmin"])
                {
                    usuarios.IsAdmin = usuarioExistente.IsAdmin;
                }

                usuarioExistente.Nombre = usuarios.Nombre;
                usuarioExistente.Contraseña = HashPassword(usuarios.Contraseña);

                // Solo actualizar IsAdmin si el usuario es administrador
                if ((bool)Session["IsAdmin"])
                {
                    usuarioExistente.IsAdmin = usuarios.IsAdmin;
                }

                db.Entry(usuarioExistente).State = EntityState.Modified;
                db.SaveChanges();

                // Establecer el mensaje en TempData
                TempData["Mensaje"] = "Usuario actualizado correctamente. Por favor, vuelva a iniciar sesión.";

                // Cerrar la sesión
                Session.Clear();

                // Redirigir a la página de inicio de sesión
                return RedirectToAction("Index", "Home");
            }
            ViewBag.LoginPanel = loginPanel; // Pasar la variable loginPanel a la vista
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
