
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
        private readonly IConfiguration _config;

        public PersonaRepository(IConfiguration config)
    {
            _config = config;
        }

    public List<PersonaModel> ObtenerPersonas()
    {
        using IDbConnection db = new MySqlConnection(_config.GetConnectionString("DefaultConnection"));
        string query = "SELECT * FROM personas";
        return db.Query<PersonaModel>(query).AsList();
    }

        public PersonaModel ObtenerPersonaPorId(int id)
        {
            using IDbConnection db = new MySqlConnection(_config.GetConnectionString("DefaultConnection"));
            string query = "SELECT * FROM personas WHERE IdPersona = @Id";
            return db.QueryFirstOrDefault<PersonaModel>(query, new { Id = id });
        }
        public string ObtenerNombrePersonaPorId(int id)
        {
            using (IDbConnection db = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                string query = "SELECT PrimerNombre FROM personas WHERE IdPersona = @Id";
                return db.QueryFirstOrDefault<string>(query, new { Id = id });
            }
        }


        public void RegistrarPersonaPorAdmin(PersonaModel persona)
        {
            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
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
