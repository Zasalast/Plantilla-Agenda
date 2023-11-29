using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Plantilla_Agenda.Data;
using Plantilla_Agenda.Models;
using Plantilla_Agenda.Servicios;
using System.Data;

namespace Plantilla_Agenda.Controllers
{
    public class ServiciosController : Controller
    {
        private readonly ContextoDB Conexiondb;


        public ServiciosController(ContextoDB contexto)
        {
            Conexiondb = contexto;
        }
        // GET: ServicioController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ServicioController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ServicioController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ServicioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.Servicios servicio)
        {
            try
            {
                ConfiguracionDB conf = new(Conexiondb);
                String sql = "INSERT INTO servicios ( Nombre) VALUES (@Nombre);";
                List<MySqlParameter> lista = new List<MySqlParameter>();
                
                lista.Add(new MySqlParameter("@Nombre", servicio.Nombre));
         
                conf.conec();
                conf.EjecutarOperacion(sql, lista, CommandType.Text);
                return RedirectToAction("Home", "Home");
                //return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                TempData["El servicio se no se Creo"] = ex.Message;
                return View();
            }
        }

        // GET: ServicioController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ServicioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ServicioController/Delete/5
        public ActionResult Delete(int idServicio)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Conexiondb.Conexiondb))
                {
                    connection.Open();

                    string query = "DELETE FROM Servicios WHERE IdServicio = @IdServicio";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdServicio", idServicio);

                        command.ExecuteNonQuery();
                    }
                }

                // Mensaje de alerta en la ventana del navegador
               
                // Mensaje en la consola del servidor
                Console.WriteLine("El servicio se eliminó correctamente");

                return View();
            }
            catch (Exception ex)
            {
                 
                TempData["El servicio No se eliminó correctamente"] = ex.Message;
                return View();
            }
        }

        // POST: ServicioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
