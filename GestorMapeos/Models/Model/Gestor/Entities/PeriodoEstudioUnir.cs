using System;
using System.Collections.Generic;

namespace GestorMapeos.Models.Model.Gestor.Entities
{
    public class PeriodoEstudioUnir
    {
        public int Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int PeriodoMatriculacionId { get; set; }
        public int EstudioId { get; set; }
        public string Borrado { get; set; }
        public virtual PeriodoMatriculacionUnir PeriodoMatriculacionUnir { get; set; }
        public virtual EstudioUnir EstudioUnir { get; set; }
        public virtual PeriodoEstudioIntegracion PeriodoEstudioIntegracion { get; set; }
        public virtual ICollection<PeriodoEstudioAsignaturaUnir> PeriodosEstudiosAsignaturasUnir { get; set; }
    }
}
