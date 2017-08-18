using System.Collections.Generic;

namespace GestorMapeos.Models.Model.Gestor.Entities
{
    public class PlantillaAsignaturaIntegracion
    {
        public int Id { get; set; }
        public int PlantillaAsignaturaId { get; set; }
        public int PlanIntegracionId { get; set; }
        public virtual PlantillaAsignatura PlantillaAsignatura { get; set; }
        public virtual PlantillaEstudioIntegracion PlanIntegracion { get; set; }
        public virtual ICollection<AsignaturaIntegracion> AsignaturasEstudioIntegracion { get; set; }
    }
}
