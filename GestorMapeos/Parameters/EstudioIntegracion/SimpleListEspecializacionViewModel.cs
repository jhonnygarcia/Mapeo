using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestorMapeos.Parameters.EstudioIntegracion
{
    public class SimpleListEspecializacionViewModel: BaseListViewModel
    {
        public string Descripcion { get; set; }
        public int? FilterTitulo { get; set; }
        public int? FilterPlanEstudio { get; set; }
        public bool RequiredPlan { get; set; }
        public bool RequiredTitulo { get; set; }
    }
}