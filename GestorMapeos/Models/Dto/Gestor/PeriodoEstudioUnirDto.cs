using System;
using System.Collections.Generic;

namespace GestorMapeos.Models.Dto.Gestor
{
    public class PeriodoEstudioUnirDto
    {
        public int Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int PeriodoMatriculacionId { get; set; }
        public int EstudioId { get; set; }
        public string Borrado { get; set; }
        public virtual PeriodoMatriculacionUnirDto PeriodoMatriculacionUnir { get; set; }
        public virtual EstudioUnirDto EstudioUnir { get; set; }
        public virtual PeriodoEstudioIntegracionDto PeriodoEstudioIntegracion { get; set; }
        public virtual IEnumerable<PeriodoEstudioAsignaturaUnirDto> PeriodosEstudiosAsignaturasUnir { get; set; }
    }
}
