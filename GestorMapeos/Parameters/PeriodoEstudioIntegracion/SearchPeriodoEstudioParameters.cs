namespace GestorMapeos.Parameters.PeriodoEstudioIntegracion
{
    public class SearchPeriodoEstudioParameters : BaseListViewModel
    {
        public enum PeriodoEstudioOrderColumn
        {
            IdPeriodoEstudio,
            Estudio,
            PeriodoMatriculacion
        }
        public int? IdPeriodoEstudio { get; set; }
        public int? IdPeriodoMatriculacion { get; set; }
        public int? IdEstudio { get; set; }
    }
}