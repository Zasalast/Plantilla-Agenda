using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Plantilla_Agenda.Data;
using Plantilla_Agenda.Models;
using System.Data;

namespace Plantilla_Agenda.Controllers
{
    public class CuentaController : Controller
    { //utilizará Dapper y ADO.NET puro:
        private readonly ContextoDB _contexto;

        public CuentaController(ContextoDB contexto)
        {
            _contexto = contexto;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Validar credenciales
                if (ValidarCredenciales(model.NombreUsuario, model.ClaveHash))
                {
                    // Autenticación exitosa
                    var claims = new[]
                    {
                        // Puedes agregar más claims según necesites
                        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, model.NombreUsuario)
                    };

                    var identity = new System.Security.Claims.ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new System.Security.Claims.ClaimsPrincipal(identity);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.Recordarme
                    };

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Nombre de usuario o contraseña incorrectos.");
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // Método para validar las credenciales
        private bool ValidarCredenciales(string nombreUsuario, string clave)
        {
            // Lógica para validar en la base de datos
            using MySqlConnection connection = new MySqlConnection(_contexto.Conexiondb);
            connection.Open();

            using MySqlCommand cmd = new MySqlCommand("IniciarSesion", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@InNombreUsuario", nombreUsuario);
            cmd.Parameters.AddWithValue("@InClave", clave);

            using MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                // Usuario autenticado
                return true;
            }

            return false;
        }
    }
}
