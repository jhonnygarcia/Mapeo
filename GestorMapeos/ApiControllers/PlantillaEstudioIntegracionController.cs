using System;
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
using GestorMapeos.Parameters.PlantillaEstudioIntegracion;
using GestorMapeos.TransferStructs;

namespace GestorMapeos.ApiControllers
{
    [RoutePrefix("api/v1/mapeo-plantilla-estudio")]
    public class PlantillaEstudioIntegracionController : ApiControllerBase
    {
        private readonly IPlantillaEstudioIntegracionServices _plantillaEstudioServices;
        public PlantillaEstudioIntegracionController(IPlantillaEstudioIntegracionServices plantillaEstudioServices)
        {
            _plantillaEstudioServices = plantillaEstudioServices;
        }

        [HttpPost]
        [Route("advanced-search")]
        [Authorize]
        public IHttpActionResult AdvancedSearch(SearchPlantillaEstudioIntegracionParameters parameters)
        {
            var result = _plantillaEstudioServices.GetPagedPlantillasEstudio(parameters);
            if (!result.HasErrors)
            {
                var elementos = result.Elements.Select(a => new
                {
                    a.Id,
                    PlantillaEstudio = new
                    {
                        a.PlantillaEstudio.Id,
                        DisplayName = a.PlantillaEstudio.Nombre + " - " + a.PlantillaEstudio.AnyoPlan
                    },
                    Plan = a.Plan != null
                        ? new
                        {
                            a.Plan.Id,
                            DisplayName = a.Plan.Codigo + " - " + a.Plan.Nombre,
                            a.Plan.Anyo
                        }
                        : null
                }).ToList();

                return ResultWithMessages(result, new { Elements = elementos, result.TotalPages, result.TotalElements });
            }
            return ResultWithMessages(result);
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var result = _plantillaEstudioServices.Get(id);
            if (result.Type == ResultType.Ok && result.Value != null)
            {
                return Ok(result.Value);
            }
            return ResultWithMessages(result);
        }
        [HttpPut]
        [Route("")]
        public IHttpActionResult New(SavePlantillaEstudioIntegracionParameters parameters)
        {
            var result = _plantillaEstudioServices.Crear(parameters);
            if (result.HasErrors)
            {
                return ResultWithMessages(result);
            }
            return Ok();
        }
        [HttpPost]
        [Route("{id}")]
        public IHttpActionResult Modify(SavePlantillaEstudioIntegracionParameters parameters)
        {
            var result = _plantillaEstudioServices.Modificar(parameters);
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
            var result = _plantillaEstudioServices.Eliminar(parameters.Ids);
            return ResultWithMessages(result);
        }
        
        [HttpGet]
        [Route("search")]
        public IHttpActionResult GetFilteredList(int? idPlanErp = null, int? idPlantillaEstudio = null)
        {
            var result = _plantillaEstudioServices.GetFilteredList(idPlanErp, idPlantillaEstudio);
            if (!result.HasErrors)
            {
                var elements = result.Elements.Select(r => new
                {
                    IdPlanErp = r.Id,
                    IdPlantillaEstudio = r.PlantillaEstudio.Id
                });
                return OkPagedList(elements, null, null);
            }

            return ResultWithMessages(result);
        }
    }
}
