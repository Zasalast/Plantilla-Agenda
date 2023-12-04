namespace Plantilla_Agenda.Repositories
{
    using Microsoft.Extensions.Configuration;
    using MySql.Data.MySqlClient;
    using Dapper;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Plantilla_Agenda.Models;
    using Plantilla_Agenda.Data;

    public interface IAgendamientoRepository
    {
        IEnumerable<AgendamientoModel> GetAgendamientos();
        AgendamientoModel GetAgendamientoById(int id);
        void CrearAgendamiento(AgendamientoModel agendamiento);
        void UpdateAgendamiento(int id, AgendamientoModel agendamiento);
        void DeleteAgendamiento(int id);
    }
    public class AgendamientoRepository
    {
        private readonly string _connectionString;
        private readonly ContextoDB _dbContext;
        public AgendamientoRepository(IConfiguration configuration, ContextoDB dbContext)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _dbContext = dbContext;
        }

        public IEnumerable<AgendamientoModel> GetAgendamientos()
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                return db.Query<AgendamientoModel>("SELECT * FROM Agendamientos");
            }
        }

        public AgendamientoModel GetAgendamientoById(int id)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                return db.QueryFirstOrDefault<AgendamientoModel>("SELECT * FROM Agendamientos WHERE IdAgendamiento = @IdAgendamiento", new { IdAgendamiento = id });
            }
        }

        public void CrearAgendamiento(AgendamientoModel agendamiento)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                string query = "INSERT INTO Agendamientos (Fecha, Descripcion) VALUES (@Fecha, @Descripcion)";
                db.Execute(query, agendamiento);
            }
        }

        public void UpdateAgendamiento(int id, AgendamientoModel agendamiento)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                string query = "UPDATE Agendamientos SET FechaHora = @FechaHora, Estado = @Estado WHERE IdAgendamiento = @IdAgendamiento";
                db.Execute(query, new { IdAgendamiento = id, agendamiento.FechaHora, agendamiento.Estado });
            }
        }

        public void DeleteAgendamiento(int id)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                string query = "DELETE FROM Agendamientos WHERE IdAgendamiento = @IdAgendamiento";
                db.Execute(query, new { IdAgendamiento = id });
            }
        }
        public List<AgendamientoModel> ObtenerCitasCliente(int idCliente)
        {
            using (var connection = new MySqlConnection("tu cadena de conexión"))
            {
                connection.Open();

                var result = connection.Query<AgendamientoModel>("NombreDeTuProcedimientoAlmacenado",
                    new { p_IdCliente = idCliente },
                    commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }

    }
}

