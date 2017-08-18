using GestorMapeos.Models.Dto.Erp20;

namespace GestorMapeos.Models.Dto.Gestor
{
    public class PeriodoEstudioIntegracionDto
    {
        public int Id { get; set; }
        public int PlanOfertadoId { get; set; }
        public int PlantillaEstudioIntegracionId { get; set; }
        public virtual PeriodoEstudioUnirDto PeriodoEstudioUnir { get; set; }
        public virtual PlantillaEstudioIntegracionDto PlantillaEstudioIntegracion { get; set; }
        public PlanOfertadoDto PlanOfertado { get; set; }
    }
}
