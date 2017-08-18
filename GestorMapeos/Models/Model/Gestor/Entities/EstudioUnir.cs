using System.Collections.Generic;

namespace GestorMapeos.Models.Model.Gestor.Entities
{
    public class EstudioUnir
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string PlanEstudio { get; set; }
        public string RamaEstudio { get; set; }
        public string Activo { get; set; }
        public string Borrado { get; set; }

        public byte TipoEstudioSegunUnirId { get; set; }
        public virtual TipoEstudioSegunUnir TipoEstudioSegunUnir { get; set; }
        public int? EstudioPrincipalUnirId { get; set; }
        public virtual EstudioPrincipalUnir EstudioPrincipalUnir { get; set; }
        public virtual ICollection<AsignaturaUnir> AsignaturasUnir { get; set; }
        public virtual ICollection<PlantillaEstudio> PlantillasEstudio { get; set; }
        public virtual ICollection<PeriodoEstudioUnir> PeriodosEstudioUnir { get; set; }
        public virtual EstudioIntegracion EstudioIntegracion { get; set; }
    }
}