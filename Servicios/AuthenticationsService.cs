using MySql.Data.MySqlClient;
using Plantilla_Agenda.Models;
using System.Data;
using System;
using Plantilla_Agenda.Repositories;

namespace Plantilla_Agenda.Servicios
{
    public class AuthenticationsService
    {
        // AuthenticationService.cs
        
            private readonly UsuarioRepository _usuarioRepository;

            public AuthenticationsService(UsuarioRepository usuarioRepository)
            {
                _usuarioRepository = usuarioRepository;
            }

            public bool AuthenticateUser(string nombreUsuario, string clave)
            {
                var user = _usuarioRepository.IniciarSesion(nombreUsuario, clave);
                return user != null;
            }
        public bool AuthenticateUser1(string nombreUsuario, string clave)
        {
            // Aquí, puedes realizar la lógica de autenticación.
            // En este ejemplo, se usa el UsuarioRepository para obtener la información del usuario.

            var user = _usuarioRepository.ObtenerUsuarioPorNombreUsuario(nombreUsuario);

            if (user != null && user.ClaveHash == clave)
            {
                // Autenticación exitosa
                return true;
            }

            // Autenticación fallida
            return false;
        }
    }
}
