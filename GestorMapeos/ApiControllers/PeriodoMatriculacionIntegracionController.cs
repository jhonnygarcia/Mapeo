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
using GestorMapeos.Parameters.PeriodoEstudioAsignaturaIntegracion;
using GestorMapeos.Parameters.PeriodoMatriculacionIntegracion;
using GestorMapeos.Parameters.PlantillaEstudioIntegracion;
using GestorMapeos.TransferStructs;

namespace GestorMapeos.ApiControllers
{
    [RoutePrefix("api/v1/mapeo-periodo-matriculacion")]
    public class PeriodoMatriculacionIntegracionController : ApiControllerBase
    {
        private readonly IPeriodoMatriculacionIntegracionServices _periodoMatriculacionIntegracionServices;
        public PeriodoMatriculacionIntegracionController(IPeriodoMatriculacionIntegracionServices periodoMatriculacionIntegracionServices)
        {
            _periodoMatriculacionIntegracionServices = periodoMatriculacionIntegracionServices;
        }

        [HttpPost]
        [Route("advanced-search")]
        public IHttpActionResult AdvancedSearch(SearchPeriodoMatriculacionIntegracionParameters parameters)
        {
            var result = _periodoMatriculacionIntegracionServices.GetPagedPeriodoMatriculacion(parameters);
            if (!result.HasErrors)
            {
                var elementos = result.Elements.Select(a => new
                {
                    a.Id,
                    PeriodoMatriculacion = new
                    {
                        a.PeriodoMatriculacion.Nombre,
                        a.PeriodoMatriculacion.Id,
                        FechaInicio = a.PeriodoMatriculacion.FechaInicio?.ToString("dd/MM/yyyy") ?? "",
                        FechaFin = a.PeriodoMatriculacion.FechaFin?.ToString("dd/MM/yyyy") ?? "",
                        a.PeriodoMatriculacion.Nro
                    },
                    PeriodoAcademico = a.PeriodoAcademico != null ? new
                    {
                        a.PeriodoAcademico.Id,
                        a.PeriodoAcademico.Nombre,
                        FechaInicio = a.PeriodoAcademico.FechaInicio.ToString("dd/MM/yyyy"),
                        FechaFin = a.PeriodoAcademico.FechaFin.ToString("dd/MM/yyyy"),
                        a.PeriodoAcademico.Nro
                    } : null,
                    a.PeriodoAcademicoId
                }).ToList();

                return ResultWithMessages(result, new { Elements = elementos, result.TotalPages, result.TotalElements });
            }
            return ResultWithMessages(result);
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var result = _periodoMatriculacionIntegracionServices.Get(id);
            if (result.Type == ResultType.Ok && result.Value != null)
            {
                var element = new
                {
                    PeriodoMatriculacion = new
                    {
                        result.Value.PeriodoMatriculacion.Id,
                        result.Value.PeriodoMatriculacion.Nombre,
                        result.Value.PeriodoMatriculacion.AnyoAcademico,
                        FechaInicio = result.Value.PeriodoMatriculacion.FechaInicio?.ToString("dd/MM/yyyy") ?? "",
                        FechaFin = result.Value.PeriodoMatriculacion.FechaFin?.ToString("dd/MM/yyyy") ?? "",
                        result.Value.PeriodoMatriculacion.Nro
                    },
                    PeriodoAcademico = new
                    {
                        result.Value.PeriodoAcademico.Id,
                        Descripcion = result.Value.PeriodoAcademico.Nombre,
                        AnyoAcademico = new
                        {
                            result.Value.PeriodoAcademico.AnyoAcademico.AnyoInicio,
                            result.Value.PeriodoAcademico.AnyoAcademico.AnyoFin,
                            result.Value.PeriodoAcademico.AnyoAcademico.DisplayName
                        },
                        FechaInicio = result.Value.PeriodoAcademico.FechaInicio.ToString("dd/MM/yyyy"),
                        FechaFin = result.Value.PeriodoAcademico.FechaFin.ToString("dd/MM/yyyy"),
                        result.Value.PeriodoAcademico.Nro
                    }
                };
                return Ok(element);
            }
            return ResultWithMessages(result);
        }
        [HttpPut]
        [Route("")]
        public IHttpActionResult New(SavePeriodoMatriculacionIntegracionParameters parameters)
        {
            var result = _periodoMatriculacionIntegracionServices.Crear(parameters);
            if (result.HasErrors)
            {
                return ResultWithMessages(result);
            }
            return Ok();
        }
        [HttpPost]
        [Route("{id}")]
        public IHttpActionResult Modify(SavePeriodoMatriculacionIntegracionParameters parameters)
        {
            var result = _periodoMatriculacionIntegracionServices.Modificar(parameters);
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
            var result = _periodoMatriculacionIntegracionServices.Eliminar(parameters.Ids);
            return ResultWithMessages(result);
        }
    }
}
