using MySql.Data.MySqlClient;
using Plantilla_Agenda.Data;
using System;
using System.Collections.Generic;
using System.Data;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Plantilla_Agenda.Servicios
{
    public class ConfiguracionDB
    {
        private MySqlConnection _conexion;
        private readonly ContextoDB _contextoDB;
        private readonly IConfiguration _config;
 
        public ConfiguracionDB(ContextoDB contextoDB, IConfiguration config)
        {
             
            _contextoDB = contextoDB;
            _conexion = new MySqlConnection(config.GetConnectionString("DefaultConnection"));
        }

        public bool Conectar()
        {
            try
            {
                _conexion.Open();
                return _conexion.State == ConnectionState.Open;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Desconectar()
        {
            _conexion.Close();
        }

        public int RetornarValidacion(string sentencia, List<MySqlParameter> listaParametros, CommandType tipoComando)
        {
            using (MySqlCommand comando = new MySqlCommand(sentencia, _conexion))
            {
                comando.CommandType = tipoComando;
                comando.Parameters.AddRange(listaParametros.ToArray());
                Conectar();
                return Convert.ToInt32(comando.ExecuteScalar());
            }
        }

        public void EjecutarOperacion(string sentencia, List<MySqlParameter> listaParametros, CommandType tipoComando)
        {
            using (MySqlCommand comando = new MySqlCommand(sentencia, _conexion))
            {
                comando.CommandType = tipoComando;
                comando.Parameters.AddRange(listaParametros.ToArray());
                Conectar();
                comando.ExecuteNonQuery();
            }
        }

        public DataTable EjecutarConsulta(string sentencia, List<MySqlParameter> listaParametros, CommandType tipoComando)
        {
            using (MySqlDataAdapter adaptador = new MySqlDataAdapter())
            {
                adaptador.SelectCommand = new MySqlCommand(sentencia, _conexion);
                adaptador.SelectCommand.CommandType = tipoComando;
                adaptador.SelectCommand.Parameters.AddRange(listaParametros.ToArray());

                DataSet resultado = new DataSet();
                Conectar();
                adaptador.Fill(resultado);
                return resultado.Tables[0];
            }
        }

        public DataTable EjecutarConsultaDS(string sentencia, List<MySqlParameter> listaParametros, CommandType tipoComando)
        {
            using (MySqlDataAdapter adaptador = new MySqlDataAdapter())
            {
                adaptador.SelectCommand = new MySqlCommand(sentencia, _conexion);
                adaptador.SelectCommand.CommandType = tipoComando;
                adaptador.SelectCommand.Parameters.AddRange(listaParametros.ToArray());

                DataTable resultado = new DataTable();
                Conectar();
                adaptador.Fill(resultado);
                return resultado;
            }
        }

        public void EjecutarTransaccion(List<string> sentencias)
        {
            using (MySqlTransaction transaccion = _conexion.BeginTransaction())
            {
                try
                {
                    foreach (string sentencia in sentencias)
                    {
                        if (!string.IsNullOrEmpty(sentencia))
                        {
                            using (MySqlCommand comando = new MySqlCommand(sentencia, _conexion, transaccion))
                            {
                                comando.ExecuteNonQuery();
                            }
                        }
                    }

                    transaccion.Commit();
                }
                catch (Exception)
                {
                    transaccion.Rollback();
                    throw; // Puedes manejar la excepción de acuerdo a tus necesidades
                }
            }
        }
    }
}
