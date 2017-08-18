using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestorMapeos.Parameters.AsignaturaOfertadaIntegracion
{
    public class SearchPeriodoEstudioAsignaturaParameters : BaseListViewModel
    {
        public enum PeriodoEstudioAsignaturaOrderColumn
        {
            IdPeriodoEstudioAsignatura,
            Asignatura,
            Estudio,
            PeriodoMatriculacion,
            TipoAsignatura,
            Creditos,
            PeriodoLectivo,
            Curso,
            Activa
        }
        public int? IdPeriodoEstudioAsignatura { get; set; }
        public int? IdPeriodoMatriculacion { get; set; }
        public int? IdEstudio { get; set; }
        public int? IdAsignatura { get; set; }

    }
}