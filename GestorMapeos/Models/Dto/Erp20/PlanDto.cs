using System.Collections.Generic;

namespace GestorMapeos.Models.Dto.Erp20
{
    public class PlanDto
    {
        // Propiedades Primitivas	
        public	int Id { get; set; }
        public int Anyo { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public bool EsOficial { get; set; }
        public int EstudioId { get; set; }
        public int TituloId { get; set; }
        public virtual EstudioDto Estudio { get; set; }
        public virtual TituloDto Titulo { get; set; }
        public virtual IEnumerable<NodoDto> Nodos { get; set; }
        public virtual IEnumerable<PlanOfertadoDto> PlanesOfertados { get; set; }
        public string DisplayName => Codigo + " - " + Nombre;
    }
}