using System.Collections.Generic;
using GestorMapeos.Models.Dto.Erp20;

namespace GestorMapeos.Models.Dto.Gestor
{
    public class PlantillaAsignaturaIntegracionDto
    {
        public int Id { get; set; }
        public int PlantillaAsignaturaId { get; set; }
        public int PlanIntegracionId { get; set; }
        public virtual PlantillaAsignaturaDto PlantillaAsignatura { get; set; }
        public virtual PlantillaEstudioIntegracionDto PlanIntegracion { get; set; }
        public virtual IEnumerable<AsignaturaIntegracionDto> AsignaturasEstudioIntegracion { get; set; }

        public AsignaturaPlanDto AsignaturaPlan { get; set; }
    }
}
