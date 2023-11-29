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
namespace Plantilla_Agenda.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RegistroPersona()
        {
            return View();
        }
 


        [Authorize]
        public IActionResult Actualizacion()
        {
            return View();
        }


      

    }
}
