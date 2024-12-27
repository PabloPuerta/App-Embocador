using System;
using System.Linq;
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
                    Session["LoginPanel"] = "trabajador"; // Almacenar el panel desde el que se ha logueado

                    ViewBag.NombreUsuario = usuarioExistente.Nombre;

                    return View("LoginTrabajadores", usuarioExistente);
                }
                else
                {
                    TempData["ErrorMessage"] = "Contraseña incorrecta";
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "No existe ningún usuario con el nombre especificado o el nombre de usuario introducido no es correcto.";
                return RedirectToAction("Index", "Home");
            }
        }

        // Método que permite volver al dashboard del usuario trabajador
        [SessionCheck]
        public ActionResult PanelTrabajador()
        {
            string nombreUsuario = Session["NombreUsuario"] as string;

            if (string.IsNullOrEmpty(nombreUsuario))
            {
             
                return RedirectToAction("Index", "Home");
            }

            var usuarioExistente = db.Usuarios.FirstOrDefault(x => x.Nombre == nombreUsuario);

            if (usuarioExistente == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View("LoginTrabajadores", usuarioExistente);
        }




        // Método para cerrar sesión
        [HttpPost]
        public ActionResult Logout()
        {
            // Limpiar la sesión
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
