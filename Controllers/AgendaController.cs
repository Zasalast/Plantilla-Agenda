using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySql.Data.MySqlClient;
using NuGet.Protocol.Plugins;
using Plantilla_Agenda.Data;
using Plantilla_Agenda.Models;
using Plantilla_Agenda.Repositories;


namespace Plantilla_Agenda.Controllers
{
    public class AgendaController : Controller
    {
        private   MySqlCommand command;
        private   ContextoDB Conexiondb;
        private   AgendaRepository _agendaRepository;
         
        public AgendaController(ContextoDB contexto, AgendaRepository agendaRepository)
        {
            Conexiondb = contexto;
            _agendaRepository = agendaRepository;
         }

        // GET: AgendaController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AgendaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AgendaController/Create
        public ActionResult Create()
        {
            var sedes = _agendaRepository.GetSedes();
            var servicios = _agendaRepository.GetServicios();
            var horarios = _agendaRepository.GetHorarios();
            var profesionales = _agendaRepository.GetProfesionales();

            var model = new AgendaCreateViewModel
            {
                Sedes = new SelectList(sedes, "IdSede", "Direccion"),
                Servicios = new SelectList(servicios, "IdServicio", "Nombre"),
                Horarios = new SelectList(horarios, "IdHorario", "HoraInicio"),
                Personas = new SelectList(profesionales, "IdPersona", "PrimerNombre"),
            };

            return View(model);
        }


        private List<Sede> ObtenerSedes()
        {
            List<Sede> sedes = new List<Sede>();
            try
            {
                

            
                using (var connection = new MySqlConnection(Conexiondb.Conexiondb))
                {
                    connection.Open();
                    String sql = "SELECT * FROM sedes;";
                    var command = new MySqlCommand(sql, connection);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var sede = new Sede
                            {
                                IdSede = Convert.ToInt32(reader["IdSede"]),
                                Direccion = reader["Direccion"].ToString()
                            };
                            sedes.Add(sede);
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener sedes: " + ex.Message);
                TempData["Error"] = ex.Message;
            }

            return sedes;
        }

        private List<Servicio> ObtenerServicios()
        {
            List<Servicio> servicios = new List<Servicio>();

            try
            {
                using (var connection = new MySqlConnection(Conexiondb.Conexiondb))
                {
                    connection.Open();
                    String sql = "SELECT * FROM servicios;";
                    var command = new MySqlCommand(sql, connection);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var servicio = new Servicio
                            {
                                IdServicio = Convert.ToInt32(reader["IdServicio"]),
                                Nombre = reader["Nombre"].ToString()
                            };
                            servicios.Add(servicio);
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener servicios: " + ex.Message);
                TempData["Error"] = ex.Message;
            }

            return servicios;
        }

        private List<Horario> ObtenerHorarios()
        {
            List<Horario> horarios = new List<Horario>();

            try
            {
                using (var connection = new MySqlConnection(Conexiondb.Conexiondb))
                {
                    connection.Open();
                    String sql = "SELECT * FROM horarios;";
                    var command = new MySqlCommand(sql, connection);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var horario = new Horario
                            {
                                IdHorario = Convert.ToInt32(reader["IdHorario"]),
                                HoraInicio = TimeSpan.Parse(reader["HoraInicio"].ToString()),
                                HoraFin = TimeSpan.Parse(reader["HoraFin"].ToString())
                            };
                            horarios.Add(horario);
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener horarios: " + ex.Message);
                TempData["Error"] = ex.Message;
            }

            return horarios;
        }

        private List<Persona> ObtenerProfesionales()
        {
            List<Persona> profesionales = new List<Persona>();

            try
            {
                using (var connection = new MySqlConnection(Conexiondb.Conexiondb))
                {
                    connection.Open();
                    String sql = "SELECT * FROM personas;";
                    var command = new MySqlCommand(sql, connection);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var profesional = new Persona
                            {
                                IdPersona = Convert.ToInt32(reader["IdPersona"]),
                                PrimerNombre = reader["PrimerNombre"].ToString(),
                                PrimerApellido = reader["PrimerApellido"].ToString()
                            };
                            profesionales.Add(profesional);
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener profesionales: " + ex.Message);
                TempData["Error"] = ex.Message;
            }

            return profesionales;
        }

        // GET: AgendaController/List
        public ActionResult List()
        {
            return View();
        }

     

        
        // POST: AgendaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AgendaCreateViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var agenda = new Agenda();
                    agenda.IdSede = model.IdSede;
                    agenda.IdServicio = model.IdServicio;
                    using (var connection = new MySqlConnection(Conexiondb.Conexiondb))
                    {
                        connection.Open();

                        // Ajusta esta consulta para que inserte los datos de la agenda con los Id seleccionados
                        string sql = "INSERT INTO agendamientos (IdProfesional, IdHorario, IdSede,   IdServicio) " +
                                     "VALUES (@IdProfesional, @IdHorario, @IdSede, @IdServicio)";

                        var command = new MySqlCommand(sql, connection);
                        command.Parameters.AddWithValue("@IdProfesional", agenda.IdProfesional);
                        command.Parameters.AddWithValue("@IdHorario", agenda.IdHorario);
                        command.Parameters.AddWithValue("@IdSede", agenda.IdSede);
                        command.Parameters.AddWithValue("@IdServicio", agenda.IdServicio);
                       

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("El servicio se creó correctamente");
                        }

                        connection.Close();
                        return RedirectToAction(nameof(Index));
                    }
                    _agendaRepository.CrearAgenda(agenda);
                }
                
                // Si el modelo no es válido, vuelve a la vista con los errores
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine("El servicio no se creó correctamente");
                TempData["El servicio no se Creó"] = ex.Message;
                return View(model);
            }
        }

        // GET: AgendaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AgendaController/Edit/5
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

        // GET: AgendaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AgendaController/Delete/5
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
