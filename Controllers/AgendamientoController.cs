using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plantilla_Agenda.Data;

namespace Plantilla_Agenda.Controllers
{
    public class AgendamientoController : Controller
    {
        private readonly ContextoDB Conexiondb;

        public AgendamientoController(ContextoDB contexto)
        {
            Conexiondb = contexto;
        }
        // GET: AgendamientoController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AgendamientoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AgendamientoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // GET: AgendaController/List
        public ActionResult List()
        {
            return View();
        }
        // POST: AgendamientoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                String sql = "INSERT INTO agendamientos (IdCliente, Fecha, Hora, Estado, IdAgenda)\r\nVALUES (1, '2023-12-01', '09:00:00', 'v', 2);";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AgendamientoController/Edit/5
        public ActionResult Edit(int id)
        {
            

            return View();
        }

        // POST: AgendamientoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                String sql = "UPDATE agendamientos SET Estado = 'c' WHERE IdAgendamiento = 3;"; 
                String sql2 = "DELETE FROM agendamientos\r\nWHERE IdAgendamiento = 4;";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AgendamientoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AgendamientoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
