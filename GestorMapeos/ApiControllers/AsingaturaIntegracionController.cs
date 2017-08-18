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
using GestorMapeos.Parameters.AsignaturaIntegracion;
using GestorMapeos.Parameters.PlantillaEstudioIntegracion;
using GestorMapeos.TransferStructs;

namespace GestorMapeos.ApiControllers
{
    [RoutePrefix("api/v1/mapeo-asignatura")]
    public class AsingaturaIntegracionController : ApiControllerBase
    {
        private readonly IAsignaturaIntegracionServices _asingaIntegracionServices;
        public AsingaturaIntegracionController(IAsignaturaIntegracionServices asingaIntegracionServices)
        {
            _asingaIntegracionServices = asingaIntegracionServices;
        }

        [HttpPost]
        [Route("advanced-search")]
        public IHttpActionResult AdvancedSearch(SearchAsignaturaIntegracionParameters parameters)
        {
            var result = _asingaIntegracionServices.GetPagedAsignatura(parameters);
            if (!result.HasErrors)
            {
                var elementos = result.Elements.Select(a => new
                {
                    a.Id,
                    a.AsignaturaUnir,
                    a.AsignaturaPlan,
                    a.AsignaturaPlanIntegracionId
                }).ToList();

                return ResultWithMessages(result, new { Elements = elementos, result.TotalPages,  result.TotalElements });
            }
            return ResultWithMessages(result);
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var result = _asingaIntegracionServices.Get(id);
            if (result.Type == ResultType.Ok && result.Value != null)
            {
                return Ok(result.Value);
            }
            return ResultWithMessages(result);
        }

        [HttpPut]
        [Route("")]
        public IHttpActionResult New(SaveAsignaturaIntegracionParameters parameters)
        {
            var result = _asingaIntegracionServices.Crear(parameters);
            if (result.HasErrors)
            {
                return ResultWithMessages(result);
            }
            return Ok();
        }
        [HttpPost]
        [Route("{id}")]
        public IHttpActionResult Modify(SaveAsignaturaIntegracionParameters parameters)
        {
            var result = _asingaIntegracionServices.Modificar(parameters);
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
            var result = _asingaIntegracionServices.Eliminar(parameters.Ids);
            return ResultWithMessages(result);
        }

        [HttpGet]
        [Route("search")]
        public IHttpActionResult GetFilteredList(int? idEstudioGestor = null, int? idAsignaturaEstudioGestor = null, int? idPlanErp = null, int? idAsignaturaPlanErp = null)
        {
            var result = _asingaIntegracionServices.GetFilteredList(idEstudioGestor, idAsignaturaEstudioGestor, idPlanErp, idAsignaturaPlanErp);
            if (!result.HasErrors)
            {
                var elementos = result.Elements.Select(a => new
                {
                    IdEstudioGestor = a.AsignaturaUnir.EstudioUnir.Id,
                    IdAsignaturaEstudioGestor = a.Id,
                    IdPlanErp = a.PlantillaAsignaturaIntegracion.PlanIntegracion.Id,
                    IdAsignaturaPlanErp = a.AsignaturaPlanIntegracionId
                }).ToList();

                return OkPagedList(elementos, null, null);
            }
            return ResultWithMessages(result);
        }
    }
}
