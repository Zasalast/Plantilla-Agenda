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
            string sql = "INSERT INTO permisos (Nombre)\r\nVALUES ('Crear Agenda'), ('Eliminar Agenda'), ('Editar Agenda');";
            return View();
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
        public async Task<IActionResult> Login(Usuario usuario)
        {
            try
            {
                using (var connection = new MySqlConnection(_Conexiondb.Conexiondb))
                {
                    connection.Open();
                    String sql = "SELECT * FROM sedes;";
                    var command = new MySqlCommand(sql, connection);
                    
                    using (var reader = command.ExecuteReader())// using (command cmd = new("sp_validar_usuario", con))
                    {
                        // cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserName", usuario.UserName);
                        command.Parameters.AddWithValue("@Clave", usuario.Clave);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("El servicio se creó correctamente");
                        }

                        while (reader.Read())
                        {
                            String IdUsername = reader.GetString("UserName");
                            String clave = reader.GetString("Clave");
                            if (IdUsername != null && usuario.Clave != null)
                            {
                                List<Claim> c = new List<Claim>()
                        {
                            new Claim(ClaimTypes.NameIdentifier, IdUsername),
                            new Claim(ClaimTypes.Role, Convert.ToString(clave)),
                        };
                                ClaimsIdentity ci = new(c, CookieAuthenticationDefaults.AuthenticationScheme);
                                AuthenticationProperties p = new();

                                p.AllowRefresh = true;
                                p.IsPersistent = usuario.MantenerActivo;


                                if (!usuario.MantenerActivo)
                                    p.ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(1);
                                else
                                    p.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1);

                                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ci), p);
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                ViewBag.Error = "Credenciales incorrectas o cuenta no registrada.";
                            }
                        }
                       
                    }

                    connection.Close();
                    return View();
                }
            
            }
            catch (System.Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }



    }
}
