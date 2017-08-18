using System.Collections.Generic;

namespace GestorMapeos.Models.Dto.Erp20
{
    public class EspecializacionDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int TituloId { get; set; }
        public virtual TituloDto Titulo { get; set; }
        public virtual IEnumerable<HitoEspecializacionDto> Hitos { get; set; }
    }
}