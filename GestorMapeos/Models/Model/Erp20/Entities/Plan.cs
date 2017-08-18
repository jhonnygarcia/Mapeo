using System.Collections.Generic;

namespace GestorMapeos.Models.Model.Erp20.Entities
{
    public class Plan
    {
        // Propiedades Primitivas	
        public	int Id { get; set; }
        public int Anyo { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public bool EsOficial { get; set; }
        public int EstudioId { get; set; }
        public int TituloId { get; set; }
        public virtual Estudio Estudio { get; set; }
        public virtual Titulo Titulo { get; set; }
        public virtual ICollection<Nodo> Nodos { get; set; }
        public virtual ICollection<PlanOfertado> PlanesOfertados { get; set; }
    }
}