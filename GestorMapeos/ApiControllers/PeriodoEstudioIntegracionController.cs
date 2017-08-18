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
using GestorMapeos.Parameters.PeriodoEstudioIntegracion;
using GestorMapeos.Parameters.PlantillaEstudioIntegracion;
using GestorMapeos.TransferStructs;

namespace GestorMapeos.ApiControllers
{
    [RoutePrefix("api/v1/mapeo-periodo-estudio")]
    public class PeriodoEstudioIntegracionController : ApiControllerBase
    {
        private readonly IPeriodoEstudioIntegracionServices _periodoEstudioIntegracionServices;
        public PeriodoEstudioIntegracionController(IPeriodoEstudioIntegracionServices periodoEstudioIntegracionServices)
        {
            _periodoEstudioIntegracionServices = periodoEstudioIntegracionServices;
        }

        [HttpPost]
        [Route("advanced-search")]
        public IHttpActionResult AdvancedSearch(SearchPeriodoEstudioIntegracionParameters parameters)
        {
            var result = _periodoEstudioIntegracionServices.GetPagedPeriodoEstudio(parameters);
            if (!result.HasErrors)
            {
                var elementos = result.Elements.Select(a => new
                {
                    a.Id,
                    PeriodoEstudio = new
                    {
                        a.PeriodoEstudioUnir.Id,
                        Estudio = new
                        {
                            a.PeriodoEstudioUnir.EstudioUnir.Nombre
                        },
                        PeriodoMatriculacion = new
                        {
                            a.PeriodoEstudioUnir.PeriodoMatriculacionUnir.Nombre
                        }
                    },
                    PlanOfertado = a.PlanOfertado != null ? new
                    {
                        a.PlanOfertado.Id,
                        Plan = new
                        {
                            a.PlanOfertado.Plan.Nombre,
                            a.PlanOfertado.Plan.Codigo,
                            a.PlanOfertado.Plan.DisplayName
                        },
                        PeriodoAcademico = new
                        {
                            a.PlanOfertado.PeriodoAcademico.Nombre,
                            a.PlanOfertado.PeriodoAcademico.FechaInicio,
                            a.PlanOfertado.PeriodoAcademico.FechaFin,
                            a.PlanOfertado.PeriodoAcademico.DisplayName
                        }
                    } : null,
                    a.PlanOfertadoId,
                    a.PlantillaEstudioIntegracionId
                }).ToList();

                return ResultWithMessages(result, new { Elements = elementos, result.TotalPages, result.TotalElements });
            }
            return ResultWithMessages(result);
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var result = _periodoEstudioIntegracionServices.Get(id);
            if (result.Type == ResultType.Ok && result.Value != null)
            {
                var element = new
                {
                    result.Value.Id,
                    result.Value.PlantillaEstudioIntegracionId,
                    PlanOfertado = new 
                    {
                        result.Value.PlanOfertado.Id,
                        Plan = new 
                        {
                            result.Value.PlanOfertado.Plan.Id,
                            result.Value.PlanOfertado.Plan.Codigo,
                            result.Value.PlanOfertado.Plan.Nombre,
                            result.Value.PlanOfertado.Plan.DisplayName
                        },
                        PeriodoAcademico = new 
                        {
                            result.Value.PlanOfertado.PeriodoAcademico.Id,
                            result.Value.PlanOfertado.PeriodoAcademico.FechaInicio,
                            result.Value.PlanOfertado.PeriodoAcademico.FechaFin,
                            result.Value.PlanOfertado.PeriodoAcademico.Nombre,
                            result.Value.PlanOfertado.PeriodoAcademico.DisplayName
                        }
                    },
                    PeriodoEstudio = new 
                    {
                        result.Value.PeriodoEstudioUnir.Id,
                        Estudio = new 
                        {
                            result.Value.PeriodoEstudioUnir.EstudioUnir.Nombre
                        },
                        PeriodoMatriculacion = new 
                        {
                            result.Value.PeriodoEstudioUnir.PeriodoMatriculacionUnir.Nombre
                        }
                    }
                };
                return Ok(element);
            }
            return ResultWithMessages(result);
        }
        [HttpPut]
        [Route("")]
        public IHttpActionResult New(SavePeriodoEstudioIntegracionParameters parameters)
        {
            var result = _periodoEstudioIntegracionServices.Crear(parameters);
            if (result.HasErrors)
            {
                return ResultWithMessages(result);
            }
            return Ok();
        }
        [HttpPost]
        [Route("{id}")]
        public IHttpActionResult Modify(SavePeriodoEstudioIntegracionParameters parameters)
        {
            var result = _periodoEstudioIntegracionServices.Modificar(parameters);
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
            var result = _periodoEstudioIntegracionServices.Eliminar(parameters.Ids);
            return ResultWithMessages(result);
        }
    }
}
