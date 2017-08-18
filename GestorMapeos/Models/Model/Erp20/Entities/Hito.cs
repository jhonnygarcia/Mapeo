using System.Collections.Generic;

namespace GestorMapeos.Models.Model.Erp20.Entities
{
    public class Hito
    {
        public int Id { get; set; }
        public virtual ICollection<Nodo> Nodos { get; set; }
        public virtual HitoEspecializacion HitoEspecializacion { get; set; }
    }
}
