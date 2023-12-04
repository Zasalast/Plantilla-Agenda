using MySql.Data.MySqlClient;
using Plantilla_Agenda.Models;
using System.Data;
using System;
using System.Collections.Generic;
using Dapper;
 
namespace Plantilla_Agenda.Repositories
{
    public class HorarioRepository
    { //utilizará Dapper y ADO.NET puro:
        private readonly string _connectionString;

        public HorarioRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<HorarioModel> MostrarDisponibilidadHorariosServicio(int idServicio)
        {
            var horarios = new List<HorarioModel>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new MySqlCommand("MostrarDisponibilidadHorariosServicio", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@p_IdServicio", idServicio);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            horarios.Add(MapHorarioFromReader(reader));
                        }
                    }
                }
            }

            return horarios;
        }

        // Otros métodos según tus necesidades...

        private HorarioModel MapHorarioFromReader(IDataReader reader)
        {
            return new HorarioModel
            {
                IdHorario = Convert.ToInt32(reader["IdHorario"]),
                HoraInicio = Convert.IsDBNull(reader["HoraInicio"]) ? TimeSpan.Zero : TimeSpan.Parse(Convert.ToString(reader["HoraInicio"])),
                HoraFin = Convert.IsDBNull(reader["HoraFin"]) ? TimeSpan.Zero : TimeSpan.Parse(Convert.ToString(reader["HoraFin"])),
                FechaInicio = Convert.IsDBNull(reader["FechaInicio"]) ? DateTime.MinValue : Convert.ToDateTime(reader["FechaInicio"]),
                FechaFin = Convert.IsDBNull(reader["FechaFin"]) ? DateTime.MinValue : Convert.ToDateTime(reader["FechaFin"])
            };
        }
    }
}
