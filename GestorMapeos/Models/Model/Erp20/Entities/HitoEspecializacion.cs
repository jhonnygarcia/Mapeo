namespace GestorMapeos.Models.Model.Erp20.Entities
{
    public class HitoEspecializacion
    {
        public int Id { get; set; }
        public int EspecializacionId { get; set; }
        public virtual Especializacion Especializacion { get; set; }

        public virtual Hito Hito { get; set; }
    }
}