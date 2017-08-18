namespace GestorMapeos.Controllers.Parameters.AsignaturaOfertada
{
    public class SearchAsignaturaOfertadaParameters : BaseListViewModel
    {
        public enum AsignaturaOfertadaOrderColumn
        {
            IdAsignaturaOfertada,
            AsignaturaOfertada,
            PeriodoLectivo,
            PlanEstudio,
            PeriodoAcademico,
            TipoAsignatura,
            Creditos,
            DuracionPeriodoLectivo,
            UbicacionPeriodoLectivo,
            Curso
        }
        public int? IdAsignaturaOfertada { get; set; }
        public int? IdPeriodoAcademico { get; set; }
        public int? IdPlanEstudio { get; set; }
    }
}