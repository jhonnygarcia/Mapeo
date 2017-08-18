using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestorMapeos.Parameters.PlanOfertado
{
    public class SearchPlanOfertadoParameters: BaseListViewModel
    {
        public enum PlanOfertadoOrderColumn
        {
            IdPlanOfertado,
            PlanEstudio,
            PeriodoAcademico
        }
        public int? IdPlanOfertado { get; set; }
        public int? IdPeriodoAcademico { get; set; }
        public int? IdPlanEstudio { get; set; }
    }
}