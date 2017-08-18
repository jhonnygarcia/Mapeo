using GestorMapeos.Models.Dto.Erp20;

namespace GestorMapeos.Models.Dto.Gestor
{
    public class PeriodoMatriculacionIntegracionDto
    {
        public int Id { get; set; }
        public int PeriodoAcademicoId { get; set; }
        public virtual PeriodoMatriculacionUnirDto PeriodoMatriculacion { get; set; }
        public PeriodoAcademicoDto PeriodoAcademico { get; set; }
    }
}