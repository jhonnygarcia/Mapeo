using GestorMapeos.Models.Dto.Erp20;

namespace GestorMapeos.Models.Dto.Gestor
{
    public class PeriodoEstudioAsignaturaIntegracionDto
    {
        public int Id { get; set; }
        public int AsignaturaOfertadaId { get; set; }
        public virtual PeriodoEstudioAsignaturaUnirDto PeriodoEstudioAsignaturaUnir { get; set; }
        public AsignaturaOfertadaDto AsignaturaOfertada { get; set; }
    }
}
