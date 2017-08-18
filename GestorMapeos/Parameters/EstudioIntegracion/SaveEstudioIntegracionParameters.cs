using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestorMapeos.Parameters.EstudioIntegracion
{
    public class SaveEstudioIntegracionParameters
    {
        public int IdEstudioGestor { get; set; }
        public int IdRefPlanErp { get; set; }
        public int? IdRefEspecializacion { get; set; }
    }
}