namespace GestorMapeos.Models.Model.Erp20.Entities
{
    public class Estudio
    {
        // Propiedades Primitivas	
        public	int Id { get; set; }
        public string Nombre { get; set; }
        public string CodigoRuct { get; set; }
        public int TipoEstudioId { get; set; }
        public int RamaConocimientoId { get; set; }
        public virtual TipoEstudio TipoEstudio { get; set; }
        public virtual RamaConocimiento RamaConocimiento { get; set; }
    }
} 