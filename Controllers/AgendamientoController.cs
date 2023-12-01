using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Plantilla_Agenda.Data;
using Plantilla_Agenda.Models;

namespace Plantilla_Agenda.Controllers
{
    public class AgendamientoController : Controller
    {
        private readonly ContextoDB Conexiondb;

        public AgendamientoController(ContextoDB contexto)
        {
            Conexiondb = contexto;
        }
        // GET: AgendamientoController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AgendamientoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AgendamientoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // GET: AgendaController/List
        public ActionResult List()
        {
            return View();
        }
        // POST: AgendamientoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.Agendamiento agendamiento)
        {
            
            try
            {
                using (var connection = new MySqlConnection(Conexiondb.Conexiondb))
                {
                    connection.Open();
                    string sql = "INSERT INTO agendamientos (IdCliente, Fecha, Hora, Estado, IdAgenda)\r\nVALUES (@IdCliente, @Fecha, @Hora, @Estado, @IdAgenda);";

                     
                    var command = new MySqlCommand(sql, connection);
                    /* command.Parameters.AddWithValue("@IdCliente", agendamiento.IdCliente);
                     command.Parameters.AddWithValue("@Fecha", agendamiento.Fecha);
                     command.Parameters.AddWithValue("@Hora", agendamiento.Hora);
                     command.Parameters.AddWithValue("@Estado", agendamiento.Estado);
                     command.Parameters.AddWithValue("@IdAgenda", agendamiento.IdAgenda);
                    */

                    command.Parameters.AddWithValue("@IdCliente", "1");
                    command.Parameters.AddWithValue("@Fecha", "'2023-12-01'");
                    command.Parameters.AddWithValue("@Hora", "'09:00:00'");
                    command.Parameters.AddWithValue("@Estado", "'v'");
                    command.Parameters.AddWithValue("@IdAgenda", "2");
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

        // GET: AgendamientoController/Edit/5
        public ActionResult Edit(int id)
        {
            

            return View();
        }

        // POST: AgendamientoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                String sql = "UPDATE agendamientos SET Estado = 'c' WHERE IdAgendamiento = 3;"; 
                String sql2 = "DELETE FROM agendamientos\r\nWHERE IdAgendamiento = 4;";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AgendamientoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AgendamientoController/Delete/5
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
