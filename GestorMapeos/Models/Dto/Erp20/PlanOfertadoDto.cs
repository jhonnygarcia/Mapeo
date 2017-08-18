namespace GestorMapeos.Models.Dto.Erp20
{
    public class PlanOfertadoDto
    {
        // Propiedades Primitivas	
        public int Id { get; set; }
        public int PeriodoAcademicoId { get; set; }
        public int PlanId { get; set; }
        public virtual PeriodoAcademicoDto PeriodoAcademico { get; set; }
        public virtual PlanDto Plan { get; set; }
    }
}