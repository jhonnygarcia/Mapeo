using System.Collections.Generic;

namespace GestorMapeos.Models.Dto.Erp20
{
    public class HitoDto
    {
        public int Id { get; set; }
        public virtual IEnumerable<NodoDto> Nodos { get; set; }
        public virtual HitoEspecializacionDto HitoEspecializacion { get; set; }
    }
}
