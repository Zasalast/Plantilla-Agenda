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
                Personas = new SelectList(profesionales, "IdPersona", "PrimerNombre" ),
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



          private AgendaCreateViewModel ObtenerDetallesAgenda(int id)
        {
            // Obtener la agenda por ID
            var agenda = _agendaRepository.GetAgendaById(id);

            if (agenda == null)
            {
                // Manejo de error o redirección, según sea necesario
                return null;
            }

            // Obtener detalles de otras tablas usando los IDs de la agenda
            var sede = _agendaRepository.GetSedes().FirstOrDefault(s => s.IdSede == agenda.IdSede);
            var servicio = _agendaRepository.GetServicios().FirstOrDefault(s => s.IdServicio == agenda.IdServicio);
            var horario = _agendaRepository.GetHorarios().FirstOrDefault(h => h.IdHorario == agenda.IdHorario);
            var profesional = _agendaRepository.GetProfesionales().FirstOrDefault(p => p.IdPersona == agenda.IdProfesional);

            // Crear el modelo de vista
            var modelo = new AgendaCreateViewModel
            {
                IdAgenda = agenda.IdAgenda,
                // Otras propiedades de la agenda...

                NombreSede = sede?.Direccion,
                NombreServicio = servicio?.Nombre,
                HoraInicioHorario = horario?.HoraInicio,
                NombreProfesional = profesional?.PrimerNombre,

                // Otras propiedades detalladas...
            };

            return modelo;
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
                                SegundoNombre = reader["SegundoNombre"].ToString(),
                                PrimerApellido = reader["PrimerApellido"].ToString(),
                                SegundoApellido = reader["SegundoApellido"].ToString(),
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

        public ActionResult ListaAgendasConDetalles()
        {
            try
            {
                // Obtener las agendas con detalles desde el repositorio
                var agendasConDetalles = _agendaRepository.GetAllAgendasWithDetails();

                // Crear una lista para almacenar los modelos de vista
                List<AgendaCreateViewModel> modelos = new List<AgendaCreateViewModel>();

                // Iterar sobre cada agenda con detalles
                foreach (var agenda in agendasConDetalles)
                {
                    // Obtener los IDs de la agenda actual
                    int idSede = agenda.IdSede.GetValueOrDefault();
                    int idServicio = agenda.IdServicio.GetValueOrDefault();
                    int idHorario = agenda.IdHorario.GetValueOrDefault();
                    int idProfesional = agenda.IdProfesional.GetValueOrDefault();

                    // Obtener los detalles de las otras tablas usando los IDs
                    var sede = _agendaRepository.GetSedes().FirstOrDefault(s => s.IdSede == idSede);
                    var servicio = _agendaRepository.GetServicios().FirstOrDefault(s => s.IdServicio == idServicio);
                    var horario = _agendaRepository.GetHorarios().FirstOrDefault(h => h.IdHorario == idHorario);
                    var profesional = _agendaRepository.GetProfesionales().FirstOrDefault(p => p.IdPersona == idProfesional);

                    // Crear el modelo de vista y agregarlo a la lista
                    var modelo = new AgendaCreateViewModel
                    {
                        IdSede = idSede,
                        Direccion = sede?.Direccion,
                        IdServicio = idServicio,
                        Nombre = servicio?.Nombre,
                        IdHorario = idHorario,
                        HoraInicio = horario?.HoraInicio,
                        HoraFin = horario?.HoraFin,
                        IdProfesional = idProfesional,
                        PrimerNombre = profesional?.PrimerNombre,
                        SegundoNombre = profesional?.SegundoNombre,
                        PrimerApellido = profesional?.PrimerApellido,
                        SegundoApellido = profesional?.SegundoApellido
                        // Agrega aquí más propiedades según sea necesario
                    };

                    modelos.Add(modelo);
                }

                // Pasa la lista de modelos de vista a la vista
                return View(modelos);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener la lista de agendas con detalles: " + ex.Message);
                ViewBag.ErrorMessage = "Ocurrió un error al obtener la lista de agendas con detalles.";
                return View(new List<AgendaCreateViewModel>()); // Devolver una lista vacía en caso de error
            }
        }

        // GET: AgendaController/Details/5
        public ActionResult Details(int id)
        {
            // Obtener el modelo de detalles para la agenda con el ID proporcionado
            var modelo = ObtenerDetallesAgenda(id);

            // Verificar si la agenda con el ID proporcionado existe
            if (modelo == null)
            {
                // Puedes manejar la situación cuando la agenda no existe, por ejemplo, redirigir o mostrar un mensaje de error
                return RedirectToAction(nameof(Index)); // Redirigir a la página principal u otra acción
            }

            // Pasar el modelo a la vista
            return View(modelo);
        }

        // GET: AgendaController/List
        public ActionResult List()
        {
            // Obtener la lista de agendas con detalles desde el repositorio
            var agendasConDetalles = _agendaRepository.GetAllAgendasWithDetails();

            // Crear una lista para almacenar los modelos de vista
            var modelos = new List<AgendaCreateViewModel>();

            // Iterar sobre cada agenda con detalles y crear los modelos de vista
            foreach (var agenda in agendasConDetalles)
            {

                var sede = agenda.Sede.Direccion; 
                var servicio = agenda.Servicio.Nombre;
                var horariof = agenda.Horario.FechaInicio;
                var horarioh = agenda.Horario.HoraInicio;
                var profesional = agenda.Profesional.NombreCompleto; 

                // ... (lógica para crear el modelo AgendaCreateViewModel)
                var modelo = new AgendaCreateViewModel
                {
                    IdAgenda = agenda.IdAgenda,
                    Direccion = sede,
                    Nombre = servicio,
                    FechaInicio= horariof,
                    HoraInicio = horarioh,
                    PrimerNombre = profesional,
                  
                };

                modelos.Add(modelo);
            }

            // Devolver la vista con la lista de modelos
            return View(modelos);
        }


        // GET: AgendaController/Edit/5
        public ActionResult Edit(int id)
        {
            // Obtener el modelo de detalles para la agenda con el ID proporcionado
            var modelo = ObtenerDetallesAgenda(id);

            // Verificar si la agenda con el ID proporcionado existe
            if (modelo == null)
            {
                // Puedes manejar la situación cuando la agenda no existe, por ejemplo, redirigir o mostrar un mensaje de error
                return RedirectToAction(nameof(Index)); // Redirigir a la página principal u otra acción
            }

            // Pasar el modelo a la vista
            return View(modelo);
        }

        // POST: AgendaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            // Obtener el modelo de detalles para la agenda con el ID proporcionado
            var modelo = ObtenerDetallesAgenda(id);

            // Verificar si la agenda con el ID proporcionado existe
            if (modelo == null)
            {
                // Puedes manejar la situación cuando la agenda no existe, por ejemplo, redirigir o mostrar un mensaje de error
                return RedirectToAction(nameof(Index)); // Redirigir a la página principal u otra acción
            }

            try
            {
                // Lógica para la edición de la agenda (si es necesario)
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                // Puedes manejar errores aquí
                return View();
            }
        }



        // GET: AgendaController/Delete/5
        // GET: AgendaController/Delete/5
        public ActionResult Delete(int id)
        {
            var modelo = ObtenerDetallesAgenda(id);

            if (modelo == null)
            {
                return NotFound();
            }

            return View(modelo); // Pasar un solo elemento a la vista
        }


        // POST: AgendaController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var modelo = ObtenerDetallesAgenda(id);

                using (var connection = new MySqlConnection(_contextoDB.Conexiondb))
                {
                    connection.Open();

                    var sql = "DELETE FROM agendas WHERE IdAgenda = @IdAgenda";
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IdAgenda", id);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // La agenda se eliminó correctamente, redirige a la página principal u otra página deseada.
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            // No se encontró la agenda para eliminar, redirige a una vista de error.
                            return RedirectToAction(nameof(ErrorViewModel));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Loguea el error y redirige a una vista de error.
                Console.WriteLine($"Error al eliminar la agenda con ID {id}: {ex.Message}");
                return RedirectToAction(nameof(ErrorViewModel));
            }
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
