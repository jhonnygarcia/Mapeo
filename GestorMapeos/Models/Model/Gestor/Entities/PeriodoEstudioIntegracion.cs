namespace GestorMapeos.Models.Model.Gestor.Entities
{
    public class PeriodoEstudioIntegracion
    {
        public int Id { get; set; }
        public int PlanOfertadoId { get; set; }
        public int PlantillaEstudioIntegracionId { get; set; }
        public virtual PeriodoEstudioUnir PeriodoEstudioUnir { get; set; }
        public virtual PlantillaEstudioIntegracion PlantillaEstudioIntegracion { get; set; }
    }
}
