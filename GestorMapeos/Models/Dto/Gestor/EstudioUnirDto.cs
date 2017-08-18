using System.Collections.Generic;

namespace GestorMapeos.Models.Dto.Gestor
{
    public class EstudioUnirDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string PlanEstudio { get; set; }
        public string RamaEstudio { get; set; }
        public string Activo { get; set; }
        public string Borrado { get; set; }

        public byte TipoEstudioSegunUnirId { get; set; }
        public virtual TipoEstudioSegunUnirDto TipoEstudio { get; set; }
        public int? EstudioPrincipalUnirId { get; set; }
        public virtual EstudioPrincipalUnirDto Titulo { get; set; }
        public virtual IEnumerable<AsignaturaUnirDto> AsignaturasUnir { get; set; }
        public virtual IEnumerable<PlantillaEstudioDto> PlantillasEstudio { get; set; }
        public virtual IEnumerable<PeriodoEstudioUnirDto> PeriodosEstudioUnir { get; set; }
        public virtual EstudioIntegracionDto EstudioIntegracion { get; set; }
    }
}