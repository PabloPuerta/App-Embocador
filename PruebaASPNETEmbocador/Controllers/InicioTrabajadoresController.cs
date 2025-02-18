using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
        public bool VerifyPassword(string inputPassword, string storedHash)
        {
            string inputHash = HashPassword(inputPassword);
            return inputHash.Equals(storedHash, StringComparison.OrdinalIgnoreCase);
        }

        // Método para validar los datos de un trabajador e iniciar sesión en el sistema
        [HttpPost]
        public ActionResult LoginTrabajadores(Usuarios IDUsuario)
        {
            using(var db = new EmbocadorEntities1())
            {
                // Verificar si existe el nombre de usuario
                var usuarioExistente = db.Usuarios.FirstOrDefault(x => x.Nombre == IDUsuario.Nombre);

                if (usuarioExistente != null)
                {
                    // Verificar si la contraseña es correcta
                    if (VerifyPassword(IDUsuario.Contraseña, usuarioExistente.Contraseña))
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
