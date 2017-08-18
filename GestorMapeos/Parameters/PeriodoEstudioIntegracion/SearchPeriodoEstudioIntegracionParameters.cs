namespace GestorMapeos.Parameters.PeriodoEstudioIntegracion
{
    public class SearchPeriodoEstudioIntegracionParameters : BaseListViewModel
    {
        public enum PeriodoEstudioIntegracionOrderColumn
        {
            IdPeriodoEstudio,
            Estudio,
            PeriodoMatriculacion,
            IdPlanOfertado
        }
        public int? IdPeriodoEstudio { get; set; }
        public int? IdPeriodoMatriculacion { get; set; }
        public int? IdEstudio { get; set; }
        public int? IdPlanOfertado { get; set; }
        public int? IdPeriodoAcademico { get; set; }
        public int? IdPlanEstudio { get; set; }
    }
}