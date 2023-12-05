using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySql.Data.MySqlClient;
using Plantilla_Agenda.Data;
using Plantilla_Agenda.Models;
using Plantilla_Agenda.Repositories;
using Plantilla_Agenda.Repositories;
using System.Linq;
namespace Plantilla_Agenda.Controllers
{
    public class AgendaController : Controller
    {
        private readonly AgendaRepository _agendaRepository;

        private readonly PersonaRepository _personaRepository;

        private readonly ServicioRepository _servicioRepository;

        private readonly UsuarioRepository _usuarioRepository;

        private readonly SedeRepository _sedeRepository;

        public AgendaController(
            AgendaRepository agendaRepository,
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
        public IActionResult Create()
        {
            var sedes = _agendaRepository.GetSedes();
            var servicios = _agendaRepository.GetServicios();
            var horarios = _agendaRepository.GetHorarios();
            var profesionales = _agendaRepository.GetProfesionales();

            var model = new AgendaCreateViewModel
            {
                Sedes = new SelectList(sedes, "IdSede", "Direccion"),
                Servicios = new SelectList(servicios, "IdServicio", "Nombre"),
                Horarios = new SelectList(horarios, "IdHorario", "HoraInicio"),
                Personas = new SelectList(profesionales, "IdPersona", "PrimerNombre"),
            };

            ViewBag.SelectSede = new SelectList(sedes, "IdSede", "Direccion");
            ViewBag.SelectServicio = new SelectList(servicios, "IdServicio", "Nombre");
            ViewBag.SelectHorario = new SelectList(horarios, "IdHorario", "HoraInicio");
            ViewBag.SelectPersona = new SelectList(profesionales, "IdPersona", "PrimerNombre");

            return View(model);
        }

        // GET: AgendaController
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AgendaCreateViewModel model)
        {
            Console.WriteLine(" Controlador Agenda " + "Crear agenda ");
            try
            {
                if (ModelState.IsValid)
                {

                    var agenda = new AgendaModel();
                    Console.WriteLine(" Controlador Agenda " + " model" + model);
                    agenda.IdSede = model.IdSede;
                    agenda.IdServicio = model.IdServicio;
                    agenda.IdHorario = model.IdHorario;
                    agenda.IdProfesional = model.IdProfesional;

                    int generatedId = _agendaRepository.Create(agenda);
                    Console.WriteLine(" Controlador Agenda " + " model" + generatedId);
                    if (generatedId > 0)
                    {
                        Console.WriteLine("La agenda se creó correctamente con ID: " + generatedId);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        Console.WriteLine("La inserción en la base de datos no generó un ID válido.");
                    }
                }
                else
                {
                    Console.WriteLine("El modelo no es válido. ModelState tiene errores.");
                }
                Console.WriteLine(" Controlador Agenda " + "create: Recargar los datos para los select boxes");
                // Recargar los datos para los select boxes
                var sedes = _agendaRepository.GetSedes();
                var servicios = _agendaRepository.GetServicios();
                var horarios = _agendaRepository.GetHorarios();
                var profesionales = _agendaRepository.GetProfesionales();
                Console.WriteLine(" Controlador Agenda " + "create: model.Sedes = new SelectList(sedes, \"IdSede\", \"Direccion\");");

                model.Sedes = new SelectList(sedes, "IdSede", "Direccion");
                model.Servicios = new SelectList(servicios, "IdServicio", "Nombre");
                model.Horarios = new SelectList(horarios, "IdHorario", "HoraInicio");
                model.Personas = new SelectList(profesionales, "IdPersona", "PrimerNombre");
                Console.WriteLine(" Controlador Agenda " + "create: ViewBag.SelectSede = new SelectList(sedes, \"IdSede\", \"Direccion\");");

                ViewBag.SelectSede = new SelectList(sedes, "IdSede", "Direccion");
                ViewBag.SelectServicio = new SelectList(servicios, "IdServicio", "Nombre");
                ViewBag.SelectHorario = new SelectList(horarios, "IdHorario", "HoraInicio");
                ViewBag.SelectPersona = new SelectList(profesionales, "IdPersona", "PrimerNombre");

                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en el controlador al intentar crear la agenda: " + ex.Message);
                throw;
                ViewBag.Message = "No hay agendas disponibles.";
                return View();
            }
            Console.WriteLine("Fin agenda ");

        }
        // GET: AgendaController
        public ActionResult Index()
        {
            var agendas = _agendaRepository.ObtenerDetallesAgendas();
        
            // Verifica si hay datos en la lista de agendas
            if (agendas != null)
            {
                // Pasa la lista de agendas a la vista
                return View(agendas);
            }
            else
            {
                // Puedes agregar un mensaje o manejar el caso en que no hay agendas
                ViewBag.Message = "No hay agendas disponibles.";
                return View(); // Retorna la vista sin datos
            }
        }
       
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

        public async Task<IActionResult> List()
        {
            //var agendas = await _agendaRepository.ObtenerDetallesAgendas();
            var agendas = await _agendaRepository.GetAgendaList();
            return View(agendas);
        }
        public IEnumerable<AgendaListVistaModelo> GetAgendaList()
        {
            // Logic to retrieve and map data to AgendaListVistaModelo objects
            // Replace this with your actual data retrieval logic

            var agendas = _agendaRepository.ObtenerDetallesAgendas();

            // Map the data to AgendaListVistaModelo objects
            var agendaList = agendas.Select(a => new AgendaListVistaModelo
            {
                IdAgenda = a.IdAgenda,
                ClienteNombre = a.Cliente.Nombre,
                NombreSede = a.SedeAgendada.Nombre,
                SedeDireccion = a.SedeAgendada.Direccion,
                NombreServicio = a.ServicioAgendado.Nombre,
                FechaInicio = a.FechaHoraInicio.ToString("yyyy-MM-dd"),
                HoraInicio = a.FechaHoraInicio.ToString("HH:mm")
            });

            return agendaList;
        }
        // En AgendaController

    }
}
