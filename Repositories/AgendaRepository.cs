using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Plantilla_Agenda.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Plantilla_Agenda.Data
{
    public class AgendaRepository
    {
        private readonly IConfiguration _config;

        public AgendaRepository(IConfiguration config)
        {
            _config = config;
        }

      

        public async Task<Agenda> GetAgendaById(int id)
        {
            using (IDbConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var agenda = await connection.QueryFirstOrDefaultAsync<Agenda>("SELECT * FROM agendas WHERE IdAgenda = @IdAgenda", new { IdAgenda = id });
                return agenda;
            }
        }

        public int Create(Agenda agenda)
        {
            using (IDbConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var sql = "INSERT INTO agendas (IdSede, IdServicio, IdHorario, IdProfesional) VALUES (@IdSede, @IdServicio, @IdHorario, @IdProfesional); SELECT LAST_INSERT_ID();";
                return connection.QueryFirstOrDefault<int>(sql, agenda);
            }
        }
        public int Create2(Agenda agenda)
        {
            try
            {
                using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    string sql = "INSERT INTO agendas (IdProfesional, IdHorario, IdSede, IdServicio) " +
                                 "VALUES (@IdProfesional, @IdHorario, @IdSede, @IdServicio);" +
                                 "SELECT LAST_INSERT_ID();";

                    var command = new MySqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@IdProfesional", agenda.IdProfesional);
                    command.Parameters.AddWithValue("@IdHorario", agenda.IdHorario);
                    command.Parameters.AddWithValue("@IdSede", agenda.IdSede);
                    command.Parameters.AddWithValue("@IdServicio", agenda.IdServicio);

                    int generatedId = Convert.ToInt32(command.ExecuteScalar());

                    if (generatedId > 0)
                    {
                        Console.WriteLine("La agenda se creó correctamente con ID: " + generatedId);
                    }

                    connection.Close();

                    return generatedId;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al crear la agenda: " + ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<Agenda>> ObtenerAgendas()
        {
            using (IDbConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var agendas = await connection.QueryAsync<Agenda>("SELECT * FROM agendas");
                return agendas;
            }
        }
        public void Update(Agenda agenda)
        {
            using (IDbConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var sql = "UPDATE agendas SET IdSede = @IdSede, IdServicio = @IdServicio, IdHorario = @IdHorario, IdProfesional = @IdProfesional WHERE IdAgenda = @IdAgenda";
                connection.Execute(sql, agenda);
            }
        }


        public void Delete(int idAgenda)
        {
            using (IDbConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var sql = "DELETE FROM agendas WHERE IdAgenda = @IdAgenda";
                connection.Execute(sql, new { IdAgenda = idAgenda });
            }
        }

        public void Agendara(int idProfesional, int idHorario, int idCliente, int idSede, int idServicio)
        {
            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Execute("Agendar", new
                {
                    p_IdProfesional = idProfesional,
                    p_IdHorario = idHorario,
                    p_IdCliente = idCliente,
                    p_IdSedeAgendada = idSede,
                    p_IdServicioAgendado = idServicio
                }, commandType: CommandType.StoredProcedure);
            }
        }

        public void Cancelar(int idAgenda, string motivo)
        {
            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Execute("Cancelar", new
                {
                    p_IdAgenda = idAgenda,
                    p_Motivo = motivo
                }, commandType: CommandType.StoredProcedure);
            }
        }

        public List<Persona> GetProfesionales()
        {
            try
            {
                using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    // Utilizamos Dapper para mapear los resultados a objetos Persona
                    string sql = "SELECT IdPersona, PrimerNombre, PrimerApellido FROM personas";
                    var profesionales = connection.Query<Persona>(sql).ToList();

                    connection.Close();

                    return profesionales;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener profesionales: " + ex.Message);
                throw;
            }
        }

        public List<Horario> ObtenerHorarios()
        {
            try
            {
                using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    // Utilizamos Dapper para mapear los resultados a objetos Horario
                    string sql = "SELECT IdHorario, HoraInicio, HoraFin FROM horarios";
                    var horarios = connection.Query<Horario>(sql).ToList();

                    connection.Close();

                    return horarios;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener horarios: " + ex.Message);
                throw;
            }
        }

        public List<Sede> ObtenerSedes()
        {
            try
            {
                using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    // Utilizamos Dapper para mapear los resultados a objetos Sede
                    string sql = "SELECT IdSede, Direccion FROM sedes";
                    var sedes = connection.Query<Sede>(sql).ToList();

                    connection.Close();

                    return sedes;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener sedes: " + ex.Message);
                throw;
            }
        }
        // En la clase AgendaRepository

        public List<Servicio> GetServicios()
        {
            Console.WriteLine("Get servicios  : ");
            const string sql = "SELECT IdServicio, Nombre, Descripcion, Duracion FROM servicios";

            List<Servicio> servicios = new List<Servicio>();
            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
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
                            Nombre = reader.GetString(1),
                            Descripcion = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            Duracion = reader.GetTimeSpan(3)
                        };
                        servicios.Add(servicio);
                    }
                }
            }
            Console.WriteLine("Fin Get servicios : ");
            return servicios;
        }

        public List<AgendaCreateViewModel> GetCitasCliente(int idCliente)
        {
            Console.WriteLine("Get citas cliente  : ");
            const string sql = "SELECT A.IdAgenda, A.Estado, S.Nombre AS Servicio, H.HoraInicio, H.HoraFin, A.IdProfesional " +
                               "FROM agendas A " +
                               "INNER JOIN horarios H ON A.IdHorario = H.IdHorario " +
                               "INNER JOIN servicios S ON A.IdServicioAgendado = S.IdServicio " +
                               "WHERE A.IdCliente = @IdCliente";

            List<AgendaCreateViewModel> citas = new List<AgendaCreateViewModel>();
            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@IdCliente", idCliente);

                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var cita = new AgendaCreateViewModel
                        {
                            IdAgenda = reader.GetInt32(0),
                            Estado = reader.GetString(1),
                            Servicio = reader.GetString(2),
                            HoraInicio = reader.GetTimeSpan(3),
                            HoraFin = reader.GetTimeSpan(4),
                            IdProfesional = reader.GetInt32(5)
                        };
                        citas.Add(cita);
                    }
                }
            }
            Console.WriteLine("Fin Get citas cliente : ");
            return citas;
        }

        public List<AgendaCreateViewModel> GetCitasProfesional(int idProfesional)
        {
            Console.WriteLine("Get citas profesional  : ");
            const string sql = "SELECT A.IdAgenda, A.Estado, S.Nombre AS Servicio, H.HoraInicio, H.HoraFin, A.IdCliente " +
                               "FROM agendas A " +
                               "INNER JOIN horarios H ON A.IdHorario = H.IdHorario " +
                               "INNER JOIN servicios S ON A.IdServicioAgendado = S.IdServicio " +
                               "WHERE A.IdProfesional = @IdProfesional";

            List<AgendaCreateViewModel> citas = new List<AgendaCreateViewModel>();
            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@IdProfesional", idProfesional);

                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var cita = new AgendaCreateViewModel
                        {
                            IdAgenda = reader.GetInt32(0),
                            Estado = reader.GetString(1),
                            Servicio = reader.GetString(2),
                            HoraInicio = reader.GetTimeSpan(3),
                            HoraFin = reader.GetTimeSpan(4),
                            IdCliente = reader.GetInt32(5)
                        };
                        citas.Add(cita);
                    }
                }
            }
            Console.WriteLine("Fin Get citas profesional : ");
            return citas;
        }

    }
}