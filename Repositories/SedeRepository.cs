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

        private readonly IConfiguration _config;
       

        public SedeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<SedeModel> ObtenerSede()
        {
            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                
                var sql = "SELECT * FROM sedes";
                return connection.Query<SedeModel>(sql).AsList();
               
                  
            } 
                    
                   
           
        }

        public SedeModel ObtenerSedePorId(int id)
        {
            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "SELECT * FROM sedes WHERE IdSede = @IdSede";
                return connection.QueryFirstOrDefault<SedeModel>(sql, new { IdServicio = id });
            }
        }

        public void CrearSede(SedeModel sede)
        {
            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "INSERT INTO sedes (Nombre, Descripcion, Duracion, Estado) VALUES (@Nombre, @Descripcion, @Duracion, @Estado)";
                connection.Execute(sql, sede);
            }
        }

        public void ActualizarSede(SedeModel sede)
        {
            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "UPDATE sedes SET Nombre = @Nombre, Descripcion = @Descripcion, Duracion = @Duracion, Estado = @Estado WHERE IdSede = @IdSede";
                connection.Execute(sql, sede);
            }
        }

        public void EliminarSede(int id)
        {
            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "DELETE FROM sedes WHERE IdSede = @IdSede";
                connection.Execute(sql, new { IdSede = id });
            }
        }
    }
}

