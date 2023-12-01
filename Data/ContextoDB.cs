namespace Plantilla_Agenda.Data
{
    public class ContextoDB
    {
        public string Conexiondb { get; set; }

        public ContextoDB(string conexiondb)
        {
            Conexiondb = conexiondb;
        }
    }
}
