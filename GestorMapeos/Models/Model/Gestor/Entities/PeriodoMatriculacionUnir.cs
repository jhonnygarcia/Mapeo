using System;
using System.Collections.Generic;

namespace GestorMapeos.Models.Model.Gestor.Entities
{
    public class PeriodoMatriculacionUnir
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string AnyoAcademico { get; set; }
        public DateTime? FechaInicio { get; set; } 
        public DateTime? FechaFin { get; set; }
        public int? Nro { get; set; }
        public string Borrado { get; set; }
        public virtual PeriodoMatriculacionIntegracion PeriodoMatriculacionIntegracion { get; set; }
        public virtual ICollection<PeriodoEstudioUnir> PeriodosEstudioUnir { get; set; }
    }
}