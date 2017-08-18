namespace GestorMapeos.Models.Dto.Gestor
{
    public class PeriodoEstudioAsignaturaUnirDto
    {
        public int Id { get; set; }
        public int PeriodoEstudioId { get; set; }
        public int AsignaturaId { get; set; }
        public bool Borrado { get; set; }
        public virtual PeriodoEstudioUnirDto PeriodoEstudioUnir { get; set; }
        public virtual AsignaturaUnirDto AsignaturaUnir { get; set; }
        public virtual PeriodoEstudioAsignaturaIntegracionDto PeriodoEstudioAsignaturaIntegracion { get; set; }
    }
}
