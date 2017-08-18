namespace GestorMapeos.Models.Dto.Erp20
{
    public class AsignaturaOfertadaDto
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
        public virtual PlanOfertadoDto PlanOfertado { get; set; }
        public virtual PeriodoLectivoDto PeriodoLectivo { get; set; }
        public virtual AsignaturaPlanDto AsignaturaPlan { get; set; }
        public virtual TipoAsignaturaDto TipoAsignatura { get; set; }
        public virtual CursoDto Curso { get; set; }

        public string DisplayName => Codigo + " - " + Nombre;
    }
}