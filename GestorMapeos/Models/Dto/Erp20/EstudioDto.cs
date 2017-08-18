namespace GestorMapeos.Models.Dto.Erp20
{
    public class EstudioDto
    {
        // Propiedades Primitivas	
        public	int Id { get; set; }
        public string Nombre { get; set; }
        public string CodigoRuct { get; set; }
        public int TipoEstudioId { get; set; }
        public int RamaConocimientoId { get; set; }
        public virtual TipoEstudioDto TipoEstudio { get; set; }
        public virtual RamaConocimientoDto RamaConocimiento { get; set; }
        public string DisplayName => CodigoRuct + " - " + Nombre;
    }
} 