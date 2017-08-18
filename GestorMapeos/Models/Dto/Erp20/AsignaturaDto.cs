namespace GestorMapeos.Models.Dto.Erp20
{
    public class AsignaturaDto
    {
        // Propiedades Primitivas	
        public	int Id { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public double Creditos { get; set; }
        public int TipoAsignaturaId { get; set; }
        public virtual TipoAsignaturaDto TipoAsignatura { get; set; }
        public string DisplayName => Codigo + " - " + Nombre;
    }
}