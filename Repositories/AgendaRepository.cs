using MySql.Data.MySqlClient;
using Plantilla_Agenda.Data;
using Plantilla_Agenda.Models;
using System.Collections.Generic;

namespace Plantilla_Agenda.Repositories
{
    public class AgendaRepository
    {
        private readonly ContextoDB _contextoDB;
        private readonly string _connectionString;

        public AgendaRepository(ContextoDB contexto)
        {
            _contextoDB = contexto ?? throw new ArgumentNullException(nameof(contexto));
            _connectionString = _contextoDB.Conexiondb;
        }
        public int Create(Agenda agenda)
        {
            Console.WriteLine("Entrando a create Repository agenda ");
            try
            {
                using (var connection = new MySqlConnection(_contextoDB.Conexiondb))
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

        public List<Persona> GetProfesionales()
        {
            Console.WriteLine("Get Profesionales  : ");
            List<Persona> profesionales = new List<Persona>();
            const string sql = "SELECT IdPersona, PrimerNombre, SegundoNombre,PrimerApellido,SegundoApellido FROM personas";

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {

                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var persona = new Persona
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

        public List<Horario> GetHorarios()
        {
            Console.WriteLine("Get horarios  : ");
              var horarios = new List<Horario>();

    using (var connection = new MySqlConnection(_connectionString))
    {
        connection.Open();

        var query = "SELECT IdHorario, HoraInicio, HoraFin, FechaInicio, FechaFin FROM Horarios";
        using (var command = new MySqlCommand(query, connection))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var horario = new Horario
                    {
                        IdHorario = reader.GetInt32(0),
                        HoraInicio = reader.IsDBNull(1) ? (TimeSpan?)null : reader.GetTimeSpan(1),
                        HoraFin = reader.IsDBNull(2) ? (TimeSpan?)null : reader.GetTimeSpan(2),
                        FechaInicio = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3),
                        FechaFin = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4)
                    };

                    horarios.Add(horario);
                }
            }
        }
    }

    return horarios;
        }

        public List<Sede> GetSedes()
        {
            Console.WriteLine("Get sedes  : ");
            const string sql = "SELECT IdSede, Direccion  FROM sedes";

            List<Sede> sedes = new List<Sede>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {

                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var sede = new Sede
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

        public List<Servicio> GetServicios()
        {
            Console.WriteLine("Get servicios  : ");
            const string sql = "SELECT IdServicio, Nombre  FROM servicios";

            List<Servicio> servicios = new List<Servicio>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var servicio = new Servicio
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
        public List<Agenda> GetAllAgendas()
        {
            List<Agenda> agendas = new List<Agenda>();
            const string sql = "SELECT IdAgenda, IdProfesional, IdHorario, IdSede, IdServicio FROM agendas";

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var agenda = new Agenda
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

        public List<Agenda> GetAllAgendasWithDetails()
        {
            List<Agenda> agendas = new List<Agenda>();
            const string sql = "SELECT a.IdAgenda, a.IdProfesional, a.IdHorario, a.IdSede, a.IdServicio, " +
                               "p.PrimerNombre AS PrimerNombre, " +
                               "p.SegundoNombre AS SegundoNombre, " +
                               "p.PrimerApellido AS PrimerApellido, " +
                               "p.SegundoApellido AS SegundoApellido, " +
                               "s.Direccion AS Direccion, " +
                               "se.Nombre AS Nombre, " +
                               "h.HoraInicio AS HoraInicio " +
                               "FROM agendas a " +
                               "INNER JOIN profesionales p ON a.IdProfesional = p.IdPersona " +
                               "INNER JOIN sedes s ON a.IdSede = s.IdSede " +
                               "INNER JOIN servicios se ON a.IdServicio = se.IdServicio " +
                               "INNER JOIN horarios h ON a.IdHorario = h.IdHorario";

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var agenda = new Agenda
                        {
                            IdAgenda = reader.GetInt32(0),
                            IdProfesional = reader.GetInt32(1),
                            IdHorario = reader.GetInt32(2),
                            IdSede = reader.GetInt32(3),
                            IdServicio = reader.GetInt32(4),
                            Profesional = new Persona { PrimerNombre = reader.GetString(5) },
                            Sede = new Sede { Direccion = reader.GetString(6) },
                            Servicio = new Servicio { Nombre = reader.GetString(7) },
                            Horario = new Horario { HoraInicio = reader.GetTimeSpan(8) }
                        };
                        agendas.Add(agenda);
                    }
                }
            }
            return agendas;
        }


        public Agenda GetAgendaById(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var sql = "SELECT * FROM agendas WHERE IdAgenda = @IdAgenda";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@IdAgenda", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Construir y devolver la agenda
                            var agenda = new Agenda
                            {
                                IdAgenda = reader.GetInt32(0),
                                // ... otras propiedades ...
                            };
                            return agenda;
                        }
                    }
                }
            }
            return null; // Si no se encuentra la agenda
        }

        public void Delete(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var sql = "DELETE FROM agendas WHERE IdAgenda = @IdAgenda";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@IdAgenda", id);
                    command.ExecuteNonQuery();
                }
            }

        }

    }
}
