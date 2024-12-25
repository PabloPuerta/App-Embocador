using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PruebaASPNETEmbocador.Filters;
using PruebaASPNETEmbocador.Models;

namespace PruebaASPNETEmbocador.Controllers
{
    public class InicioTrabajadoresController : Controller
    {

        private static EmbocadorEntities1 db = new EmbocadorEntities1();

        // GET: InicioTrabajadores
        public ActionResult Index()
        {
            return View();
        }
    
    // Método para validar los datos de un trabajador e iniciar sesión en el sistema
    [HttpPost]

    public ActionResult LoginTrabajadores(Usuarios IDUsuario)
        {
            // Verificar si existe el nombre de usuario
            var usuarioExistente = db.Usuarios.FirstOrDefault(x => x.Nombre == IDUsuario.Nombre);

            if (usuarioExistente != null)
            {
                // Verificar si la contraseña es correcta
                if (usuarioExistente.Contraseña == IDUsuario.Contraseña)
                {
                    // Almacenar la información del usuario en la sesión
                    Session["IdUsuario"] = usuarioExistente.IDUsuario;
                    Session["NombreUsuario"] = usuarioExistente.Nombre;
                    Session["IsAdmin"] = usuarioExistente.IsAdmin;

                    ViewBag.NombreUsuario = usuarioExistente.Nombre;
                    return View(usuarioExistente);
                }
                else
                {
                    return Json(new { succes = false, message = "Contraseña incorrecta" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { succes = false, message = "No existe ningún usuario con el nombre especificado o el nombre de usuario introducido no es correcto." }, JsonRequestBehavior.AllowGet);
            }
        }


        // Método que permite volver al dashboard del usuario administrador
        [SessionCheck]
        public ActionResult PanelTrabajador()
        {
            ViewBag.NombreUsuario = Session["NombreUsuario"];
            string nombreUsuario = Session["NombreUsuario"] as string;
            var usuarioExistente = db.Usuarios.FirstOrDefault(x => x.Nombre == nombreUsuario);
            return View("LoginTrabajadores", usuarioExistente);
        }



        // Método para cerrar sesión
        [HttpPost]
     public ActionResult Logout()
     {
     // Limpiar la sesión
     Session.Clear();
     return RedirectToAction("Index" , "Home");
        }

    }
}