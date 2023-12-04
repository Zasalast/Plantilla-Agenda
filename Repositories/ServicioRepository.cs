// ServicioRepository.cs
using MySql.Data.MySqlClient;
using Plantilla_Agenda.Models;
using System;
using System.Collections.Generic;
using Dapper;

namespace Plantilla_Agenda.Repositories
{
    public class ServicioRepository
    {
        private readonly string _connectionString;

        public ServicioRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Servicio> ObtenerServicios()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM servicios";
                return connection.Query<Servicio>(sql).AsList();
            }
        }

        public Servicio ObtenerServicioPorId(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM servicios WHERE IdServicio = @IdServicio";
                return connection.QueryFirstOrDefault<Servicio>(sql, new { IdServicio = id });
            }
        }

        public void CrearServicio(Servicio servicio)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                var sql = "INSERT INTO servicios (Nombre, Descripcion, Duracion, Estado) VALUES (@Nombre, @Descripcion, @Duracion, @Estado)";
                connection.Execute(sql, servicio);
            }
        }

        public void ActualizarServicio(Servicio servicio)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                var sql = "UPDATE servicios SET Nombre = @Nombre, Descripcion = @Descripcion, Duracion = @Duracion, Estado = @Estado WHERE IdServicio = @IdServicio";
                connection.Execute(sql, servicio);
            }
        }

        public void EliminarServicio(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                var sql = "DELETE FROM servicios WHERE IdServicio = @IdServicio";
                connection.Execute(sql, new { IdServicio = id });
            }
        }
    }
}
