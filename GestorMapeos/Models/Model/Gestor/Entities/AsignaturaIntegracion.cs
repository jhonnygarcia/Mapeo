namespace GestorMapeos.Models.Model.Gestor.Entities
{
    public class AsignaturaIntegracion
    {
        public int Id { get; set; }
        public int AsignaturaPlanIntegracionId { get; set; }
        public virtual AsignaturaUnir AsignaturaUnir { get; set; }
        public virtual PlantillaAsignaturaIntegracion AsignaturaPlanIntegracion { get; set; }
    }
}
