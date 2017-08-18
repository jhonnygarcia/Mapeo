using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Net.Http;
using System.Net;
using System.Web.Http.Cors;
using GestorMapeos.ApiControllers.Base;
using GestorMapeos.Models.Services.Gestor;
using GestorMapeos.Parameters;
using GestorMapeos.Parameters.EstudioIntegracion;
using GestorMapeos.Parameters.PlantillaEstudioIntegracion;
using GestorMapeos.TransferStructs;

namespace GestorMapeos.ApiControllers
{
    [RoutePrefix("api/v1/mapeo-estudio")]
    public class EstudioIntegracionController : ApiControllerBase
    {
        private readonly IEstudioIntegracionServices _estudioIntegracionServices;
        public EstudioIntegracionController(IEstudioIntegracionServices estudioIntegracionServices)
        {
            _estudioIntegracionServices = estudioIntegracionServices;
        }

        [HttpPost]
        [Route("advanced-search")]
        public IHttpActionResult AdvancedSearch(SearchEstudioIntegracionParameters parameters)
        {
            var result = _estudioIntegracionServices.GetPagedEstudios(parameters);
            if (!result.HasErrors)
            {
                var elementos = result.Elements.Select(a => new
                {
                    a.Id,
                    IdEspecializacion = a.EspecializacionId,
                    PlantillaEstudioIntegracion = new
                    {
                      Id = a.PlantillaEstudioIntegracionId  
                    },
                    Estudio = new
                    {
                        a.Id,
                        a.EstudioUnir.Nombre
                    },
                    Plan = a.PlantillaEstudioIntegracion?.Plan != null? new
                    {
                        a.PlantillaEstudioIntegracion.Plan.Id,
                        a.PlantillaEstudioIntegracion.Plan.Codigo,
                        a.PlantillaEstudioIntegracion.Plan.Nombre,
                        a.PlantillaEstudioIntegracion.Plan.Anyo,
                        a.PlantillaEstudioIntegracion.Plan.DisplayName
                    } : null,
                    Especializacion = a.Especializacion != null ? new
                    {
                        Id = a.EspecializacionId,
                        a.Especializacion.Nombre
                    }: null
                }).ToList();

                return ResultWithMessages(result, new { Elements = elementos, result.TotalPages, result.TotalElements });
            }
            return ResultWithMessages(result);
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var result = _estudioIntegracionServices.Get(id);
            if (result.Type == ResultType.Ok && result.Value != null)
            {
                return Ok(result.Value);
            }
            return ResultWithMessages(result);
        }

        [HttpPut]
        [Route("")]
        public IHttpActionResult New(SaveEstudioIntegracionParameters parameters)
        {
            var result = _estudioIntegracionServices.Crear(parameters);
            if (result.HasErrors)
            {
                return ResultWithMessages(result);
            }
            return Ok();
        }
        [HttpPost]
        [Route("{id}")]
        public IHttpActionResult Modify(SaveEstudioIntegracionParameters parameters)
        {
            var result = _estudioIntegracionServices.Modificar(parameters);
            if (result.HasErrors)
            {
                return ResultWithMessages(result);
            }
            return Ok();
        }

        [HttpPost]
        [Route("delete")]
        public virtual IHttpActionResult Delete(DeletedViewModel parameters)
        {
            var result = _estudioIntegracionServices.Eliminar(parameters.Ids);
            return ResultWithMessages(result);
        }
        [HttpGet]
        [Route("search")]
        public IHttpActionResult GetFilteredList(int? idEstudioGestor = null, int? idPlanErp = null, int? idEspecializacionErp = null)
        {
            var result = _estudioIntegracionServices.GetFilteredList(idEstudioGestor, idPlanErp, idEspecializacionErp);
            if (!result.HasErrors)
            {
                var elements = result.Elements.Select(r => new
                {
                    IdEstudioGestor = r.Id,
                    IdPlanErp = r.PlantillaEstudioIntegracion.Id,
                    IdEspecializacionErp = r.EspecializacionId
                });

                return OkPagedList(elements, null, null);
            }
            return ResultWithMessages(result);
        }
    }
}
