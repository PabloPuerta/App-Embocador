using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PruebaASPNETEmbocador.Filters;
using PruebaASPNETEmbocador.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace PruebaASPNETEmbocador.Controllers
{
    [SessionCheck]
    public class TurnosHorariosController : Controller
    {
        private EmbocadorEntities1 db = new EmbocadorEntities1();

        // GET: TurnosHorarios
        public ActionResult Index(string searchString, DateTime? searchDate, int page = 1, int pageSize = 31)
        {
            var turnos = from t in db.TurnosHorarios
                         select t;

            if (!string.IsNullOrEmpty(searchString))
            {
                turnos = turnos.Where(t => t.Usuarios.Nombre.Contains(searchString));
            }

            if (searchDate.HasValue)
            {
                if (DateTime.TryParse(searchDate.ToString(), out DateTime date))
                {
                    turnos = turnos.Where(t => t.Fecha == date);
                }
            }

            var totalRecords = turnos.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var turnosPaged = turnos.OrderBy(t => t.Fecha)
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchString = searchString;
            ViewBag.SearchDate = searchDate;

            return View(turnosPaged);
        }

        // GET: TurnosHorarios/Create
        public ActionResult Create()
        {
            ViewBag.IDUsuario = new SelectList(db.Usuarios, "IDUsuario", "Nombre");
            return View();
        }

        // POST: TurnosHorarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDTurnoHorario,IDUsuario,Fecha,HoraInicio,HoraFin")] TurnosHorarios turnosHorarios)
        {
            if (ModelState.IsValid)
            {
                // Asegúrate de que solo se guarden las horas y los minutos
                turnosHorarios.HoraInicio = new TimeSpan(turnosHorarios.HoraInicio.Hours, turnosHorarios.HoraInicio.Minutes, 0);
                turnosHorarios.HoraFin = new TimeSpan(turnosHorarios.HoraFin.Hours, turnosHorarios.HoraFin.Minutes, 0);

                db.TurnosHorarios.Add(turnosHorarios);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDUsuario = new SelectList(db.Usuarios, "IDUsuario", "Nombre", turnosHorarios.IDUsuario);
            return View(turnosHorarios);
        }

        // GET: TurnosHorarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TurnosHorarios turnosHorarios = db.TurnosHorarios.Find(id);
            if (turnosHorarios == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDUsuario = new SelectList(db.Usuarios, "IDUsuario", "Nombre", turnosHorarios.IDUsuario);
            return View(turnosHorarios);
        }

        // POST: TurnosHorarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDTurnoHorario,IDUsuario,Fecha,HoraInicio,HoraFin")] TurnosHorarios turnosHorarios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(turnosHorarios).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDUsuario = new SelectList(db.Usuarios, "IDUsuario", "Nombre", turnosHorarios.IDUsuario);
            return View(turnosHorarios);
        }

        // GET: TurnosHorarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TurnosHorarios turnosHorarios = db.TurnosHorarios.Find(id);
            if (turnosHorarios == null)
            {
                return HttpNotFound();
            }
            return View(turnosHorarios);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TurnosHorarios turnosHorarios = db.TurnosHorarios.Find(id);
            db.TurnosHorarios.Remove(turnosHorarios);
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


        public ActionResult ExportToPdf(string searchString, DateTime? searchDate)
        {
            var turnosHorarios = db.TurnosHorarios.Include(t => t.Usuarios).AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                turnosHorarios = turnosHorarios.Where(t => t.Usuarios.Nombre.Contains(searchString));
            }

            if (searchDate.HasValue)
            {
                turnosHorarios = turnosHorarios.Where(t => DbFunctions.TruncateTime(t.Fecha) == DbFunctions.TruncateTime(searchDate.Value));
            }

            var turnosList = turnosHorarios.ToList();

            using (MemoryStream stream = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();

                // Título del documento
                Paragraph title = new Paragraph("Turnos y horarios Embocador", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16));
                title.Alignment = Element.ALIGN_CENTER;
                pdfDoc.Add(title);

                // Fecha de generación del documento
                Paragraph date = new Paragraph("Documento generado con fecha: " + DateTime.Now.ToString("dd/MM/yyyy"), FontFactory.GetFont(FontFactory.HELVETICA, 12));
                date.Alignment = Element.ALIGN_CENTER;
                pdfDoc.Add(date);

                // Nombre del usuario que generó el documento
                string nombreUsuario = Session["NombreUsuario"] != null ? Session["NombreUsuario"].ToString() : "Desconocido";
                Paragraph user = new Paragraph("Generado por: " + nombreUsuario, FontFactory.GetFont(FontFactory.HELVETICA, 12));
                user.Alignment = Element.ALIGN_CENTER;
                pdfDoc.Add(user);

                pdfDoc.Add(new Paragraph(" ")); // Espacio en blanco

                // Tabla
                PdfPTable table = new PdfPTable(4);
                table.WidthPercentage = 100;

                // Fuente para los encabezados de la tabla
                Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10); // Ajustar el tamaño de la fuente

                // Encabezados de la tabla
                PdfPCell cell = new PdfPCell(new Phrase("Nombre del trabajador", headerFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 5;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Fecha de trabajo", headerFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 5;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Hora de inicio del turno", headerFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 5;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Hora de finalización del turno", headerFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 5;
                table.AddCell(cell);

                // Datos de la tabla
                Font dataFont = FontFactory.GetFont(FontFactory.HELVETICA, 10); // Ajustar el tamaño de la fuente para los datos

                foreach (var item in turnosList)
                {
                    cell = new PdfPCell(new Phrase(item.Usuarios.Nombre, dataFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.Fecha.ToString("dd/MM/yyyy"), dataFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.HoraInicio.ToString(), dataFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.HoraFin.ToString(), dataFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Padding = 5;
                    table.AddCell(cell);
                }

                pdfDoc.Add(table);
                pdfDoc.Close();

                return File(stream.ToArray(), "application/pdf", "TurnosHorarios.pdf");
            }
        }
    }
}
