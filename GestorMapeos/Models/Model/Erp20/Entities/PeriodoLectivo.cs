using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestorMapeos.Models.Model.Erp20.Entities
{
    public class PeriodoLectivo
    {
        // Propiedades Primitivas
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int DuracionPeriodoLectivoId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public virtual DuracionPeriodoLectivo DuracionPeriodoLectivo { get; set; }
        public virtual ICollection<AsignaturaOfertada> AsignaturasOfertadas { get; set; }
    }
}