using System;
using System.Collections.Generic;

namespace GestorMapeos.Models.Dto.Gestor
{
    public class PeriodoMatriculacionUnirDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string AnyoAcademico { get; set; }
        public DateTime? FechaInicio { get; set; } 
        public DateTime? FechaFin { get; set; }
        public int? Nro { get; set; }
        public string Borrado { get; set; }
        public virtual PeriodoMatriculacionIntegracionDto PeriodoMatriculacionIntegracion { get; set; }
        public virtual IEnumerable<PeriodoEstudioUnirDto> PeriodosEstudioUnir { get; set; }
    }
}