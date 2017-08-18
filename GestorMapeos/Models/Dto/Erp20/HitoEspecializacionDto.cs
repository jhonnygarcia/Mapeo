namespace GestorMapeos.Models.Dto.Erp20
{
    public class HitoEspecializacionDto
    {
        public int Id { get; set; }
        public int EspecializacionId { get; set; }
        public virtual EspecializacionDto Especializacion { get; set; }

        public virtual HitoDto Hito { get; set; }
    }
}