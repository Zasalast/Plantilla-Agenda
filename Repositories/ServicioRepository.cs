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
        private readonly IConfiguration _config;

        public ServicioRepository(IConfiguration config)
        {
            _config = config;
        }

        public List<ServicioModel> ObtenerServicios()
        {
            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "SELECT * FROM servicios";
                return connection.Query<ServicioModel>(sql).AsList();
            }
        }

        public ServicioModel ObtenerServicioPorId(int id)
        {
            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "SELECT * FROM servicios WHERE IdServicio = @IdServicio";
                return connection.QueryFirstOrDefault<ServicioModel>(sql, new { IdServicio = id });
            }
        }

        public void CrearServicio(ServicioModel servicio)
        {
            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "INSERT INTO servicios (Nombre, Descripcion, Duracion, Estado) VALUES (@Nombre, @Descripcion, @Duracion, @Estado)";
                connection.Execute(sql, servicio);
            }
        }

        public void ActualizarServicio(ServicioModel servicio)
        {
            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "UPDATE servicios SET Nombre = @Nombre, Descripcion = @Descripcion, Duracion = @Duracion, Estado = @Estado WHERE IdServicio = @IdServicio";
                connection.Execute(sql, servicio);
            }
        }

        public void EliminarServicio(int id)
        {
            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "DELETE FROM servicios WHERE IdServicio = @IdServicio";
                connection.Execute(sql, new { IdServicio = id });
            }
        }
    }
}
