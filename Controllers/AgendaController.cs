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
            return View(new AgendaDetails
            {
                Persona = personas,
                Servicio = servicios,
                Sede = sedes
            });
        }
        public IActionResult List()
        {
            // Obtener datos
            var personas = _personaRepository.ObtenerPersonas();
            var servicios = _servicioRepository.ObtenerServicios();
            var sedes = _sedeRepository.ObtenerSede();
            var agendas = _agendaRepository.ObtenerAgendas();

            // Pasar datos al view
            return View(new AgendaDetails
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

        [HttpGet]
        public IActionResult Create()
        {
            var model = new AgendaCreateViewModel
            {
                Sedes = new SelectList(_sedeRepository.ObtenerSede(), "IdSede", "Direccion"),
                Servicios = new SelectList(_servicioRepository.ObtenerServicios(), "IdServicio", "Nombre"),
                Horarios = new SelectList(_agendaRepository.ObtenerHorarios(), "IdHorario", "HoraInicio"),
                Personas = new SelectList(_agendaRepository.GetProfesionales(), "IdPersona", "NombreCompleto")
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("IdAgenda,IdSede,IdServicio,IdHorario,IdProfesional,Estado,IdCliente,IdSedeAgendada,IdServicioAgendado,NombreCliente")] Agenda agenda)
        {
            if (ModelState.IsValid)
            {
                _agendaRepository.Create2(agenda);  // Utiliza el método Create2 o el que prefieras

                return RedirectToAction(nameof(Index));
            }

            // Recargar datos necesarios en caso de error
           

            return View(agenda);
        }

        // POST: Agenda/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AgendaCreateViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var agenda = new Agenda
                    {
                        IdSede = model.IdSede,
                        IdServicio = model.IdServicio,
                        IdHorario = model.IdHorario,
                        IdProfesional = model.IdProfesional
                    };

                    int generatedId = _agendaRepository.Create(agenda);

                    if (generatedId > 0)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Error creating agenda.");
                    }
                }
                // Repopulate dropdowns if needed
                // Repopular dropdowns en caso de error
                model.Sedes = new SelectList(_sedeRepository.ObtenerSede(), "IdSede", "Direccion");
                model.Servicios = new SelectList(_servicioRepository.ObtenerServicios(), "IdServicio", "Nombre");
                model.Horarios = new SelectList(_agendaRepository.ObtenerHorarios(), "IdHorario", "HoraInicio");
                model.Personas = new SelectList(_agendaRepository.GetProfesionales(), "IdPersona", "NombreCompleto");

                return View(model);
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
