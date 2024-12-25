using System;
using System.Linq;
using System.Web.Mvc;
using PruebaASPNETEmbocador.Models;

namespace PruebaASPNETEmbocador.Controllers
{
    public class FicharTurnosController : Controller
    {
        private EmbocadorEntities1 db = new EmbocadorEntities1();

        // GET: FicharTurnos
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Confirmacion()
        {
            return View("Confirmacion");
        }

        // Acción para registrar la entrada
        [HttpPost]
        public ActionResult RegistrarEntrada()
        {
            int idUsuario = (int)Session["IdUsuario"];
            var usuario = db.Usuarios.Find(idUsuario);
            if (usuario != null)
            {
                // Verificar si el usuario ya ha fichado en las últimas 24 horas
                var ultimaEntrada = db.TurnosTrabajadoresEmbocador
                    .Where(t => t.IDUsuario == usuario.IDUsuario && t.RegistroEntrada != null)
                    .OrderByDescending(t => t.RegistroEntrada)
                    .FirstOrDefault();

                if (ultimaEntrada != null && ultimaEntrada.RegistroEntrada > DateTime.Now.AddHours(-24))
                {
                    // Almacenar el mensaje de error en TempData
                    TempData["Mensaje"] = "No puedes fichar una nueva entrada hasta que hayan pasado 24 horas desde la ultima entrada.";
                    TempData["HoraFecha"] = ultimaEntrada.RegistroEntrada.ToString();
                    return RedirectToAction("PanelTrabajador", "InicioTrabajadores");
                }

                var turno = new TurnosTrabajadoresEmbocador
                {
                    IDUsuario = usuario.IDUsuario,
                    RegistroEntrada = DateTime.Now
                };
                db.TurnosTrabajadoresEmbocador.Add(turno);
                db.SaveChanges();

                // Almacenar el mensaje de confirmación en TempData
                TempData["Mensaje"] = "Entrada registrada correctamente.";
                TempData["HoraFecha"] = turno.RegistroEntrada.ToString();
            }
            return RedirectToAction("PanelTrabajador", "InicioTrabajadores");
        }



        // Acción para registrar la salida
        [HttpPost]
        public ActionResult RegistrarSalida()
        {
            int idUsuario = (int)Session["IdUsuario"];
            var usuario = db.Usuarios.Find(idUsuario);
            if (usuario != null)
            {
                // Verificar si el usuario ya ha fichado una salida en las últimas 24 horas
                var ultimaSalida = db.TurnosTrabajadoresEmbocador
                    .Where(t => t.IDUsuario == usuario.IDUsuario && t.RegistroSalida != null)
                    .OrderByDescending(t => t.RegistroSalida)
                    .FirstOrDefault();

                if (ultimaSalida != null && ultimaSalida.RegistroSalida > DateTime.Now.AddHours(-24))
                {
                    // Almacenar el mensaje de error en TempData
                    TempData["Mensaje"] = "No puedes fichar una nueva salida hasta que hayan pasado 24 horas desde la ultima salida.";
                    TempData["HoraFecha"] = ultimaSalida.RegistroSalida.ToString();
                    return RedirectToAction("PanelTrabajador", "InicioTrabajadores");
                }

                // Buscar el turno actual del usuario
                var turno = db.TurnosTrabajadoresEmbocador
                    .Where(t => t.IDUsuario == usuario.IDUsuario && t.RegistroSalida == null)
                    .OrderByDescending(t => t.RegistroEntrada)
                    .FirstOrDefault();

                if (turno != null)
                {
                    // Actualizar el campo RegistroSalida con la hora actual
                    turno.RegistroSalida = DateTime.Now;
                    db.SaveChanges();

                    // Almacenar el mensaje de confirmación en TempData
                    TempData["Mensaje"] = "Salida registrada correctamente.";
                    TempData["HoraFecha"] = turno.RegistroSalida.ToString();
                }
            }
            return RedirectToAction("PanelTrabajador", "InicioTrabajadores");
        }


    }
}
