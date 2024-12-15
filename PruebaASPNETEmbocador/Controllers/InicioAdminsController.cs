using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using PruebaASPNETEmbocador.Models;

namespace PruebaASPNETEmbocador.Controllers
{
    public class InicioAdminsController : Controller
    {

        private static EmbocadorEntities1 db = new EmbocadorEntities1();

        // GET: InicioAdmins
        public ActionResult Index()
        {
            return View();
        }

        // GET: AdministrarUsuarios
        public ActionResult AdministrarUsuarios()
        {
            var usuarios = db.Usuarios.ToList();
            return View(usuarios);
        }


        [HttpPost]
        public ActionResult Login(Usuarios IDUsuario)
        {

            // Verificar si existe el nombre de usuario
            var usuarioExistente = db.Usuarios.FirstOrDefault(x => x.Nombre == IDUsuario.Nombre);

            if (usuarioExistente != null)
            {
                // Verificar si la contraseña es correcta
                if (usuarioExistente.Contraseña == IDUsuario.Contraseña)
                {
                    ViewBag.NombreUsuario = usuarioExistente.Nombre;
                    return View();
                }
                else
                {
                    return Json(new { succes = false, message = "Contraseña incorrecta" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { succes = false, message = "No existe ningún usuario con el nombre especificado" }, JsonRequestBehavior.AllowGet);
            }
        }

    }

}
   
