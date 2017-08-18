namespace GestorMapeos.Models.Model.Gestor.Entities
{
    public class PeriodoEstudioAsignaturaUnir
    {
        public int Id { get; set; }
        public int PeriodoEstudioId { get; set; }
        public int AsignaturaId { get; set; }
        public bool Borrado { get; set; }
        public virtual PeriodoEstudioUnir PeriodoEstudioUnir { get; set; }
        public virtual AsignaturaUnir AsignaturaUnir { get; set; }
        public virtual PeriodoEstudioAsignaturaIntegracion PeriodoEstudioAsignaturaIntegracion { get; set; }
    }
}
