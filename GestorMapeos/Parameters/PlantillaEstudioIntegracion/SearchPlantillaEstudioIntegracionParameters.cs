using System;

namespace GestorMapeos.Parameters.PlantillaEstudioIntegracion
{
    public class SearchPlantillaEstudioIntegracionParameters : BaseListViewModel
    {
        public enum PlanIntegracionOrderColumn
        {
            IdPlantilla,
            PlantillaEstudio,
            IdPlan
        }
        public int? IdPlantillaEstudio { get; set; }
        public int? FilterIdPlantillaEstudio { get; set; }
        public int? FilterIdPlan { get; set; }
        public int? IdPlan { get; set; }
    }
}