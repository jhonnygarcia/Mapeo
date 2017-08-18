using GestorMapeos.Models.Dto.Erp20;

namespace GestorMapeos.Models.Dto.Gestor
{
    public class AsignaturaIntegracionDto
    {
        public int Id { get; set; }
        public int AsignaturaPlanIntegracionId { get; set; }
        public virtual AsignaturaUnirDto AsignaturaUnir { get; set; }
        public virtual PlantillaAsignaturaIntegracionDto PlantillaAsignaturaIntegracion { get; set; }
        public AsignaturaPlanDto AsignaturaPlan { get; set; }
    }
}
