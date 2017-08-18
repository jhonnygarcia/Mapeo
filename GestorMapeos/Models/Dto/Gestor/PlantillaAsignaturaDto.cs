using System.Collections.Generic;

namespace GestorMapeos.Models.Dto.Gestor
{
    public class PlantillaAsignaturaDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string NombreAsignatura { get; set; }
        public decimal Creditos { get; set; }
        public int PlantillaEstudioId { get; set; }
        public byte TipoAsignaturaId { get; set; }
        public virtual PlantillaEstudioDto PlantillaEstudio { get; set; }
        public virtual TipoAsignaturaUnirDto TipoAsignatura { get; set; }
        public virtual IEnumerable<AsignaturaUnirDto> Asignaturas { get; set; }
    }
}