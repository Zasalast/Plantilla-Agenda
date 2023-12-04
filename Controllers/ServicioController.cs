// ServicioController.cs
using Microsoft.AspNetCore.Mvc;
using Plantilla_Agenda.Models;
using Plantilla_Agenda.Repositories;
using System;

namespace Plantilla_Agenda.Controllers
{
    public class ServicioController : Controller
    {
        private readonly ServicioRepository _servicioRepository;

        public ServicioController(ServicioRepository servicioRepository)
        {
            _servicioRepository = servicioRepository;
        }

        public ActionResult Index()
        {
            var servicios = _servicioRepository.ObtenerServicios();
            return View(servicios);
        }

        public ActionResult Details(int id)
        {
            var servicio = _servicioRepository.ObtenerServicioPorId(id);
            return View(servicio);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Servicio servicio)
        {
            try
            {
                _servicioRepository.CrearServicio(servicio);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to create servicio: " + ex.Message;
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var servicio = _servicioRepository.ObtenerServicioPorId(id);
            return View(servicio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Servicio updatedServicio)
        {
            try
            {
                _servicioRepository.ActualizarServicio(updatedServicio);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to update servicio: " + ex.Message;
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            var servicio = _servicioRepository.ObtenerServicioPorId(id);
            return View(servicio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                _servicioRepository.EliminarServicio(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to delete servicio: " + ex.Message;
                return View();
            }
        }
    }
}