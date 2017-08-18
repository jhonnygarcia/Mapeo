using System.Collections.Generic;

namespace GestorMapeos.Models.Model.Gestor.Entities
{
    public class PlantillaEstudio
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int AnyoPlan { get; set; }
        public byte TipoEstudioId { get; set; }
        public byte? RamaId { get; set; }
        public virtual TipoEstudioUnir TipoEstudio { get; set; }
        public virtual RamaUnir Rama { get; set; }
        public virtual ICollection<PlantillaAsignatura> PlantillasAsignatura { get; set; }
        public virtual ICollection<EstudioUnir> EstudiosUnir { get; set; }
    }
}