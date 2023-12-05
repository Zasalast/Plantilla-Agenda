using System.Collections.Generic;
using System.Data;
using System;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Plantilla_Agenda.Models;

namespace Plantilla_Agenda.Data
{
    public interface IAgendaRepository
    {
        Task<List<AgendaModel>> ObtenerDetallesAgendas();
        Task<AgendaModel> ObtenerDetallesAgendasPorId(int id);

        Task<AgendaModel> ObtenerDetallesAgendaPorId(int id);
        Task Create(AgendaModel agenda);
        Task UpdateAgenda(AgendaModel model);
        Task Delete(int id);
        Task Cancelar(int id, string reason);
        Task<List<AgendaListVistaModelo>> GetAgendaList();
    }
    public class AgendaRepository
    {
        private readonly IConfiguration _config;

        public AgendaRepository(IConfiguration config)
        {
            _config = config;
        }
        public async Task CreateAgenda(CreateAgendaModel model)
        {
            // 1. Validate model

            // 2. Check slot availability

            // 3. Map model to entity

            // 4. Add to repository
        }

        public async Task<AgendaModel> GetAgendaById(int id)
        {
            using (
                var connection =
                    new MySqlConnection(_config
                            .GetConnectionString("DefaultConnection"))
            )
            {
                connection.Open();
                var agenda =
                    await connection
                        .QueryFirstOrDefaultAsync
                        <AgendaModel
                        >("SELECT * FROM agendas WHERE IdAgenda = @IdAgenda",
                        new { IdAgenda = id });
                return agenda;
            
        }
        }
        public async Task<List<AgendaListVistaModelo>> GetAgendaList()
        {
            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                // Llamada a procedimiento almacenado con Dapper
                var result = await connection.QueryAsync<AgendaListVistaModelo>(
                     "NombreDeTuProcedimientoAlmacenado",
                    commandType: CommandType.StoredProcedure
                );

                return result.ToList();
            }
        }

        public int Create(AgendaModel agenda)
        {
            using (
                var connection =
                    new MySqlConnection(_config
                            .GetConnectionString("DefaultConnection"))
            )
            {
                connection.Open();
                var sql =
                    "INSERT INTO agendas (IdSede, IdServicio, IdHorario, IdProfesional) VALUES (@IdSede, @IdServicio, @IdHorario, @IdProfesional); SELECT LAST_INSERT_ID();";
                return connection.QueryFirstOrDefault<int>(sql, agenda);
            }
        }

