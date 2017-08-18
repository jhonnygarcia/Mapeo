namespace GestorMapeos.Parameters.PeriodoMatriculacionIntegracion
{
    public class SearchPeriodoMatriculacionIntegracionParameters : BaseListViewModel
    {
        public enum PeriodoMatriculacionIntegracionOrderColumn
        {
            IdPeriodoMatriculacion,
            PeriodoMatriculacion,
            FechaInicio,
            FechaFin
        }
        public int? FilterIdPeriodoMatriculacion { get; set; }
        public int? FilterIdPeriodoAcademico { get; set; }
        public int? IdPeriodoMatriculacion { get; set; }
        public int? IdPeriodoAcademico { get; set; }
    }
}