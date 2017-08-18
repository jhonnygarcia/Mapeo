using System;
using System.Collections.Generic;

namespace GestorMapeos.Models.Model.Erp20.Entities
{
    public class AnyoAcademico
    {
        public int Id { get; set; }
        public int AnyoInicio { get; set; }
        public int AnyoFin { get; set; }
        public virtual ICollection<PeriodoAcademico> PeriodosAcademicos { get; set; }

    }
}