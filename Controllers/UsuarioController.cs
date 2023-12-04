using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plantilla_Agenda.Models;
using Plantilla_Agenda.Repositories;
using Plantilla_Agenda.Servicios;
using System.Security.Claims;

namespace Plantilla_Agenda.Controllers
{
    [Route("usuario")]
    [Authorize]
    public class UsuarioController : Controller
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly AuthenticationsService _authenticationsService;

        public UsuarioController(UsuarioRepository usuarioRepository, AuthenticationsService authenticationsService)
        {
            _usuarioRepository = usuarioRepository;
            _authenticationsService = authenticationsService;
        }

        [HttpGet("index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("registro-persona")]
        public IActionResult RegistroPersona()
        {
            return View();
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_authenticationsService.AuthenticateUser(model.NombreUsuario, model.ClaveHash))
                {
                    // Iniciar sesión correctamente
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.NombreUsuario),
                // Puedes agregar más claims según tus necesidades
            };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    // Mensaje de alerta de inicio de sesión correcto
                    TempData["Mensaje"] = "Inicio de sesión exitoso.";

                    return RedirectToAction("Index", "Usuario"); // Redirigir a la página de inicio de usuario
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Nombre de usuario o contraseña incorrectos.");
                }
            }

            // Mensaje de alerta en caso de error
            TempData["MensajeError"] = "Error al iniciar sesión. Verifica tus credenciales.";

            return View(model);
        }



        [HttpPost("registrar-admin")]
        public IActionResult RegistrarAdmin(PersonaModel persona)
        {
            // Utiliza un servicio o repositorio dedicado para el registro de persona
            // _personaRepository.RegistrarPersonaPorAdmin(persona);
            return Ok();
        }

        [HttpGet("actualizacion")]
        public IActionResult Actualizacion()
        {
            return View();
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
