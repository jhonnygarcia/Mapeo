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
using GestorMapeos.Parameters.PlantillaAsignaturaIntegracion;
using GestorMapeos.Parameters.EstudioIntegracion;
using GestorMapeos.Parameters.PlantillaEstudioIntegracion;
using GestorMapeos.TransferStructs;

namespace GestorMapeos.ApiControllers
{
    [RoutePrefix("api/v1/mapeo-plantilla-asignatura")]
    public class PlantillaAsignaturaIntegracionController : ApiControllerBase
    {
        private readonly IPlantillaAsignaturaIntegracionServices _plantillaAsignaturaIntegracionServices;
        public PlantillaAsignaturaIntegracionController(IPlantillaAsignaturaIntegracionServices plantillaAsignaturaIntegracionServices)
        {
            _plantillaAsignaturaIntegracionServices = plantillaAsignaturaIntegracionServices;
        }

        [HttpPost]
        [Route("advanced-search")]
        public IHttpActionResult AdvancedSearch(SearchPlantillaAsignaturaIntegracionParameters parameters)
        {
            var result = _plantillaAsignaturaIntegracionServices.GetPagedPlantillaAsignatura(parameters);
            if (!result.HasErrors)
            {
                var elementos = result.Elements.Select(a => new
                {
                    a.Id,
                   a.PlantillaAsignatura,
                   a.AsignaturaPlan
                }).ToList();

                return ResultWithMessages(result, new { Elements = elementos, result.TotalPages, result.TotalElements });
            }
            return ResultWithMessages(result);
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var result = _plantillaAsignaturaIntegracionServices.Get(id);
            if (result.Type == ResultType.Ok && result.Value != null)
            {
                return Ok(result.Value);
            }
            return ResultWithMessages(result);
        }

        [HttpPut]
        [Route("")]
        public IHttpActionResult New(SavePlantillaAsignaturaIntegracionParameters parameters)
        {
            var result = _plantillaAsignaturaIntegracionServices.Crear(parameters);
            if (result.HasErrors)
            {
                return ResultWithMessages(result);
            }
            return Ok();
        }
        [HttpPost]
        [Route("{id}")]
        public IHttpActionResult Modify(SavePlantillaAsignaturaIntegracionParameters parameters)
        {
            var result = _plantillaAsignaturaIntegracionServices.Modificar(parameters);
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
            var result = _plantillaAsignaturaIntegracionServices.Eliminar(parameters.Ids);
            return ResultWithMessages(result);
        }
        [HttpGet]
        [Route("search")]
        public IHttpActionResult GetFilteredList(int? idPlantillaEstudio = null, int? idPlantillaAsignatura = null, int? idPlanErp = null, int? idAsignaturaPlanErp = null)
        {
            var result = _plantillaAsignaturaIntegracionServices.GetFilteredList(idPlantillaEstudio, idPlantillaAsignatura, idPlanErp, idAsignaturaPlanErp);
            if (!result.HasErrors)
            {
                var elements = result.Elements.Select(r => new
                {
                    IdPlantillaEstudio = r.PlantillaAsignatura.PlantillaEstudio.Id,
                    IdPlantillaAsignatura = r.PlantillaAsignatura.Id,
                    IdPlanErp = r.PlanIntegracion.Id,
                    IdAsignaturaPlanErp = r.Id
                }).ToList();

                return OkPagedList(elements, null, null);
            }
            return ResultWithMessages(result);
        }
    }
}
