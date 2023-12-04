using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using System.Security.Claims;
 

using Plantilla_Agenda.Models;
using Microsoft.AspNetCore.Authorization; 
using Plantilla_Agenda.Servicios;
using System.ComponentModel.DataAnnotations;
using Plantilla_Agenda.Data;
using Plantilla_Agenda.Repositories;
namespace Plantilla_Agenda.Controllers
{
    public class UsuarioController : Controller
    { //utilizará Dapper y ADO.NET puro:

        private readonly UsuarioRepository _usuarioRepository;
        private readonly PersonaRepository _personaRepository;

        public UsuarioController(UsuarioRepository usuarioRepository, PersonaRepository personaRepository)
        {
            _usuarioRepository = usuarioRepository;
            _personaRepository = personaRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RegistroPersona()
        {
            return View();
        }



        public IActionResult Login()
        {
            ClaimsPrincipal c = HttpContext.User;
            if (c.Identity != null)
            {
                if (c.Identity.IsAuthenticated)
                    return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(string usuario, string clave)
        {
            var user = _usuarioRepository.ObtenerUsuarioPorNombreUsuario(usuario);
            if (user != null && user.ClaveHash == clave)
            {
                // Login exitoso
                return RedirectToAction("Index");
            }
            else
            {
                // Mostrar error en login
                return View();
            }
        }
        [HttpPost("registrar-admin")]
        public IActionResult RegistrarAdmin(Persona persona)
        {
            _personaRepository.RegistrarPersonaPorAdmin(persona);
            return Ok();
        }

        [Authorize]
        public IActionResult Actualizacion()
        {
            return View();
        }
        
             private readonly ContextoDB _Conexiondb;
        public UsuarioController(ContextoDB Conexiondb)
        {
            _Conexiondb = Conexiondb;
        }
       

       




    }
}
