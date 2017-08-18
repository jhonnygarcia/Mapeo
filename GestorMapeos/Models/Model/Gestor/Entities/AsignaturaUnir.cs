using System.Collections.Generic;

namespace GestorMapeos.Models.Model.Gestor.Entities
{
    public class AsignaturaUnir
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
        public virtual EstudioUnir EstudioUnir { get; set; }
        public virtual AsignaturaIntegracion AsignaturaEstudioIntegracion { get; set; }

        public virtual ICollection<PlantillaAsignatura> PlantillasAsignaturas { get; set; }
        public virtual ICollection<PeriodoEstudioAsignaturaUnir> PeriodosEstudiosAsignaturasUnir { get; set; }
    }
}
