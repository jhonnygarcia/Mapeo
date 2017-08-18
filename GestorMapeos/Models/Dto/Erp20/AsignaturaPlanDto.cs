using System.Collections.Generic;

namespace GestorMapeos.Models.Dto.Erp20
{
    public class AsignaturaPlanDto
    {
        // Propiedades Primitivas	
        public	int Id { get; set; }
        public int? UbicacionPeriodoLectivo { get; set; }
        public int PlanId { get; set; }
        public int AsignaturaId { get; set; }
        public int DuracionPeriodoLectivoId { get; set; }
        public int? CursoId { get; set; }
        public virtual PlanDto Plan { get; set; }
        public virtual AsignaturaDto Asignatura { get; set; }
        public virtual DuracionPeriodoLectivoDto DuracionPeriodoLectivo { get; set; }
        public virtual CursoDto Curso { get; set; }
        public virtual IEnumerable<AsignaturaOfertadaDto> AsignaturasOfertadas { get; set; }
        public string DisplayName => (Asignatura != null ? Asignatura.Codigo + " - " + Asignatura.Nombre : string.Empty);
    }
}