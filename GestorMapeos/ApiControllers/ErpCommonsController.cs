using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Net.Http;
using System.Net;
using System.Web.Http.Cors;
using GestorMapeos.ApiControllers.Base;
using GestorMapeos.Models.Services.Erp20;
using GestorMapeos.Parameters;
using GestorMapeos.Parameters.AsignaturaOfertadaIntegracion;
using GestorMapeos.Parameters.ErpCommons;
using GestorMapeos.Parameters.EstudioIntegracion;
using GestorMapeos.Parameters.PlanOfertado;

namespace GestorMapeos.ApiControllers
{
    [RoutePrefix("api/v1/erp-commons")]
    public class ErpCommonsController : ApiControllerBase
    {
        private readonly IErpCommonsServices _commonsServices;

        public ErpCommonsController(IErpCommonsServices commonsServices)
        {
            _commonsServices = commonsServices;
        }

        [HttpPost]
        [Route("search-plan")]
        public IHttpActionResult SimpleSearchPlan(SimpleListPlanParameters parameters)
        {
            var result = _commonsServices.SimpleSearchPlan(parameters);
            if (!result.HasErrors)
            {
                var elements = result.Elements.Select(r => new
                {
                    r.Id,
                    Descripcion = r.Codigo + " - " + r.Nombre,
                    r.Anyo,
                    r.EsOficial,
                    Estudio = r.Estudio.CodigoRuct + " - " + r.Estudio.Nombre,
                    TipoEstudio = r.Estudio.TipoEstudio.Nombre,
                    Rama = r.Estudio.RamaConocimiento.Nombre,
                    Titulo = r.Titulo.CodigoMec + " - " + r.Titulo.Nombre
                }).ToList();

                return OkPagedList(elements, result.TotalElements, result.TotalPages);
            }
            return ResultWithMessages(result);
        }
        [HttpPost]
        [Route("search-titulo")]
        public IHttpActionResult SimpleSearchTitulos(SimpleListViewModel parameters)
        {
            var result = _commonsServices.SimpleSearchTitulos(parameters);
            if (!result.HasErrors)
            {
                var elements = result.Elements.Select(r => new
                {
                    r.Id,
                    r.CodigoMec,
                    Descripcion = r.CodigoMec + " - " + r.Nombre,
                    DisplayName = r.CodigoMec + " - " + r.Nombre,
                }).ToList();

                return OkPagedList(elements, result.TotalElements, result.TotalPages);
            }
            return ResultWithMessages(result);
        }

        [HttpPost]
        [Route("search-especializacion")]
        public IHttpActionResult SimpleSearchEspecializaciones(SimpleListEspecializacionViewModel parameters)
        {
            var result = _commonsServices.SimpleSearchEspecializaciones(parameters);
            if (!result.HasErrors)
            {
                var elements = result.Elements.Select(r => new
                {
                    r.Id,
                    Descripcion = r.Nombre
                }).ToList();

                return OkPagedList(elements, result.TotalElements, result.TotalPages);
            }
            return ResultWithMessages(result);
        }

        [HttpPost]
        [Route("search-periodo-academico")]
        public IHttpActionResult SimpleSearchPeriodoAcademico(SimpleListViewModel parameters)
        {
            var result = _commonsServices.SimpleSearchPeriodoAcademico(parameters);
            if (!result.HasErrors)
            {
                var elements = result.Elements.Select(a => new
                {
                    a.Id,
                    Descripcion = a.Nombre,
                    a.AnyoAcademico,
                    FechaInicio = a.FechaInicio.ToString("dd/MM/yyyy"),
                    FechaFin = a.FechaFin.ToString("dd/MM/yyyy"),
                    a.Nro
                }).ToList();

                return OkPagedList(elements, result.TotalElements, result.TotalPages);
            }
            return ResultWithMessages(result);
        }

