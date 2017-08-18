using GestorMapeos.Models.Dto.Erp20;

namespace GestorMapeos.Models.Dto.Gestor
{
    public class EstudioIntegracionDto
    {
        public int Id { get; set; }
        public int PlantillaEstudioIntegracionId { get; set; }
        public int? EspecializacionId { get; set; }
        
        public virtual  PlantillaEstudioIntegracionDto PlantillaEstudioIntegracion { get; set; }
        public virtual EstudioUnirDto EstudioUnir { get; set; }
        public EspecializacionDto Especializacion { get; set; }
    }
}