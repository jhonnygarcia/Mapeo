using System.Collections.Generic;

namespace GestorMapeos.Models.Dto.Gestor
{
    public class AsignaturaUnirDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string TipoAsignatura { get; set; }
        public int Creditos { get; set; }
        public string PeriodoLectivo { get; set; }
        public int Curso { get; set; }
        public string Activo { get; set; }
        public string Borrado { get; set; }
        public int EstudioUnirId { get; set; }
        public virtual EstudioUnirDto EstudioUnir { get; set; }
        public virtual AsignaturaIntegracionDto AsignaturaIntegracion { get; set; }

        public virtual IEnumerable<PlantillaAsignaturaDto> PlantillasAsignaturas { get; set; }
        public virtual IEnumerable<PeriodoEstudioAsignaturaUnirDto> PeriodosEstudiosAsignaturasUnir { get; set; }
    }
}
