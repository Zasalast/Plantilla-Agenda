using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Plantilla_Agenda.Data;
using Plantilla_Agenda.Models;
using Plantilla_Agenda.Servicios;
using System.Data;

namespace Plantilla_Agenda.Controllers
{
    public class ServicioController : Controller
    {
        private readonly ContextoDB Conexiondb;


        public ServicioController(ContextoDB contexto)
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
        public ActionResult Create(Models.Servicio servicio)
        {
            try
            {
                using (var connection = new MySqlConnection(Conexiondb.Conexiondb))
                {
                    connection.Open();
                 
                    String sql = "INSERT INTO servicios (Nombre) VALUES (@Nombre);";
                    var command = new MySqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@Nombre", servicio.Nombre);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                  
                        Console.WriteLine("El servicio se creo correctamente");
                    }
                    connection.Close();
                 
                    return RedirectToAction("Index", "home");
          
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("El servicio no se creo correctamente");
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
