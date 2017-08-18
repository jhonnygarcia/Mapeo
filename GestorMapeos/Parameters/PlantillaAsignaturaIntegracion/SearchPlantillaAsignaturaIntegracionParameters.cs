namespace GestorMapeos.Parameters.PlantillaAsignaturaIntegracion
{
    public class SearchPlantillaAsignaturaIntegracionParameters : BaseListViewModel
    {
        public enum AsignaturaPlanIntegracionOrderColumn
        {
            IdPlantilla,
            PlantillaAsignatura,
            PlantillaEstudio,
            Tipo,
            Creditos,
            IdAsignaturaPlan
        }
        public int? IdPlantillaEstudio { get; set; }
        public int? IdPlantillaAsignatura { get; set; }
        public int? IdPlanEstudio { get; set; }
        public int? IdAsignaturaPlan { get; set; }

        public int? FilterIdPlantillaEstudio { get; set; }
        public int? FilterIdPlantillaAsignatura { get; set; }
        public int? FilterIdPlanEstudio { get; set; }
        public int? FilterIdAsignaturaPlan { get; set; }
    }
}