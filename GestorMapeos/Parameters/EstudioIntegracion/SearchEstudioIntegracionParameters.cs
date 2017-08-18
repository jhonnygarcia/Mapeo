using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestorMapeos.Parameters.EstudioIntegracion
{
    public class SearchEstudioIntegracionParameters: BaseListViewModel
    {
        public enum EstudioIntegracionOrderColumn
        {
            IdEstudio,
            Estudio,
            IdPlan
        }

        public int? IdEstudio { get; set; }
        public int? FilterEstudio { get; set; } 
        public int? IdPlanEstudio { get; set; }
        public int? FilterPlanEstudio { get; set; }
        public int? IdTitulo { get; set; }
        public int? IdEspecializacion { get; set; }

    }
}