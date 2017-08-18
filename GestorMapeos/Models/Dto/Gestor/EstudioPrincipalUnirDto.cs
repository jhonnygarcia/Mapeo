using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestorMapeos.Models.Dto.Gestor
{
    public class EstudioPrincipalUnirDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string DisplayName => Codigo + " - " + Nombre;
    }
}