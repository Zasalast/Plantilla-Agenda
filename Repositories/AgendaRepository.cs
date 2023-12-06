using MySql.Data.MySqlClient;
using Plantilla_Agenda.Data;
using Plantilla_Agenda.Models;
using System.Collections.Generic;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Plantilla_Agenda.Repositories
{
    public class AgendaRepository
    {
        //private readonly ContextoDB _contextoDB;
        private readonly string _connectionString;

        private readonly IConfiguration _config;

        public AgendaRepository(IConfiguration config)
        {
            _config = config;
        }
        public int Create(AgendaModel agenda)
        {
            Console.WriteLine("Entrando a create Repository agenda ");
            try
            {
                using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    Console.WriteLine("La Creo la conexion correctamente  : ");

                    string sql = "INSERT INTO agendas (IdProfesional, IdHorario, IdSede, IdServicio) " +
                                 "VALUES (@IdProfesional, @IdHorario, @IdSede, @IdServicio);" +
                                 "SELECT LAST_INSERT_ID();";  // Esta línea devuelve el ID generado
                    Console.WriteLine("El script utilizado es  : " + sql);
                    var command = new MySqlCommand(sql, connection);
                    Console.WriteLine("se va a iniciar la configuracion de datos  : ");
                    command.Parameters.AddWithValue("@IdProfesional", agenda.IdProfesional);
                    command.Parameters.AddWithValue("@IdHorario", agenda.IdHorario);
                    command.Parameters.AddWithValue("@IdSede", agenda.IdSede);
                    command.Parameters.AddWithValue("@IdServicio", agenda.IdServicio);
                    Console.WriteLine("Se agregaron los datos y se van a convertir  : ");
                    // Ejecuta la consulta y obtiene el ID generado
                    int generatedId = Convert.ToInt32(command.ExecuteScalar());

                    if (generatedId > 0)
                    {
                        Console.WriteLine("La agenda se creó correctamente con ID: " + generatedId);
                    }

                    connection.Close();
                    Console.WriteLine("La cerror la conexion correctamente  : ");
                    // Retorna el ID generado en la base de datos
                    return generatedId;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al crear la agenda: " + ex.Message);
                throw;
            }
        }


        // AgendaRepository

        public List<PersonaModel> GetProfesionales()
        {
            Console.WriteLine("Get Profesionales  : ");
            List<PersonaModel> profesionales = new List<PersonaModel>();
            const string sql = "SELECT IdPersona, PrimerNombre, PrimerApellido FROM personas";

            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {

                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var persona = new PersonaModel
                        {
                            IdPersona = reader.GetInt32(0),
                            PrimerNombre = reader.GetString(1),
                            PrimerApellido = reader.GetString(2)
                        };
                        profesionales.Add(persona);
                    }
                }
            }
            Console.WriteLine("Fin Get profesional : ");
            return profesionales;
        }

        public List<HorarioModel> GetHorarios()
        {
            Console.WriteLine("Get horarios  : ");
            List<HorarioModel> horarios = new List<HorarioModel>();
            const string sql = "SELECT IdHorario, HoraInicio, HoraFin FROM horarios";
            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                        while (reader.Read())
                        {
                            var horario = new HorarioModel
                            {
                                IdHorario = reader.GetInt32(0),
                                HoraInicio = reader.GetTimeSpan(1),
                                HoraFin = TimeSpan.Parse(reader.GetString(2))
                            };
                            horarios.Add(horario);
                        }
                }
            }
            Console.WriteLine("fin horaios  : ");
            return horarios;
        }

        public List<SedeModel> GetSedes()
        {
            Console.WriteLine("Get sedes  : ");
            const string sql = "SELECT IdSede, Direccion  FROM sedes";

            List<SedeModel> sedes = new List<SedeModel>();
            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {

                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var sede = new SedeModel
                        {
                            IdSede = reader.GetInt32(0),
                            Direccion = reader.GetString(1)
                        };
                        sedes.Add(sede);
                    }
                }
            }
            Console.WriteLine("fin get sedes : ");
            return sedes;
            // Similar para sedes
        }

        public List<ServicioModel> GetServicios()
        {
            Console.WriteLine("Get servicios  : ");
            const string sql = "SELECT IdServicio, Nombre  FROM servicios";

            List<ServicioModel> servicios = new List<ServicioModel>();
            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var servicio = new ServicioModel
                        {
                            IdServicio = reader.GetInt32(0),
                            Nombre = reader.GetString(1)
                        };
                        servicios.Add(servicio);
                    }
                }
            }
            Console.WriteLine("fin servicios  : ");
            return servicios;

            // Similar para servicios
        }
        public List<AgendaModel> GetAllAgendas()
        {
            List<AgendaModel> agendas = new List<AgendaModel>();
            const string sql = "SELECT IdAgenda, IdProfesional, IdHorario, IdSede, IdServicio FROM agendas";

            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var agenda = new AgendaModel
                        {
                            IdAgenda = reader.GetInt32(0),
                            IdProfesional = reader.GetInt32(1),
                            IdHorario = reader.GetInt32(2),
                            IdSede = reader.GetInt32(3),
                            IdServicio = reader.GetInt32(4)
                        };
                        agendas.Add(agenda);
                    }
                }
            }
            return agendas;
        }

        public List<AgendaModel> GetAllAgendasWithDetails()
        {
            PersonaModel Profesional;
            List<AgendaModel> agendas = new List<AgendaModel>();
            const string sql = "SELECT a.IdAgenda, a.IdProfesional, a.IdHorario, a.IdSede, a.IdServicio, " +
                               "p.PrimerNombre AS PrimerNombre, " +
                               "s.Direccion AS Direccion, " +
                               "se.Nombre AS Nombre, " +
                               "h.HoraInicio AS HoraInicio " +
                               "FROM agendas a " +
                               "INNER JOIN profesionales p ON a.IdProfesional = p.IdPersona " +
                               "INNER JOIN sedes s ON a.IdSede = s.IdSede " +
                               "INNER JOIN servicios se ON a.IdServicio = se.IdServicio " +
                               "INNER JOIN horarios h ON a.IdHorario = h.IdHorario";

            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    
                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var agenda = new AgendaModel
                        {
                            IdAgenda = reader.GetInt32(0),
                            IdProfesional = reader.GetInt32(1),
                            IdHorario = reader.GetInt32(2),
                            IdSede = reader.GetInt32(3),
                            IdServicio = reader.GetInt32(4),
                            Profesional = new PersonaModel { PrimerNombre = reader.GetString(5) },
                            Sede = new SedeModel { Direccion = reader.GetString(6) },
                            Servicio = new ServicioModel { Nombre = reader.GetString(7) },
                            Horario = new HorarioModel { HoraInicio = reader.GetTimeSpan(8) }
                        }; 
                        agendas.Add(agenda);
                    }
                }
            }
            return agendas;
        }


    }

}