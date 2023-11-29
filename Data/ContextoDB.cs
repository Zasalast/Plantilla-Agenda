namespace Plantilla_Agenda.Data
{
    public class ContextoDB
    {
        public string Conexiondb { get; set; }

        public ContextoDB(String valor)
        {

            Conexiondb = valor;

        }

    }
}
