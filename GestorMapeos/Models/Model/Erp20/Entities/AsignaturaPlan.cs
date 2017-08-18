using System.Collections.Generic;

namespace GestorMapeos.Models.Model.Erp20.Entities
{
    public class AsignaturaPlan
    {
        // Propiedades Primitivas	
        public	int Id { get; set; }
        public int? UbicacionPeriodoLectivo { get; set; }
        public int PlanId { get; set; }
        public int AsignaturaId { get; set; }
        public int DuracionPeriodoLectivoId { get; set; }
        public int? CursoId { get; set; }
        public virtual Plan Plan { get; set; }
        public virtual Asignatura Asignatura { get; set; }
        public virtual DuracionPeriodoLectivo DuracionPeriodoLectivo { get; set; }
        public virtual Curso Curso { get; set; }
        public virtual ICollection<AsignaturaOfertada> AsignaturasOfertadas { get; set; }
    }
}