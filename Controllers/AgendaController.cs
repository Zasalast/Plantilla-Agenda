using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using MySql.Data.MySqlClient;
using NuGet.Protocol.Plugins;
using Plantilla_Agenda.Data;
using Plantilla_Agenda.Models;
using Plantilla_Agenda.Repositories;


namespace Plantilla_Agenda.Controllers
{
    public class AgendaController : Controller
    {
        private AgendaRepository _agendaRepository;
        private readonly ContextoDB _contextoDB;

        public AgendaController(AgendaRepository agendaRepository, ContextoDB contextoDB)
        {
            _agendaRepository = agendaRepository;
            _contextoDB = contextoDB;
        }
        public IActionResult Create()
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

            ViewBag.SelectSede = new SelectList(sedes, "IdSede", "Direccion");
            ViewBag.SelectServicio = new SelectList(servicios, "IdServicio", "Nombre");
            ViewBag.SelectHorario = new SelectList(horarios, "IdHorario", "HoraInicio");
            ViewBag.SelectPersona = new SelectList(profesionales, "IdPersona", "PrimerNombre");

            return View(model);
        }

        // GET: AgendaController
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AgendaCreateViewModel model)
        {
            Console.WriteLine(" Controlador Agenda " + "Crear agenda ");
            try
            {
                if (ModelState.IsValid)
                {

                    var agenda = new Agenda();
                    Console.WriteLine(" Controlador Agenda " + " model" + model);
                    agenda.IdSede = model.IdSede;
                    agenda.IdServicio = model.IdServicio;
                    agenda.IdHorario = model.IdHorario;
                    agenda.IdProfesional = model.IdProfesional;

                    int generatedId = _agendaRepository.Create(agenda);
                    Console.WriteLine(" Controlador Agenda " + " model" + generatedId);
                    if (generatedId > 0)
                    {
                        Console.WriteLine("La agenda se creó correctamente con ID: " + generatedId);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        Console.WriteLine("La inserción en la base de datos no generó un ID válido.");
                    }
                }
                else
                {
                    Console.WriteLine("El modelo no es válido. ModelState tiene errores.");
                }
                Console.WriteLine(" Controlador Agenda " + "create: Recargar los datos para los select boxes");
                // Recargar los datos para los select boxes
                var sedes = _agendaRepository.GetSedes();
                var servicios = _agendaRepository.GetServicios();
                var horarios = _agendaRepository.GetHorarios();
                var profesionales = _agendaRepository.GetProfesionales();
                Console.WriteLine(" Controlador Agenda " + "create: model.Sedes = new SelectList(sedes, \"IdSede\", \"Direccion\");");

                model.Sedes = new SelectList(sedes, "IdSede", "Direccion");
                model.Servicios = new SelectList(servicios, "IdServicio", "Nombre");
                model.Horarios = new SelectList(horarios, "IdHorario", "HoraInicio");
                model.Personas = new SelectList(profesionales, "IdPersona", "PrimerNombre");
                Console.WriteLine(" Controlador Agenda " + "create: ViewBag.SelectSede = new SelectList(sedes, \"IdSede\", \"Direccion\");");

                ViewBag.SelectSede = new SelectList(sedes, "IdSede", "Direccion");
                ViewBag.SelectServicio = new SelectList(servicios, "IdServicio", "Nombre");
                ViewBag.SelectHorario = new SelectList(horarios, "IdHorario", "HoraInicio");
                ViewBag.SelectPersona = new SelectList(profesionales, "IdPersona", "PrimerNombre");

                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en el controlador al intentar crear la agenda: " + ex.Message);
                throw;
            }
            Console.WriteLine("Fin agenda ");

        }
        // GET: AgendaController
        public ActionResult Index()
            {
            var agendas = _agendaRepository.GetAllAgendas();
            return View(agendas);
            // Verifica si hay datos en la lista de agendas
            if (agendas != null && agendas.Any())
                {
                    // Pasa la lista de agendas a la vista
                    return View(agendas);
                }
                else
                {
                    // Puedes agregar un mensaje o manejar el caso en que no hay agendas
                    ViewBag.Message = "No hay agendas disponibles.";
                    return View(); // Retorna la vista sin datos
                }
            }


         

       
       



        private List<Sede> ObtenerSedes()
        {
            Console.WriteLine(" Controlador Agenda " + " obt sedes");
            List<Sede> sedes = new List<Sede>();
            try
            {



                using (var connection = new MySqlConnection(_contextoDB.Conexiondb))
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
            Console.WriteLine(" Controlador Agenda " + "obt sedes fin ");
            return sedes;
        }

        private List<Servicio> ObtenerServicios()
        {
            Console.WriteLine(" Controlador Agenda " + " obt servicios");
            List<Servicio> servicios = new List<Servicio>();

            try
            {
                using (var connection = new MySqlConnection(_contextoDB.Conexiondb))
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
            Console.WriteLine(" Controlador Agenda " + "fin obt servicios ");
            return servicios;
        }

        private List<Horario> ObtenerHorarios()
        {
            Console.WriteLine(" Controlador Agenda " + " obt horario");
            List<Horario> horarios = new List<Horario>();

            try
            {
                using (var connection = new MySqlConnection(_contextoDB.Conexiondb))
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
            Console.WriteLine(" Controlador Agenda " + " fin obt horario");
            return horarios;
        }

        private List<Persona> ObtenerProfesionales()
        {
            Console.WriteLine(" Controlador Agenda " + " obt profesional");
            List<Persona> profesionales = new List<Persona>();

            try
            {
                using (var connection = new MySqlConnection(_contextoDB.Conexiondb))
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
                Console.WriteLine(" Controlador Agenda "  +" ");
                Console.WriteLine("Error al obtener profesionales: " + ex.Message);
                TempData["Error"] = ex.Message;
            }
            Console.WriteLine(" Controlador Agenda " + " fin optener profesional");
            return profesionales;
        }


        // GET: AgendaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        public ActionResult List()
        {
            var agendas = _agendaRepository.GetAllAgendas();
            return View(agendas);
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
