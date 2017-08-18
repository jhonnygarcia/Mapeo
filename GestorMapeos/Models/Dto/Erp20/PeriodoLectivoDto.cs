using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestorMapeos.Models.Dto.Erp20
{
    public class PeriodoLectivoDto
    {
        // Propiedades Primitivas
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int DuracionPeriodoLectivoId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public virtual DuracionPeriodoLectivoDto DuracionPeriodoLectivo { get; set; }
        public virtual IEnumerable<AsignaturaOfertadaDto> AsignaturasOfertadas { get; set; }

        public string DisplayName => Nombre + " (" + FechaInicio.ToString("dd/MM/yyyy") + " - " +
                                     FechaFin.ToString("dd/MM/yyyy") + ")";
    }
}