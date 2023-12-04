
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Plantilla_Agenda.Models;
using System.Collections.Generic;
using System.Data;
namespace Plantilla_Agenda.Repositories
{
    public class PersonaRepository
    { //utilizará Dapper y ADO.NET puro:
        private readonly string _connectionString;

    public PersonaRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Conexiondb");
    }

    public List<Persona> ObtenerPersonas()
    {
        using IDbConnection db = new MySqlConnection(_connectionString);
        string query = "SELECT * FROM personas";
        return db.Query<Persona>(query).AsList();
    }

    public Persona ObtenerPersonaPorId(int id)
    {
        using IDbConnection db = new MySqlConnection(_connectionString);
        string query = "SELECT * FROM personas WHERE IdPersona = @Id";
        return db.QueryFirstOrDefault<Persona>(query, new { Id = id });
    }

        public void RegistrarPersonaPorAdmin(Persona persona)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Execute("RegistrarPersonaPorAdmin",
                  new
                  {
                      p_PrimerNombre = persona.PrimerNombre,
                      p_SegundoNombre = persona.SegundoNombre,
                      // ... resto de campos  
                  }, commandType: CommandType.StoredProcedure);
            }

        }
    }
}
