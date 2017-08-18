using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestorMapeos.Parameters.AsignaturaOfertadaIntegracion
{
    public class SearchAsignaturaOfertadaParameters: BaseListViewModel
    {
        public enum AsignaturaOfertadaOrderColumn
        {
            IdAsignaturaOfertada,
            AsignaturaOfertada,
            PeriodoLectivo,
            PlanEstudio,
            PeriodoAcademico,
            TipoAsignatura,
            Creditos,
            DuracionPeriodoLectivo,
            UbicacionPeriodoLectivo,
            Curso
        }

        public int? IdAsignaturaOfertada { get; set; }
        public int? IdPeriodoAcademico { get; set; }
        public int? IdPlanEstudio { get; set; }
        public int? FilterIdAsignaturaOfertada { get; set; }
    }
}