namespace GestorMapeos.Models.Model.Gestor.Entities
{
    public class EstudioIntegracion
    {
        public int Id { get; set; }
        public int PlantillaEstudioIntegracionId { get; set; }
        public int? EspecializacionId { get; set; }
        
        public virtual  PlantillaEstudioIntegracion PlantillaEstudioIntegracion { get; set; }
        public virtual EstudioUnir EstudioUnir { get; set; }
    }
}