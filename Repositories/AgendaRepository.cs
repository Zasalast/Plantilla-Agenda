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

      

        public async Task<AgendaModel> GetAgendaById(int id)
        {
            using (IDbConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var agenda = await connection.QueryFirstOrDefaultAsync<AgendaModel>("SELECT * FROM agendas WHERE IdAgenda = @IdAgenda", new { IdAgenda = id });
                return agenda;
            }
        }

        public int Create(AgendaModel agenda)
        {
            using (IDbConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var sql = "INSERT INTO agendas (IdSede, IdServicio, IdHorario, IdProfesional) VALUES (@IdSede, @IdServicio, @IdHorario, @IdProfesional); SELECT LAST_INSERT_ID();";
                return connection.QueryFirstOrDefault<int>(sql, agenda);
            }
        }
        public int Create2(AgendaModel agenda)
        {
            try
            {
                using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    var parameters = new
                    {
                        IdProfesional = agenda.IdProfesional,
                        IdHorario = agenda.IdHorario,
                        IdSede = agenda.IdSede,
                        IdServicio = agenda.IdServicio
                    };

                    const string sql = "INSERT INTO agendas (IdProfesional, IdHorario, IdSede, IdServicio) " +
                                       "VALUES (@IdProfesional, @IdHorario, @IdSede, @IdServicio);" +
                                       "SELECT LAST_INSERT_ID();";

                    return connection.ExecuteScalar<int>(sql, parameters);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al crear la agenda: " + ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<AgendaModel>> ObtenerAgendas()
        {
            using (IDbConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var agendas = await connection.QueryAsync<AgendaModel>("SELECT * FROM agendas");
                return agendas;
            }
        }
        public void Update(AgendaModel agenda)
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

        public IEnumerable<PersonaModel> GetProfesionales()
        {
            try
            {
                using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    // Utilizamos Dapper para mapear los resultados a objetos Persona
                    connection.Open();

                    const string sql = "SELECT IdPersona, PrimerNombre, PrimerApellido FROM personas";
                    return connection.Query<PersonaModel>(sql);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener profesionales: " + ex.Message);
                throw;
            }
        }

        public IEnumerable<PersonaModel> GetHorarios()
        {
            try
            {
                using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    // Utilizamos Dapper para mapear los resultados a objetos Horario
                    connection.Open();

                    const string sql = "SELECT IdPersona, PrimerNombre, PrimerApellido FROM horarios";
                    return connection.Query<PersonaModel>(sql);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener Horarios: " + ex.Message);
                throw;
            }
        }


        public IEnumerable<PersonaModel> GetSedes()
        {
            try
            {
                using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    // Utilizamos Dapper para mapear los resultados a objetos Sede
                    connection.Open();

                    const string sql = "SELECT IdPersona, PrimerNombre, PrimerApellido FROM sedes";
                    return connection.Query<PersonaModel>(sql);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener Sedes: " + ex.Message);
                throw;
            }
        }


        public IEnumerable<PersonaModel> GetServicios()
        {
            try
            {
                using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    // Utilizamos Dapper para mapear los resultados a objetos Servicio
                    connection.Open();

                    const string sql = "SELECT IdPersona, PrimerNombre, PrimerApellido FROM servicios";
                    return connection.Query<PersonaModel>(sql);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener servicios: " + ex.Message);
                throw;
            }
        }


        /*    string sql = "SELECT IdHorario, HoraInicio, HoraFin FROM horarios";
     
        string sql = "SELECT IdSede, Direccion FROM sedes";
 
const string sql = "SELECT IdServicio, Nombre, Descripcion, Duracion FROM servicios";
         
const string sql = "SELECT A.IdAgenda, A.Estado, S.Nombre AS Servicio, H.HoraInicio, H.HoraFin, A.IdProfesional " +
                   "FROM agendas A " +
                   "INNER JOIN horarios H ON A.IdHorario = H.IdHorario " +
                   "INNER JOIN servicios S ON A.IdServicioAgendado = S.IdServicio " +
                   "WHERE A.IdCliente = @IdCliente";


     
const string sql = "SELECT A.IdAgenda, A.Estado, S.Nombre AS Servicio, H.HoraInicio, H.HoraFin, A.IdCliente " +
                   "FROM agendas A " +
                   "INNER JOIN horarios H ON A.IdHorario = H.IdHorario " +
                   "INNER JOIN servicios S ON A.IdServicioAgendado = S.IdServicio " +
                   "WHERE A.IdProfesional = @IdProfesional";
        
         
         SELECT
    A.IdAgenda,
    A.Estado,
    CONCAT(P.PrimerNombre, ' ', P.PrimerApellido) AS ProfesionalNombre,
    S.Nombre AS SedeNombre,
    Se.Nombre AS ServicioNombre,
    CONCAT(C.PrimerNombre, ' ', C.PrimerApellido) AS ClienteNombre,
    H.FechaInicio AS FechaInicio,
    H.HoraInicio AS HoraInicio
FROM
    agendas A
INNER JOIN profesionales P ON A.IdProfesional = P.IdPersona
INNER JOIN sedes S ON A.IdSede = S.IdSede
INNER JOIN servicios Se ON A.IdServicio = Se.IdServicio
INNER JOIN horarios H ON A.IdHorario = H.IdHorario
LEFT JOIN personas C ON A.IdCliente = C.IdPersona;*/

    }
}