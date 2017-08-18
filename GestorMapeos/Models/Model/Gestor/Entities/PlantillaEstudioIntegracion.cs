using System.Collections.Generic;

namespace GestorMapeos.Models.Model.Gestor.Entities
{
    public class PlantillaEstudioIntegracion
    {
        public PlantillaEstudioIntegracion()
        {
            EstudiosIntegracion = new List<EstudioIntegracion>();
            AsignaturasIntegracion = new List<PlantillaAsignaturaIntegracion>();
            PeriodosEstudioIntegracion = new List<PeriodoEstudioIntegracion>();
         
        }

        public int Id { get; set; }
        public int PlantillaEstudioId { get; set; }
        public virtual PlantillaEstudio PlantillaEstudio { get; set; }

        public virtual ICollection<EstudioIntegracion>  EstudiosIntegracion { get; set; }
        public virtual ICollection<PlantillaAsignaturaIntegracion> AsignaturasIntegracion { get; set; }
        public virtual ICollection<PeriodoEstudioIntegracion> PeriodosEstudioIntegracion { get; set; }
    }
}