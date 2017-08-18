using System.Collections.Generic;

namespace GestorMapeos.Models.Model.Erp20.Entities
{
    public class Titulo
    {
        // Propiedades Primitivas	
        public	int Id { get; set; }
        public string Nombre { get; set; }
        public string CodigoMec { get; set; }

        public virtual ICollection<Especializacion> Especializaciones { get; set; }
    }
}