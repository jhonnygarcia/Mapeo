using System;
using System.Collections.Generic;

namespace GestorMapeos.Models.Dto.Erp20
{
    public class AnyoAcademicoDto
    {
        public int Id { get; set; }
        public int AnyoInicio { get; set; }
        public int AnyoFin { get; set; }
        public virtual IEnumerable<PeriodoAcademicoDto> PeriodosAcademicos { get; set; }
        public string DisplayName => AnyoInicio + "/" + AnyoFin;
    }
}