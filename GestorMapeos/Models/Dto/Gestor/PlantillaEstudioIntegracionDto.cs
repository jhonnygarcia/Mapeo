using System.Collections.Generic;
using GestorMapeos.Models.Dto.Erp20;

namespace GestorMapeos.Models.Dto.Gestor
{
    public class PlantillaEstudioIntegracionDto
    {
        public int Id { get; set; }
        public int PlantillaEstudioId { get; set; }
        public virtual PlantillaEstudioDto PlantillaEstudio { get; set; }
        public virtual IEnumerable<EstudioIntegracionDto>  EstudiosIntegracion { get; set; }
        public virtual IEnumerable<PlantillaAsignaturaIntegracionDto> AsignaturasIntegracion { get; set; }
        public virtual IEnumerable<PeriodoEstudioIntegracionDto> PeriodosEstudioIntegracion { get; set; }

        //Propiedades de navegacion a ERP-V20
        public PlanDto Plan { get; set; }
    }
}