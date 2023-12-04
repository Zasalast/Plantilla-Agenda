namespace Plantilla_Agenda.Repositories
{
    using Microsoft.Extensions.Configuration;
    using MySql.Data.MySqlClient;
    using Dapper;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Plantilla_Agenda.Models;

    public interface IAgendamientoRepository
    {
        IEnumerable<Agendamiento> GetAgendamientos();
        Agendamiento GetAgendamientoById(int id);
        void CrearAgendamiento(Agendamiento agendamiento);
        void UpdateAgendamiento(int id, Agendamiento agendamiento);
        void DeleteAgendamiento(int id);
    }
    public class AgendamientoRepository
    {
        private readonly string _connectionString;

        public AgendamientoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<Agendamiento> GetAgendamientos()
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                return db.Query<Agendamiento>("SELECT * FROM Agendamientos");
            }
        }

        public Agendamiento GetAgendamientoById(int id)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                return db.QueryFirstOrDefault<Agendamiento>("SELECT * FROM Agendamientos WHERE IdAgendamiento = @IdAgendamiento", new { IdAgendamiento = id });
            }
        }

        public void CrearAgendamiento(Agendamiento agendamiento)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                string query = "INSERT INTO Agendamientos (Fecha, Descripcion) VALUES (@Fecha, @Descripcion)";
                db.Execute(query, agendamiento);
            }
        }

        public void UpdateAgendamiento(int id, Agendamiento agendamiento)
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
    }
}

