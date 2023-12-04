using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Security.Claims;
using Plantilla_Agenda.Data;
using Plantilla_Agenda.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Plantilla_Agenda.Servicios;

namespace Plantilla_Agenda.Controllers
{
    public class PersonaController : Controller
    { //utilizará Dapper y ADO.NET puro:


        private readonly ContextoDB Conexiondb;

        public PersonaController(ContextoDB contexto)
            {
            Conexiondb = contexto;
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

    

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                return View("Error!");
            }

        [Authorize]
        [HttpPost]
        public IActionResult Actualizacion(PersonaModel persona, string confirmarContrasena, string contrasenaNueva)
        {
           
                    return RedirectToAction("Home", "Home");
                
            
            return View();

        }


    }
}