        [HttpPost]
        [Route("search-asignatura-plan")]
        public IHttpActionResult SimpleSearchAsignaturaPlan(SimpleListAsignaturaPlanParameters parameters)
        {
            var result = _commonsServices.SimpleSearchAsignaturaPlan(parameters);
            if (!result.HasErrors)
            {
                var elements = result.Elements.Select(ap => new
                {
                    ap.Id,
                    Descripcion = ap.Asignatura.Codigo + " - " + ap.Asignatura.Nombre,
                    ap.Asignatura.Creditos,
                    TipoAsignatura = ap.Asignatura.TipoAsignatura.Nombre,
                    PlanEstudio = ap.Plan.Codigo + " - " + ap.Plan.Nombre,
                    DuracionPeriodoLectivo = ap.DuracionPeriodoLectivo.Nombre,
                    ap.UbicacionPeriodoLectivo,
                    Curso = ap.Curso?.Numero
                }).ToList();

                return OkPagedList(elements, result.TotalElements, result.TotalPages);
            }
            return ResultWithMessages(result);
        }

        [HttpPost]
        [Route("search-asignatura-ofertada")]
        public IHttpActionResult SimpleSearchAsignaturaOfertada(SimpleListAsignaturaOfertadaParameters parameters)
        {
            var result = _commonsServices.GetPageAsignaturaOfertada(parameters);
            if (!result.HasErrors)
            {
                var elements = result.Elements.Select(ao => new
                {
                    ao.Id,
                    Descripcion = ao.DisplayName,
                    ao.DisplayName,
                    ao.Codigo,
                    ao.Nombre,
                    ao.UbicacionPeriodoLectivo,
                    Curso = ao.Curso != null ? new
                    {
                        ao.Curso.Numero
                    } : null,
                    PeriodoLectivo = new
                    {
                        ao.PeriodoLectivo.DisplayName,
                        ao.PeriodoLectivo.Nombre,
                        ao.PeriodoLectivo.FechaInicio,
                        ao.PeriodoLectivo.FechaFin,
                        DuracionPeriodoLectivo = new
                        {
                            ao.PeriodoLectivo.DuracionPeriodoLectivo.Nombre
                        }
                    },
                    PlanOfertado = new
                    {
                        ao.PlanOfertado.Id,
                        Plan = new
                        {
                            ao.PlanOfertado.Plan.Codigo,
                            ao.PlanOfertado.Plan.Nombre,
                            ao.PlanOfertado.Plan.DisplayName
                        },
                        PeriodoAcademico = new
                        {
                            ao.PlanOfertado.PeriodoAcademico.DisplayName,
                            ao.PlanOfertado.PeriodoAcademico.Nombre,
                            ao.PlanOfertado.PeriodoAcademico.FechaInicio,
                            ao.PlanOfertado.PeriodoAcademico.FechaFin
                        },
                    },
                    TipoAsignatura = new
                    {
                        ao.TipoAsignatura.Nombre
                    },
                    AsignaturaPlan = new
                    {
                        Asignatura = new
                        {
                            ao.AsignaturaPlan.Asignatura.Codigo,
                            ao.AsignaturaPlan.Asignatura.Creditos,
                            ao.AsignaturaPlan.Asignatura.Nombre,
                            ao.AsignaturaPlan.Asignatura.DisplayName
                        },
                    }
                }).ToList();

                return OkPagedList(elements, result.TotalElements, result.TotalPages);
            }
            return ResultWithMessages(result);
        }
        [HttpPost]
        [Route("search-plan-ofertado")]
        public IHttpActionResult SearchPeriodoEstudio(SearchPlanOfertadoParameters parameters)
        {
            var result = _commonsServices.GetPagePlanOfertado(parameters);
            if (!result.HasErrors)
            {
                var elementos = result.Elements.Select(a => new
                {
                    a.Id,
                    Plan = new
                    {
                        a.Plan.Codigo,
                        a.Plan.Nombre,
                        a.Plan.DisplayName
                    },
                    PeriodoAcademico = new
                    {
                        a.PeriodoAcademico.Nombre,
                        a.PeriodoAcademico.FechaInicio,
                        a.PeriodoAcademico.FechaFin,
                        a.PeriodoAcademico.DisplayName
                    }
                }).ToList();

                return ResultWithMessages(result, new { Elements = elementos, result.TotalPages, result.TotalElements });
            }
            return ResultWithMessages(result);
        }
    }
}
