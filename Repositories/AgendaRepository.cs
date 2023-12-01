using MySql.Data.MySqlClient;
using Plantilla_Agenda.Models;
using System.Collections.Generic;

namespace Plantilla_Agenda.Repositories
{
    public class AgendaRepository
    {
        private  string _connectionString;

        public AgendaRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public int Create(Agenda agenda)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var command = new MySqlCommand(
                  "INSERT INTO agendamientos (IdProfesional, IdHorario, IdSede, IdServicio) VALUES (@IdProfesional, @IdHorario, @IdSede, @IdServicio)",
                  connection);

                command.Parameters.AddWithValue("@IdProfesional", agenda.IdProfesional);
                command.Parameters.AddWithValue("@IdHorario", agenda.IdHorario);
                command.Parameters.AddWithValue("@IdSede", agenda.IdSede);
                command.Parameters.AddWithValue("@IdServicio", agenda.IdServicio);

                object result = command.ExecuteScalar();

                if (result != null)
                {
                    return Convert.ToInt32(result);
                }

                return -1; ;
            }
        }

        // AgendaRepository

        public List<Persona> GetProfesionales()
        {
            List<Persona> profesionales = new List<Persona>();
            const string sql = "SELECT IdPersona, PrimerNombre, PrimerApellido FROM personas";

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {

                    using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var persona = new Persona
                    {
                        IdPersona = reader.GetInt32(0),
                        PrimerNombre = reader.GetString(1),
                        PrimerApellido = reader.GetString(2)
                    };
                    profesionales.Add(persona);
                    }
                }
            }
            return profesionales;
        }

        public List<Horario> GetHorarios()
        {
            List<Horario> horarios = new List<Horario>();
            const string sql = "SELECT IdHorario, HoraInicio, HoraFin FROM horarios";
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader()) 
            while (reader.Read())
            {
                var horario = new Horario
                {
                    IdHorario = reader.GetInt32(0),
                    HoraInicio = reader.GetTimeSpan(1),
                    HoraFin = TimeSpan.Parse(reader.GetString(2))
                };
                horarios.Add(horario);
            }}}
            return horarios;
        }

        public List<Sede> GetSedes()
        {
            const string sql = "SELECT IdSede, Direccion  FROM sedes";

            List<Sede> sedes = new List<Sede>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {

                    using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var sede = new Sede
                    {
                        IdSede = reader.GetInt32(0),
                        Direccion = reader.GetString(1)
                    };
                    sedes.Add(sede);
                    }
                }
            }
            return sedes;
            // Similar para sedes
        }

        public List<Servicio> GetServicios()
        {
            const string sql = "SELECT IdServicio, Nombre  FROM servicios";

            List<Servicio> servicios = new List<Servicio>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var servicio = new Servicio
                    {
                        IdServicio = reader.GetInt32(0),
                        Nombre = reader.GetString(1)
                    };
                    servicios.Add(servicio);
                    }
                }
            }
            return servicios;

            // Similar para servicios
        }

        // Luego el insert
        public int CrearAgenda(Agenda agenda)
        {
          
            string sql  = "INSERT INTO agendamientos (IdProfesional, IdHorario, IdSede, IdServicio)    VALUES(@IdProfesional, @IdHorario, @IdSede, @IdServicio)";

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@IdProfesional", agenda.IdProfesional);
                command.Parameters.AddWithValue("@IdHorario", agenda.IdHorario);
                command.Parameters.AddWithValue("@IdSede", agenda.IdSede);
                command.Parameters.AddWithValue("@IdServicio", agenda.IdServicio);
                    // Código para mapear resultado a lista de Persona
                    return command.ExecuteNonQuery();
                }
               
            }

            // Mapear parámetros y ejecutar
            
        }
    }
}
