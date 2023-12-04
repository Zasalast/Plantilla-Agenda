using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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

     


        public ActionResult List()
        {
             
            var  agendamientos = _agendamientoRepository.GetAgendamientos();
             
            return View(agendamientos);
        }
        public ActionResult Index()
        {
            var idUsuario = HttpContext.User.FindFirst("IdUsuario")?.Value;

            // Verificar si el IdUsuario es válido
            if (string.IsNullOrEmpty(idUsuario) || !int.TryParse(idUsuario, out int idUsuarioInt))
            {
             
                TempData["ErrorMessage"] = "Error: IdUsuario no válido.";
                return View();
            }
            else {
                int IdUser = Convert.ToInt32(idUsuario);
                // Obtener agendamientos del cliente utilizando el procedimiento almacenado
                List<AgendamientoModel> citasCliente = _agendamientoRepository.ObtenerCitasCliente(idUsuarioInt)
                .Select(agendamiento => new AgendamientoModel
                {
                    IdAgendamiento = agendamiento.IdAgendamiento,
                    Estado = agendamiento.Estado,
                    Servicio = agendamiento.Servicio,
                    HoraInicio = agendamiento.HoraInicio,
                    HoraFin = agendamiento.HoraFin,
                    NombreProfesional = ObtenerNombreProfesional(IdUser)
                })
                .ToList();
                return View(citasCliente);
            }

            return View();
        }

        // Método para obtener el primer nombre del profesional
        private string ObtenerNombreProfesional(int idProfesional)
        {
            // Aquí debes implementar la lógica para obtener el primer nombre del profesional
            // Puedes utilizar el repositorio u otros métodos que tengas disponibles.
            // Ejemplo hipotético:
            var profesional = _personaRepository.ObtenerPersonaPorId(idProfesional);
            return profesional != null ? profesional.PrimerNombre : "Desconocido";
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

            AgendamientoModel agendamiento;

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
        public ActionResult Create(AgendamientoModel agendamiento)
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
            AgendamientoModel agendamiento = _agendamientoRepository.GetAgendamientoById(id);

            // Populate the list of personas for dropdown
            ViewBag.Personas = new SelectList(_personaRepository.ObtenerPersonas(), "IdPersona", "NombreCompleto");

            return View(agendamiento);
        }

        // POST: AgendamientoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, AgendamientoModel updatedAgendamiento)
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
            AgendamientoModel agendamiento = _agendamientoRepository.GetAgendamientoById(id);
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