using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Plantilla_Agenda.Data;
using Plantilla_Agenda.Models;
using Plantilla_Agenda.Repositories;
using System;
using System.Collections.Generic;

namespace Plantilla_Agenda.Controllers
{
    public class AgendamientoController : Controller
    {
        private readonly ContextoDB _contexto;
        AgendamientoRepository _agendamientoRepository;
        public AgendamientoController(ContextoDB contexto, AgendamientoRepository agendamientoRepository)
        {
            _agendamientoRepository = agendamientoRepository;
            _contexto = contexto;
        }

        // GET: AgendamientoController
        public ActionResult Index()
        {
            // Retrieve all agendamientos from the database
            List<Agendamiento> agendamientos = _agendamientoRepository.GetAgendamientos().ToList();
            return View(agendamientos);
        }

        // GET: AgendamientoController/Details/5
        public ActionResult Details(int id)
        {
            // Retrieve the agendamiento with the specified ID from the database
            Agendamiento agendamiento = _agendamientoRepository.GetAgendamientoById(id);
            return View(agendamiento);
        }

        // GET: AgendamientoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AgendamientoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Agendamiento agendamiento)
        {
            try
            {
                // Save the new agendamiento to the database
                _agendamientoRepository.CrearAgendamiento(agendamiento);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to create agendamiento: " + ex.Message;
                return View();
            }
        }

        // GET: AgendamientoController/Edit/5
        public ActionResult Edit(int id)
        {
            // Retrieve the agendamiento with the specified ID from the database
            Agendamiento agendamiento = _agendamientoRepository.GetAgendamientoById(id);
            return View(agendamiento);
        }

        // POST: AgendamientoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Agendamiento updatedAgendamiento)
        {
            try
            {
                // Update the agendamiento in the database
                _agendamientoRepository.UpdateAgendamiento(id, updatedAgendamiento);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to update agendamiento: " + ex.Message;
                return View();
            }
        }

        // GET: AgendamientoController/Delete/5
        public ActionResult Delete(int id)
        {
            // Retrieve the agendamiento with the specified ID from the database
            Agendamiento agendamiento = _agendamientoRepository.GetAgendamientoById(id);
            return View(agendamiento);
        }

        // POST: AgendamientoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // Delete the agendamiento from the database
                _agendamientoRepository.DeleteAgendamiento(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to delete agendamiento: " + ex.Message;
                return View();
            }
        }
    }
}