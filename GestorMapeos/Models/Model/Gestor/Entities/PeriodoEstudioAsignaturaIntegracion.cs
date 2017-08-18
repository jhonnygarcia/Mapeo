namespace GestorMapeos.Models.Model.Gestor.Entities
{
    public class PeriodoEstudioAsignaturaIntegracion
    {
        public int Id { get; set; }
        public int AsignaturaOfertadaId { get; set; }
        public virtual PeriodoEstudioAsignaturaUnir PeriodoEstudioAsignaturaUnir { get; set; }
    }
}
