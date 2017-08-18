using System.Collections.Generic;

namespace GestorMapeos.Models.Dto.Gestor
{
    public class PlantillaEstudioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int AnyoPlan { get; set; }
        public byte TipoEstudioId { get; set; }
        public byte? RamaId { get; set; }
        public virtual TipoEstudioUnirDto TipoEstudio { get; set; }
        public virtual RamaUnirDto Rama { get; set; }
        public virtual IEnumerable<PlantillaAsignaturaDto> PlantillasAsignatura { get; set; }
        public virtual IEnumerable<EstudioUnirDto> EstudiosUnir { get; set; }

        public string DisplayName => Nombre + " - " + AnyoPlan;
    }
}