using MySql.Data.MySqlClient;
using Plantilla_Agenda.Data;
using System.Data;

namespace Plantilla_Agenda.Servicios
{
    public class ConfiguracionDB
    {

        MySqlConnection conex;

        private readonly ContextoDB _contextodb;

        public ConfiguracionDB(ContextoDB contextoDB)
        {
            _contextodb = contextoDB;
        }

        public void conec()
        {
            conex = new MySqlConnection(_contextodb.Conexiondb);
        }
        public bool Conectar()
        {
            conec();
            try
            {
                conex.Open();
                if (conex.State == ConnectionState.Open)
                {
                    // La conexión se ha establecido correctamente
                    return true;
                }
                else
                {
                    // La conexión no se ha abierto correctamente
                    return false;
                }
            }
            catch (Exception exc)
            {

                return false;
            }

        }

        /* private  MySqlConnection ConexionMySlq()
         {
             MySqlConnection Conexion = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConexionMysql"].ConnectionString);

             try
             {
                 Conexion.Open();
             }
             catch
             {
                 return null;
             }

             return Conexion;
         }
         */
        public void Desconectar()
        {
            conex.Close();
        }

        public int RetornarValidacion(string sentencia, List<MySqlParameter> ListaParametros, CommandType TipoComando)
        {
            MySqlCommand comando = new MySqlCommand();
            comando.CommandText = sentencia;
            comando.CommandType = TipoComando;
            comando.Connection = conex;
            foreach (MySqlParameter parametro in ListaParametros)
            {
                comando.Parameters.Add(parametro);
            }
            int count = Convert.ToInt32(comando.ExecuteScalar());

            Desconectar();
            return count;
        }


        public void EjecutarOperacion(string sentencia, List<MySqlParameter> ListaParametros, CommandType TipoComando)
        {
            MySqlCommand comando = new MySqlCommand();
            comando.CommandText = sentencia;
            comando.CommandType = TipoComando;
            comando.Connection = conex;
            foreach (MySqlParameter parametro in ListaParametros)
            {
                comando.Parameters.Add(parametro);
            }
            comando.ExecuteNonQuery();
            Desconectar();
        }

        public DataTable EjecutarConsulta(string sentencia, List<MySqlParameter> ListaParametros, CommandType TipoComando)
        {
            MySqlDataAdapter adaptador = new MySqlDataAdapter();
            adaptador.SelectCommand = new MySqlCommand(sentencia, conex);
            adaptador.SelectCommand.CommandType = TipoComando;

            foreach (MySqlParameter parametro in ListaParametros)
            {
                adaptador.SelectCommand.Parameters.Add(parametro);
            }
            DataSet resultado = new DataSet();
            adaptador.Fill(resultado);
            Desconectar();
            return resultado.Tables[0];
        }

        public DataTable EjecutarConsultaDS(string sentencia, List<MySqlParameter> ListaParametros, CommandType TipoComando)
        {
            MySqlDataAdapter adaptador = new MySqlDataAdapter();
            adaptador.SelectCommand = new MySqlCommand(sentencia, conex);
            adaptador.SelectCommand.CommandType = TipoComando;

            foreach (MySqlParameter parametro in ListaParametros)
            {
                adaptador.SelectCommand.Parameters.Add(parametro);
            }
            DataTable resultado = new();
            adaptador.Fill(resultado);
            adaptador.Dispose();
            Desconectar();
            return resultado;
        }

        public void EjecutarTransaccion(List<string> Sentencia)
        {
            MySqlTransaction transa = conex.BeginTransaction();
            MySqlCommand mySqlCommand;

            for (int i = 0; i < Sentencia.Count; i++)
            {
                if (Sentencia[i].Length > 0)
                {
                    mySqlCommand = new MySqlCommand(Sentencia[i], conex);
                    mySqlCommand.Transaction = transa;
                    mySqlCommand.ExecuteNonQuery();
                }
            }

        }

    }
}
