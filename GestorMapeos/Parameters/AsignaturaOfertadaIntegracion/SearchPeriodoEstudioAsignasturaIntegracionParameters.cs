namespace GestorMapeos.Parameters.AsignaturaOfertadaIntegracion
{
    public class SearchPeriodoEstudioAsignasturaIntegracionParameters : BaseListViewModel
    {
        public enum AsignaturaOfertadaIntegracionOrderColumn
        {
            Id,
            Asignatura,
            Estudio,
            PeriodoMatriculacion,
            IdAsignaturaOfertada
        }
        public int? IdPeriodoEstudioAsignatura { get; set; }
        public int? IdPeriodoMatriculacion { get; set; }
        public int? IdEstudio { get; set; }
        public int? IdAsignatura { get; set; }


        public int? IdAsignaturaOfertada { get; set; }
        public int? IdPeriodoAcademico { get; set; }
        public int? IdPlanEstudio { get; set; }
        public int? FilterIdAsignaturaOfertada { get; set; }
    }
}