namespace GestorMapeos.Models.Model.Erp20.Entities
{
    public class AsignaturaOfertada
    {
        // Propiedades Primitivas	
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public int UbicacionPeriodoLectivo { get; set; }
        public int PlanOfertadoId { get; set; }
        public int PeriodoLectivoId { get; set; }
        public int AsignaturaPlanId { get; set; }
        public int TipoAsignaturaId { get; set; }
        public int? CursoId { get; set; }
        public virtual PlanOfertado PlanOfertado { get; set; }
        public virtual PeriodoLectivo PeriodoLectivo { get; set; }
        public virtual AsignaturaPlan AsignaturaPlan { get; set; }
        public virtual TipoAsignatura TipoAsignatura { get; set; }
        public virtual Curso Curso { get; set; }
    }
}