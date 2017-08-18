using System.Collections.Generic;

namespace GestorMapeos.Models.Model.Gestor.Entities
{
    public class PlantillaAsignatura
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string NombreAsignatura { get; set; }
        public decimal Creditos { get; set; }
        public int PlantillaEstudioId { get; set; }
        public byte TipoAsignaturaId { get; set; }
        public virtual PlantillaEstudio PlantillaEstudio { get; set; }
        public virtual TipoAsignaturaUnir TipoAsignatura { get; set; }
        public virtual ICollection<AsignaturaUnir> Asignaturas { get; set; }
    }
}