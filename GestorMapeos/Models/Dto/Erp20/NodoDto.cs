using System.Collections.Generic;

namespace GestorMapeos.Models.Dto.Erp20
{
    public class NodoDto
    {
        public int Id { get; set; }
        public int PlanId { get; set; }
        public virtual PlanDto Plan { get; set; }
        public virtual IEnumerable<HitoDto> Hitos { get; set; }
    }
}