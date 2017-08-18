namespace GestorMapeos.Models.Model.Erp20.Entities
{
    public class Asignatura
    {
        // Propiedades Primitivas	
        public	int Id { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public double Creditos { get; set; }
        public int TipoAsignaturaId { get; set; }
        public virtual TipoAsignatura TipoAsignatura { get; set; }
    }
}