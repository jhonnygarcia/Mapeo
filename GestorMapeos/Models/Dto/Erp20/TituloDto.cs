using System.Collections.Generic;

namespace GestorMapeos.Models.Dto.Erp20
{
    public class TituloDto
    {
        // Propiedades Primitivas	
        public	int Id { get; set; }
        public string Nombre { get; set; }
        public string CodigoMec { get; set; }
        public virtual IEnumerable<EspecializacionDto> Especializaciones { get; set; }

        public string DisplayName => CodigoMec + " - " + Nombre;
    }
}