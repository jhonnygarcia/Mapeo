namespace GestorMapeos.Models.Model.Erp20.Entities
{
    public class PlanOfertado
    {
        // Propiedades Primitivas	
        public int Id { get; set; }
        public int PeriodoAcademicoId { get; set; }
        public int PlanId { get; set; }
        public virtual PeriodoAcademico PeriodoAcademico { get; set; }
        public virtual Plan Plan { get; set; }
    }
}