        public int Create2(AgendaModel agenda)
        {
            try
            {
                using (
                    var connection =
                        new MySqlConnection(_config
                                .GetConnectionString("DefaultConnection"))
                )
                {
                    var parameters =
                        new {
                            IdProfesional = agenda.IdProfesional,
                            IdHorario = agenda.IdHorario,
                            IdSede = agenda.IdSede,
                            IdServicio = agenda.IdServicio
                        };

                    const string
                        sql
                        =
                        "INSERT INTO agendas (IdProfesional, IdHorario, IdSede, IdServicio) " +
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

        public List<AgendaModel> ObtenerDetallesAgenda()
        {
            int idAgenda = 0;
            string Estado = "";
            string ProfesionalNombre = "";
            string SedeDireccion = "";
            string ServicioNombre = "";
            string HorarioInicio = "";
            string HorarioFin = "";
            string idPersona = "";
            string idSede = "";
            string idServicio = "";
            string idHorario = "";

            using (
                var connection =
                    new MySqlConnection(_config
                            .GetConnectionString("DefaultConnection"))
            )
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("p_IdAgenda", idAgenda);
                parameters.Add("p_Estado", Estado);
                parameters.Add("p_PrimerNombre", ProfesionalNombre);
                parameters.Add("p_Direccion", SedeDireccion);
                parameters.Add("p_Nombre", ServicioNombre);
                parameters.Add("HoraInicio ", HorarioInicio);
                parameters.Add("HoraFin ", HorarioFin);
                parameters.Add("IdProfesional ", idPersona);
                parameters.Add("IdSede", idSede);
                parameters.Add("IdServicio", idServicio);
                parameters.Add("IdHorario", idHorario);

                // Llamada a procedimiento almacenado con Dapper
                var result =
                    connection
                        .Query<AgendaDetailsModel>("ObtenerDetallesAgenda",
                        parameters,
                        commandType: CommandType.StoredProcedure);


                return (List<AgendaModel>)result;
            }
        }
        public List<AgendaModel> ObtenerDetallesAgendaPorId(int idAgenda)
        {
            string Estado = "";
            string ProfesionalNombre = "";
            string SedeDireccion = "";
            string ServicioNombre = "";
            string HorarioInicio = "";
            string HorarioFin = "";
            string idPersona = "";
            string idSede = "";
            string idServicio = "";
            string idHorario = "";

            using (
                var connection =
                    new MySqlConnection(_config
                            .GetConnectionString("DefaultConnection"))
            )
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("p_IdAgenda", idAgenda);
                parameters.Add("p_Estado", Estado);
                parameters.Add("p_PrimerNombre", ProfesionalNombre);
                parameters.Add("p_Direccion", SedeDireccion);
                parameters.Add("p_Nombre", ServicioNombre);
                parameters.Add("HoraInicio ", HorarioInicio);
                parameters.Add("HoraFin ", HorarioFin);
                parameters.Add("IdProfesional ", idPersona);
                parameters.Add("IdSede", idSede);
                parameters.Add("IdServicio", idServicio);
                parameters.Add("IdHorario", idHorario);

                // Llamada a procedimiento almacenado con Dapper
                var result =
                    connection
                        .Query<AgendaModel>("ObtenerDetallesAgendaPorId(@idAgenda)",
                        parameters,
                        commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }

        public async Task<List<AgendaModel>> ObtenerDetallesAgendas()
        {
            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                // Use Dapper to get agenda details
                var agendas = await connection.QueryAsync<AgendaModel>(@"
                SELECT
                    A.IdAgenda,
                    A.Estado,
                    CONCAT(P.PrimerNombre, ' ', P.PrimerApellido) AS ProfesionalNombre,
                    S.Nombre AS NombreSede,
                    Se.Nombre AS NombreServicio,
                    CONCAT(C.PrimerNombre, ' ', C.PrimerApellido) AS PrimerNombreCliente,
                    H.FechaInicio,
                    H.HoraInicio
                FROM
                    Agenda A
                INNER JOIN Profesional P ON A.IdProfesional = P.IdProfesional
                INNER JOIN Sede S ON A.IdSede = S.IdSede
                INNER JOIN Servicio Se ON A.IdServicio = Se.IdServicio
                INNER JOIN Horario H ON A.IdHorario = H.IdHorario
                LEFT JOIN Cliente C ON A.IdCliente = C.IdCliente
            ");

                return agendas.ToList();
            }
        }
        public List<AgendaModel> GetAllAgendas()
        {
            List<AgendaModel> agendas = new List<AgendaModel>();
            const string sql = "SELECT IdAgenda, IdProfesional, IdHorario, IdSede, IdServicio FROM agendas";

            using (var connection = new MySqlConnection(_config
                            .GetConnectionString("DefaultConnection")))
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
      
        public void Update(AgendaModel agenda)
        {
            using (
                var connection =
                    new MySqlConnection(_config
                            .GetConnectionString("DefaultConnection"))
            )
            {
                connection.Open();
                var sql =
                    "UPDATE agendas SET IdSede = @IdSede, IdServicio = @IdServicio, IdHorario = @IdHorario, IdProfesional = @IdProfesional WHERE IdAgenda = @IdAgenda";
                connection.Execute (sql, agenda);
            }
        }

        public void Delete(int idAgenda)
        {
            using (
                var connection =
                    new MySqlConnection(_config
                            .GetConnectionString("DefaultConnection"))
            )
            {
                connection.Open();
                var sql = "DELETE FROM agendas WHERE IdAgenda = @IdAgenda";
                connection.Execute(sql, new { IdAgenda = idAgenda });
            }
        }

        public void Agendara(
            int idProfesional,
            int idHorario,
            int idCliente,
            int idSede,
            int idServicio
        )
        {
            using (
                var connection =
                    new MySqlConnection(_config
                            .GetConnectionString("DefaultConnection"))
            )
            {
                connection
                    .Execute("Agendar",
                    new {
                        p_IdProfesional = idProfesional,
                        p_IdHorario = idHorario,
                        p_IdCliente = idCliente,
                        p_IdSedeAgendada = idSede,
                        p_IdServicioAgendado = idServicio
                    },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public void Cancelar(int idAgenda, string motivo)
        {
            using (
                var connection =
                    new MySqlConnection(_config
                            .GetConnectionString("DefaultConnection"))
            )
            {
                connection
                    .Execute("Cancelar",
                    new { p_IdAgenda = idAgenda, p_Motivo = motivo },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public List<PersonaModel> GetProfesionales()
        {
            try
            {
                using (
                    var connection =
                        new MySqlConnection(_config
                                .GetConnectionString("DefaultConnection"))
                )
                {
                    connection.Open();

                    // Utilizamos Dapper para mapear los resultados a objetos Persona
                    const string
                        sql
                        =
                        "SELECT IdPersona, PrimerNombre, PrimerApellido FROM personas";
                    return (List<PersonaModel>)connection.Query<PersonaModel>(sql);
                }
            }
            catch (Exception ex)
            {
                Console
                    .WriteLine("Error al obtener profesionales: " + ex.Message);
                throw;
            }
        }

        public List<PersonaModel> GetHorarios()
        {
            try
            {
                using (
                    var connection =
                        new MySqlConnection(_config
                                .GetConnectionString("DefaultConnection"))
                )
                {
                    connection.Open();

                    // Utilizamos Dapper para mapear los resultados a objetos Horario
                    const string
                        sql
                        =
                        "SELECT HoraInicio, HoraFin, FechaInicio,Disponibilidad FROM horarios";
                    return (List<PersonaModel>)connection.Query<PersonaModel>(sql);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener Horarios: " + ex.Message);
                throw;
            }
        }

        public List<PersonaModel> GetSedes()
        {
            try
            {
                using (
                    var connection =
                        new MySqlConnection(_config
                                .GetConnectionString("DefaultConnection"))
                )
                {
                    connection.Open();

                    // Utilizamos Dapper para mapear los resultados a objetos Sede
                    const string
                        sql = "SELECT IdSede, Direccion, Nombre FROM sedes";
                    return (List<PersonaModel>)connection.Query<PersonaModel>(sql);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener Sedes: " + ex.Message);
                throw;
            }
        }

        public List<PersonaModel> GetServicios()
        {
            try
            {
                using (
                    var connection =
                        new MySqlConnection(_config
                                .GetConnectionString("DefaultConnection"))
                )
                {
                    connection.Open();

                    // Utilizamos Dapper para mapear los resultados a objetos Servicio
                    const string
                        sql
                        =
                        "SELECT Nombre, Descripcion, Estado FROM servicios";
                    return (List<PersonaModel>)connection.Query<PersonaModel>(sql);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener servicios: " + ex.Message);
                throw;
            }
        }
        public async Task<IEnumerable<AgendaModel>> ObtenerAgendass()
        {
            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var agendas = await connection.QueryAsync<AgendaModel>("SELECT * FROM agendas");
                return agendas;
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
