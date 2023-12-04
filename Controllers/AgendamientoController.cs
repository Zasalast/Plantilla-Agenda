using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly AgendamientoRepository _agendamientoRepository;
        private readonly PersonaRepository _personaRepository;

        public AgendamientoController(ContextoDB contexto, AgendamientoRepository agendamientoRepository, PersonaRepository personaRepository)
        {
            _contexto = contexto;
            _agendamientoRepository = agendamientoRepository;
            _personaRepository = personaRepository;
        }

        // GET: AgendamientoController
        public ActionResult Index()
        {
            var idUsuario = HttpContext.User.FindFirst("IdUsuario")?.Value;

            // Verificar si el IdUsuario es válido
            if (string.IsNullOrEmpty(idUsuario))
            {
                TempData["ErrorMessage"] = "Error: No se puede obtener el IdUsuario desde la sesión.";
                return View();
            }

            // Retrieve all agendamientos from the database
            List<Agendamiento> agendamientos = _agendamientoRepository.GetAgendamientos().ToList();

            // Map IdCliente to Persona's name
            foreach (var agendamiento in agendamientos)
            {
                // Assuming you have a property called NombreCliente in your Agendamiento model
                agendamiento.NombreCliente = _personaRepository.ObtenerNombrePersonaPorId(Convert.ToInt32( idUsuario));
            }

            return View(agendamientos);
        }
        public ActionResult List()
        {
            var idUsuario = HttpContext.User.FindFirst("IdUsuario")?.Value;

            // Verificar si el IdUsuario es válido
            if (string.IsNullOrEmpty(idUsuario))
            {
                TempData["ErrorMessage"] = "Error: No se puede obtener el IdUsuario desde la sesión.";
                return View();
            }

            // Retrieve all agendamientos from the database
            List<Agendamiento> agendamientos = _agendamientoRepository.GetAgendamientos().ToList();

            // Map IdCliente to Persona's name
            foreach (var agendamiento in agendamientos)
            {
                // Assuming you have a property called NombreCliente in your Agendamiento model
                agendamiento.NombreCliente = _personaRepository.ObtenerNombrePersonaPorId(Convert.ToInt32(idUsuario));
            }

            return View(agendamientos);
        }

        // GET: AgendamientoController/Details/5
        public ActionResult Details(int id)
        {
            var idUsuario = HttpContext.User.FindFirst("IdUsuario")?.Value;

            // Verificar si el IdUsuario es válido
            if (string.IsNullOrEmpty(idUsuario))
            {
                TempData["ErrorMessage"] = "Error: No se puede obtener el IdUsuario desde la sesión.";
                return View();
            }

            Agendamiento agendamiento;

            // Asignar el IdUsuario al agendamiento
            agendamiento = _agendamientoRepository.GetAgendamientoById(id);

            // Verify if the agendamiento is associated with the current user
            if (agendamiento == null || agendamiento.IdCliente != Convert.ToInt32(idUsuario))
            {
                TempData["ErrorMessage"] = "Error: El agendamiento no está asociado al usuario actual.";
                return View();
            }

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
                var idUsuario = HttpContext.User.FindFirst("IdUsuario")?.Value;

                // Verificar si el IdUsuario es válido
                if (string.IsNullOrEmpty(idUsuario))
                {
                    TempData["ErrorMessage"] = "Error: No se puede obtener el IdUsuario desde la sesión.";
                    return View();
                }

                // Asignar el IdUsuario al agendamiento
                agendamiento.IdCliente = Convert.ToInt32(idUsuario);

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

            // Populate the list of personas for dropdown
            ViewBag.Personas = new SelectList(_personaRepository.ObtenerPersonas(), "IdPersona", "NombreCompleto");

            return View(agendamiento);
        }

        // POST: AgendamientoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Agendamiento updatedAgendamiento)
        {
            try
            {
                var idUsuario = HttpContext.User.FindFirst("IdUsuario")?.Value;

                // Verificar si el IdUsuario es válido
                if (string.IsNullOrEmpty(idUsuario))
                {
                    TempData["ErrorMessage"] = "Error: No se puede obtener el IdUsuario desde la sesión.";
                    return View(updatedAgendamiento);
                }

                // Asignar el IdUsuario al agendamiento
                updatedAgendamiento.IdCliente = Convert.ToInt32(idUsuario);

                // Update the agendamiento in the database
                _agendamientoRepository.UpdateAgendamiento(id, updatedAgendamiento);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to update agendamiento: " + ex.Message;

                // Repopulate the list of personas for dropdown
                ViewBag.Personas = new SelectList(_personaRepository.ObtenerPersonas(), "IdPersona", "NombreCompleto");

                return View(updatedAgendamiento);
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