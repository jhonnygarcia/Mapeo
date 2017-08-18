using System.Collections.Generic;

namespace GestorMapeos.Models.Model.Erp20.Entities
{
    public class Nodo
    {
        public int Id { get; set; }
        public int PlanId { get; set; }
        public virtual Plan Plan { get; set; }
        public virtual ICollection<Hito> Hitos { get; set; }
    }
}