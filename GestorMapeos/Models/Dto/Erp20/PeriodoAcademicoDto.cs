using System;
using System.Collections.Generic;

namespace GestorMapeos.Models.Dto.Erp20
{
    public class PeriodoAcademicoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int Nro { get; set; }
        public int AnyoAcademicoId { get; set; }
        public virtual AnyoAcademicoDto AnyoAcademico { get; set; }
        public virtual IEnumerable<PlanOfertadoDto> PlanesOfertados { get; set; }

        public string DisplayName => Nombre + " (" + FechaInicio.ToString("dd/MM/yyyy") + " - " +
                                     FechaFin.ToString("dd/MM/yyyy") + ")";
    }
}