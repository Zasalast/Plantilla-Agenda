using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Plantilla_Agenda.Models;
using Plantilla_Agenda.Repositories;
using System;
using System.Collections.Generic;
using Plantilla_Agenda.Repositories;
using MySql.Data.MySqlClient;
using Plantilla_Agenda.Data;
namespace Plantilla_Agenda.Controllers
{
    public class AgendaController : Controller
    {
        private readonly AgendaRepository _agendaRepository;
        private readonly PersonaRepository _personaRepository;
        private readonly ServicioRepository _servicioRepository;
        private readonly UsuarioRepository _usuarioRepository;
        private readonly SedeRepository _sedeRepository;
        public AgendaController(AgendaRepository agendaRepository,
                                PersonaRepository personaRepository,
                                ServicioRepository servicioRepository,
                                UsuarioRepository usuarioRepository,
                                SedeRepository sedeRepository   
            )
        {
            _agendaRepository = agendaRepository;
            _personaRepository = personaRepository;
            _servicioRepository = servicioRepository;
            _usuarioRepository = usuarioRepository;
            _sedeRepository = sedeRepository;
        }





        // GET: Agenda/Index

        public IActionResult Index()
        {
            // Obtener datos
            var personas = _personaRepository.ObtenerPersonas();
            var servicios = _servicioRepository.ObtenerServicios();
            var sedes = _sedeRepository.ObtenerSede();
            var agendas = _agendaRepository.ObtenerAgendas();

            // Pasar datos al view
            return View(new AgendaDetailsModel
            {
                Persona = personas,
                Servicio = servicios,
                Sede = sedes
            });
        }
        


        /*  public async Task<IActionResult> Index()
          {
              var agendas = await _agendaRepository.ObtenerAgendas();
              return View(agendas);
          }*/

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agenda = await _agendaRepository.GetAgendaById(id.Value);

            if (agenda == null)
            {
                return NotFound();
            }

            return View(agenda);
        }

        public async Task<IActionResult> List2()
        {
            var agendas = await _agendaRepository.ObtenerAgendas();
            var agendaViewModels = agendas.Select(a => new AgendaViewModel
            {
                IdAgenda = a.IdAgenda,
                Estado = a.Estado,
                ProfesionalNombre = $"{a.PrimerNombreProfesional} {a.PrimerApellidoProfesional}",
                SedeNombre = a.NombreSede,
                ServicioNombre = a.NombreServicio,
                ClienteNombre = $"{a.PrimerNombreCliente} {a.PrimerApellidoCliente}",
                FechaInicio = a.FechaInicio,
                HoraInicio = a.HoraInicio.TimeOfDay
            }).ToList();

            return View(agendaViewModels);
        }



        [HttpGet]
        public IActionResult Create()
        {
            var model = new AgendaCreateViewModel
            {
                Sedes = new SelectList(_agendaRepository.GetSedes(), "IdSede", "Nombre"),
                Servicios = new SelectList(_agendaRepository.GetServicios(), "IdServicio", "Nombre"),
                Horarios = new SelectList(_agendaRepository.GetHorarios(), "IdHorario", "HoraInicio"),
                Profesionales = new SelectList(_agendaRepository.GetProfesionales(), "IdPersona", "NombreCompleto")
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AgendaCreateViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var agenda = new AgendaModel
                    {
                        IdSede = model.IdSede,
                        IdServicio = model.IdServicio,
                        IdHorario = model.IdHorario,
                        IdProfesional = model.IdProfesional
                    };

                    _agendaRepository.Create(agenda);

                    return RedirectToAction("Index");
                }

                // Repopulate dropdowns if needed
                model.Sedes = new SelectList(_agendaRepository.GetSedes(), "IdSede", "Nombre");
                model.Servicios = new SelectList(_agendaRepository.GetServicios(), "IdServicio", "Nombre");
                model.Horarios = new SelectList(_agendaRepository.GetHorarios(), "IdHorario", "HoraInicio");
                model.Profesionales = new SelectList(_agendaRepository.GetProfesionales(), "IdPersona", "NombreCompleto");

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                return View(model);
            }
        }

    }
}
