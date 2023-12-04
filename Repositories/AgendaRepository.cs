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

        public async Task<IEnumerable<Agenda>> ObtenerAgendas()
        {
            using (IDbConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var agendas = await connection.QueryAsync<Agenda>("SELECT * FROM Agenda");
                return agendas;
            }
        }

        public async Task<Agenda> GetAgendaById(int id)
        {
            using (IDbConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var agenda = await connection.QueryFirstOrDefaultAsync<Agenda>("SELECT * FROM Agenda WHERE IdAgenda = @Id", new { Id = id });
                return agenda;
            }
        }

        public int CrearAgenda(Agenda agenda)
        {
            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "INSERT INTO agendas (Campo1, Campo2, ...) VALUES (@Val1, @Val2, ...)";
                return connection.Execute(sql, agenda);
            }
        }

        public void ActualizarAgenda(Agenda agenda)
        {
            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "UPDATE agendas SET Campo1 = @Val1, ...";
                connection.Execute(sql, agenda);
            }
        }

        public void EliminarAgenda(int id)
        {
            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "DELETE FROM agendas WHERE IdAgenda = @Id";
                connection.Execute(sql, new { Id = id });
            }
        }

        public void AgendarCita(int idProfesional, int idHorario, int idCliente, int idSede, int idServicio)
        {
            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Execute("AgendarCita", new
                {
                    p_IdProfesional = idProfesional,
                    p_IdHorario = idHorario,
                    p_IdCliente = idCliente,
                    p_IdSedeAgendada = idSede,
                    p_IdServicioAgendado = idServicio
                }, commandType: CommandType.StoredProcedure);
            }
        }

        public void CancelarCita(int idAgenda, string motivo)
        {
            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Execute("CancelarCita", new
                {
                    p_IdAgenda = idAgenda,
                    p_Motivo = motivo
                }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}