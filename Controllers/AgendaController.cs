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

      

    }
}
