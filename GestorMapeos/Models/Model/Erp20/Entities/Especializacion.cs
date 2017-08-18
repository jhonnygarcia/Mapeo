using System.Collections.Generic;

namespace GestorMapeos.Models.Model.Erp20.Entities
{
    public class Especializacion
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int TituloId { get; set; }
        public virtual Titulo Titulo { get; set; }
        public virtual ICollection<HitoEspecializacion> Hitos { get; set; }
    }
}