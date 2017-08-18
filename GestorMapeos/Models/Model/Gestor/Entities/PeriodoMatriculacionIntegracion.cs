namespace GestorMapeos.Models.Model.Gestor.Entities
{
    public class PeriodoMatriculacionIntegracion
    {
        public int Id { get; set; }
        public int PeriodoAcademicoId { get; set; }
        public virtual PeriodoMatriculacionUnir PeriodoMatriculacion { get; set; }
    }
}