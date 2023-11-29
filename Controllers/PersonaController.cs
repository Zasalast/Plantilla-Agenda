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
    {

       
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

            [HttpPost]
        public async Task<IActionResult> Login(Persona persona)
        {
            try
            {
                MySqlConnection cone = new(Conexiondb.Conexiondb);
                cone.Open();
                MySqlCommand cmd = new("proce_login ", cone);
                
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserName",  persona.UserName);
                cmd.Parameters.AddWithValue("@Clave", persona.Clave);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    String UserName = reader.GetString("Identificacion");
                    String Clave = reader.GetString("Clave");
                    int FKRol = reader.GetInt16("3");

                    if (UserName != null && persona.UserName != null)
                    {
                        List<Claim> c = new List<Claim>()
                        {
                            new Claim(ClaimTypes.NameIdentifier, UserName),
                            new Claim(ClaimTypes.Role, Convert.ToString(FKRol)),
                        };
                        ClaimsIdentity ci = new(c, CookieAuthenticationDefaults.AuthenticationScheme);
                        AuthenticationProperties p = new();

                        p.AllowRefresh = true;
                        p.IsPersistent = persona.MantenerActivo;


                        if (!persona.MantenerActivo)
                        {
                            p.ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60);
                        }
                        else
                        {
                            p.ExpiresUtc = DateTimeOffset.MaxValue;
                        }
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ci), p);
                        return RedirectToAction("Home", "Home");
                    }
                    else
                    {
                        ViewBag.Error = "Credenciales incorrectas o cuenta no registrada";
                    }
                }
                cone.Close();
                return View();
            }
            catch (Exception e)
            {
                ViewBag.Error = e.ToString() + "Credenciales incorrectas o cuenta no registrada";
                return View();
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                return View("Error!");
            }

        [Authorize]
        [HttpPost]
        public IActionResult Actualizacion(Persona persona, string confirmarContrasena, string contrasenaNueva)
        {
            //var identity = Int16.Parse(User.FindFirst(ClaimTypes.UserData)?.Value);
            ConfiguracionDB conf = new ConfiguracionDB(Conexiondb);
            List<MySqlParameter> lista = new List<MySqlParameter>();
            lista.Add(new MySqlParameter("@Identificacion", persona.Identificacion));
            lista.Add(new MySqlParameter("@Clave", persona.Clave));
            conf.Conectar();
            DataTable dt = conf.EjecutarConsultaDS("select * from persona where Identificacion=@Identificacion and Clave=@Clave;", lista, CommandType.Text);
            if (dt.Rows.Count > 0)
            {

                if (confirmarContrasena.Trim().Equals(contrasenaNueva.Trim()))
                {


                    string sql = "UPDATE persona SET Telefono = @Telefono, Direccion = @Direccion, Correo = @Correo, Clave = @Clave, TipoDocumento=@TipoDocumento WHERE (TipoDocumento = @TipoDocumento);";

                    List<MySqlParameter> lista2 = new List<MySqlParameter>();

                     
                    lista2.Add(new MySqlParameter("@Direccion", persona.Direccion));
                    lista2.Add(new MySqlParameter("@CORREO", persona.Correo));
                    lista2.Add(new MySqlParameter("@CONTRASENA", persona.Clave));
                    lista2.Add(new MySqlParameter("@TIPO_DOCUMTelefonoENTO", persona.Telefono));
                    lista2.Add(new MySqlParameter("@TIPO_DOCUMENTO", persona.TipoDocumento));
                    lista2.Add(new MySqlParameter("@NUMERO_DOCUMENTO", persona.Identificacion));
                    conf.Conectar();
                    conf.EjecutarOperacion(sql, lista2, CommandType.Text);
                    return RedirectToAction("Home", "Home");
                }
            }
            return View();

        }


    }
}
