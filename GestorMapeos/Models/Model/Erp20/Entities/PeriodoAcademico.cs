using System;
using System.Collections.Generic;

namespace GestorMapeos.Models.Model.Erp20.Entities
{
    public class PeriodoAcademico
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int Nro { get; set; }
        public int AnyoAcademicoId { get; set; }
        public virtual AnyoAcademico AnyoAcademico { get; set; }
        public virtual ICollection<PlanOfertado> PlanesOfertados { get; set; }
    }
}