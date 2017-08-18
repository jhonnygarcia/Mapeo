namespace GestorMapeos.Parameters.AsignaturaIntegracion
{
    public class SearchAsignaturaIntegracionParameters : BaseListViewModel
    {
        public enum AsignaturaEstudioIntegracionOrderColumn
        {
            IdAsignatura,
            Asignatura,
            Estudio,
            Tipo,
            Creditos,
            IdAsignaturaPlan
        }
        public int? IdEstudio { get; set; }
        public int? IdAsignatura { get; set; }
        public int? IdPlanEstudio { get; set; }
        public int? IdAsignaturaPlan { get; set; }
    }
}