using MySql.Data.MySqlClient;
using Plantilla_Agenda.Models;
using System.Data;
using System;
using System.Collections.Generic;
using Dapper;
 

namespace Plantilla_Agenda.Repositories
{
    public class UsuarioRepository
    { //utilizará Dapper y ADO.NET puro:
        private readonly IConfiguration _config;

        public UsuarioRepository(IConfiguration config)
        {
            _config = config;
        }

        public Usuario ObtenerUsuarioPorNombreUsuario(string nombreUsuario)
        {
            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                // Execute the query to obtain the user by username
                return connection.QueryFirstOrDefault<Usuario>(
                    "SELECT * FROM usuario WHERE NombreUsuario = @NombreUsuario",
                    new { NombreUsuario = nombreUsuario });
            }

            return null;
        }
        public Usuario IniciarSesion(string nombreUsuario, string clave)
        {
            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                return connection.QueryFirstOrDefault<Usuario>(
                    "IniciarSesion",
                    new { InNombreUsuario = nombreUsuario, InClave = clave },
                    commandType: CommandType.StoredProcedure);
            }
        }
        public void RegistrarUsuario(Usuario usuario)
        {
            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Execute("RegistrarUsuario", new
                {
                    p_NombreUsuario = usuario.NombreUsuario,
                    p_Clave = usuario.ClaveHash,
                    p_IdPersona = usuario.IdPersona,
                    p_IdRol = usuario.IdRol,
                    p_Activo = usuario.Activo
                }, commandType: CommandType.StoredProcedure);
            }
        }

        public void EliminarUsuario(int idUsuario)
        {
            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Execute("EliminarUsuario", new
                {
                    p_IdUsuario = idUsuario
                }, commandType: CommandType.StoredProcedure);
            }
        }

        public void ActualizarUsuario(Usuario usuario)
        {
            using (var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Execute("ActualizarUsuario", new
                {
                    p_IdUsuario = usuario.IdUsuario,
                    p_NuevoNombreUsuario = usuario.NombreUsuario,
                    p_NuevaClave = usuario.ClaveHash,
                    p_NuevoIdPersona = usuario.IdPersona,
                    p_NuevoIdRol = usuario.IdRol,
                    p_NuevoActivo = usuario.Activo
                }, commandType: CommandType.StoredProcedure);
            }
        }


    }
}