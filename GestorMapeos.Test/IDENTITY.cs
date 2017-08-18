using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorMapeos.Test
{
    public class IDENTITY
    {
        #region PropiedadesPrivadas

        private static int _ID_PLANTILLA_ESTUDIO = 0;
        private static int _ID_ESTUDIO = 0;
        private static int _ID_TIPO_ESTUDIO = 0;
        private static byte _ID_TIPO_ESTUDIO_UNIR = 0;
        private static byte _ID_TIPO_ASIGNATURA_UNIR = 0;
        private static int _ID_TIPO_ASIGNATURA = 0;
        private static byte _ID_TIPO_ESTUDIO_SEGUN_UNIR = 0;
        private static int _ID_RAMA_CONOCIMIENTO = 0;
        private static int _ID_DURACION_PERIODO_LECTIVO = 0;
        #endregion



        #region Propiedades Publicas

        public static int ID_PLANTILLA_ESTUDIO => ++_ID_PLANTILLA_ESTUDIO;
        public static int ID_ESTUDIO => ++_ID_ESTUDIO;
        public static int ID_TIPO_ESTUDIO => ++_ID_TIPO_ESTUDIO;
        public static byte ID_TIPO_ESTUDIO_UNIR => ++_ID_TIPO_ESTUDIO_UNIR;
        public static byte ID_TIPO_ASIGNATURA_UNIR => ++_ID_TIPO_ASIGNATURA_UNIR;
        public static int ID_TIPO_ASIGNATURA => ++_ID_TIPO_ASIGNATURA;
        public static byte ID_TIPO_ESTUDIO_SEGUN_UNIR => ++_ID_TIPO_ESTUDIO_SEGUN_UNIR;
        public static int ID_RAMA_CONOCIMIENTO => ++_ID_RAMA_CONOCIMIENTO;
        public static int ID_DURACION_PERIODO_LECTIVO => ++_ID_DURACION_PERIODO_LECTIVO;
        #endregion
    }
}
