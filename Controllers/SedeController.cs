using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plantilla_Agenda.Models;
using Plantilla_Agenda.Repositories;

namespace Plantilla_Agenda.Controllers
{
    public class SedeController : Controller
    {
        private readonly SedeRepository _sedeRepository;

        public SedeController(SedeRepository sedeRepository)
        {
            _sedeRepository = sedeRepository;
        }

        public ActionResult Index()
        {
            var servicios = _sedeRepository.ObtenerSede();
            return View(servicios);
        }
        public ActionResult List()
        {
            var servicios = _sedeRepository.ObtenerSede();
            return View(servicios);
        }

        public ActionResult Details(int id)
        {
            var servicio = _sedeRepository.ObtenerSedePorId(id);
            return View(servicio);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Sede sede)
        {
            try
            {
                _sedeRepository.CrearSede(sede);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to create sede: " + ex.Message;
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var sede = _sedeRepository.ObtenerSedePorId(id);
            return View(sede);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Sede updated)
        {
            try
            {
                _sedeRepository.ActualizarSede(updated);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to update sede: " + ex.Message;
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            var sede = _sedeRepository.ObtenerSedePorId(id);
            return View(sede);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                _sedeRepository.EliminarSede(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to delete sede: " + ex.Message;
                return View();
            }
        }
    }
}
