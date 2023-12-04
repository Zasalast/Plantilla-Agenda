// ServicioRepository.cs
using MySql.Data.MySqlClient;
using Plantilla_Agenda.Models;
using System;
using System.Collections.Generic;
using Dapper;

namespace Plantilla_Agenda.Repositories
{
    public class SedeRepository
    {
        private readonly string _connectionString;

        public SedeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Sede> ObtenerSede()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM sedes";
                return connection.Query<Sede>(sql).AsList();
            }
        }

        public Sede ObtenerSedePorId(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM sedes WHERE IdSede = @IdSede";
                return connection.QueryFirstOrDefault<Sede>(sql, new { IdServicio = id });
            }
        }

        public void CrearSede(Sede sede)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                var sql = "INSERT INTO sedes (Nombre, Descripcion, Duracion, Estado) VALUES (@Nombre, @Descripcion, @Duracion, @Estado)";
                connection.Execute(sql, sede);
            }
        }

        public void ActualizarSede(Sede sede)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                var sql = "UPDATE sedes SET Nombre = @Nombre, Descripcion = @Descripcion, Duracion = @Duracion, Estado = @Estado WHERE IdSede = @IdSede";
                connection.Execute(sql, sede);
            }
        }

        public void EliminarSede(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                var sql = "DELETE FROM sedes WHERE IdSede = @IdSede";
                connection.Execute(sql, new { IdSede = id });
            }
        }
    }
}
