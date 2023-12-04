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
        private readonly string _connectionString;

        public UsuarioRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Usuario ObtenerUsuarioPorNombreUsuario(string nombreUsuario)
        {
           

            return null;
        }
        public Usuario IniciarSesion(string usuario, string password)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<Usuario>(
                  "IniciarSesion",
                  new { InNombreUsuario = usuario, InClave = password },
                  commandType: CommandType.StoredProcedure);
            }
        }
        public void RegistrarUsuario(Usuario usuario)
        {
            using (var connection = new MySqlConnection(_connectionString))
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
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Execute("EliminarUsuario", new
                {
                    p_IdUsuario = idUsuario
                }, commandType: CommandType.StoredProcedure);
            }
        }

        public void ActualizarUsuario(Usuario usuario)
        {
            using (var connection = new MySqlConnection(_connectionString))
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