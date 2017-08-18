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
using GestorMapeos.Parameters.AsignaturaOfertadaIntegracion;
using GestorMapeos.Parameters.PeriodoEstudioAsignaturaIntegracion;
using GestorMapeos.Parameters.PlantillaEstudioIntegracion;
using GestorMapeos.TransferStructs;

namespace GestorMapeos.ApiControllers
{
    [RoutePrefix("api/v1/mapeo-periodo-estudio-asignatura")]
    public class PeriodoEstudioAsignaturaIntegracionController : ApiControllerBase
    {
        private readonly IPeriodoEstudioAsignaturaIntegracionServices _periodoEstudioAsignaturaIntegracionServices;
        public PeriodoEstudioAsignaturaIntegracionController(IPeriodoEstudioAsignaturaIntegracionServices periodoEstudioAsignaturaIntegracionServices)
        {
            _periodoEstudioAsignaturaIntegracionServices = periodoEstudioAsignaturaIntegracionServices;
        }

        [HttpPost]
        [Route("advanced-search")]
        public IHttpActionResult AdvancedSearch(SearchPeriodoEstudioAsignasturaIntegracionParameters parameters)
        {
            var result = _periodoEstudioAsignaturaIntegracionServices.GetPagedPeriodoEstudioAsignatura(parameters);
            if (!result.HasErrors)
            {
                var elementos = result.Elements.Select(a => new
                {
                    a.Id,
                    PeriodoEstudioAsignatura = new
                    {
                        a.PeriodoEstudioAsignaturaUnir.Id,
                        Asignatura = new
                        {
                            a.PeriodoEstudioAsignaturaUnir.AsignaturaUnir.Nombre,
                            Estudio = new
                            {
                                a.PeriodoEstudioAsignaturaUnir.AsignaturaUnir.EstudioUnir.Nombre
                            }
                        },
                        PeriodoEstudio = new
                        {
                            PeriodoMatriculacion = new
                            {
                                a.PeriodoEstudioAsignaturaUnir.PeriodoEstudioUnir.PeriodoMatriculacionUnir.Nombre
                            }
                        }
                    },
                    AsignaturaOfertada = a.AsignaturaOfertada != null ? new
                    {
                        a.AsignaturaOfertada.Id,
                        a.AsignaturaOfertada.Codigo,
                        a.AsignaturaOfertada.Nombre,
                        PeriodoLectivo = new 
                        {
                            a.AsignaturaOfertada.PeriodoLectivo.Nombre,
                            FechaInicio = a.AsignaturaOfertada.PeriodoLectivo.FechaInicio.ToString("dd/MM/yyyy"),
                            FechaFin = a.AsignaturaOfertada.PeriodoLectivo.FechaFin.ToString("dd/MM/yyyy")
                        },
                        PlanOfertado = new 
                        {
                            Plan = new 
                            {
                                a.AsignaturaOfertada.PlanOfertado.Plan.Codigo,
                                a.AsignaturaOfertada.PlanOfertado.Plan.Nombre
                            },
                            PeriodoAcademico = new 
                            {
                                a.AsignaturaOfertada.PlanOfertado.PeriodoAcademico.Nombre,
                                FechaInicio = a.AsignaturaOfertada.PlanOfertado.PeriodoAcademico.FechaInicio.ToString("dd/MM/yyyy"),
                                FechaFin = a.AsignaturaOfertada.PlanOfertado.PeriodoAcademico.FechaFin.ToString("dd/MM/yyyy")
                            }
                        }
                    }: null,
                    a.AsignaturaOfertadaId
                }).ToList();

                return ResultWithMessages(result, new { Elements = elementos, result.TotalPages, result.TotalElements });
            }
            return ResultWithMessages(result);
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var result = _periodoEstudioAsignaturaIntegracionServices.Get(id);
            if (result.Type == ResultType.Ok && result.Value != null)
            {
                var element = new
                {
                    result.Value.Id,
                    PeriodoEstudioAsignatura = new 
                    {
                        result.Value.PeriodoEstudioAsignaturaUnir.Id,
                        Asignatura = new 
                        {
                            result.Value.PeriodoEstudioAsignaturaUnir.AsignaturaUnir.Nombre,
                            result.Value.PeriodoEstudioAsignaturaUnir.AsignaturaUnir.TipoAsignatura,
                            result.Value.PeriodoEstudioAsignaturaUnir.AsignaturaUnir.Creditos,
                            result.Value.PeriodoEstudioAsignaturaUnir.AsignaturaUnir.PeriodoLectivo,
                            result.Value.PeriodoEstudioAsignaturaUnir.AsignaturaUnir.Curso,
                            result.Value.PeriodoEstudioAsignaturaUnir.AsignaturaUnir.Activo
                        },
                        PeriodoEstudio = new 
                        {
                            Id = result.Value.PeriodoEstudioAsignaturaUnir.PeriodoEstudioId,
                            Estudio = new 
                            {
                                result.Value.PeriodoEstudioAsignaturaUnir.PeriodoEstudioUnir.EstudioUnir.Id,
                                result.Value.PeriodoEstudioAsignaturaUnir.PeriodoEstudioUnir.EstudioUnir.Nombre
                            },
                            PeriodoMatriculacion = new 
                            {
                                result.Value.PeriodoEstudioAsignaturaUnir.PeriodoEstudioUnir.PeriodoMatriculacionUnir.Nombre
                            }
                        }
                    },
                    AsignaturaOfertada = new 
                    {
                        result.Value.AsignaturaOfertada.DisplayName,
                        result.Value.AsignaturaOfertada.Id,
                        result.Value.AsignaturaOfertada.Codigo,
                        result.Value.AsignaturaOfertada.Nombre,
                        result.Value.AsignaturaOfertada.UbicacionPeriodoLectivo,
                        Curso = result.Value.AsignaturaOfertada.Curso != null ? new 
                        {
                            result.Value.AsignaturaOfertada.Curso.Numero
                        } : null,
                        TipoAsignatura = new 
                        {
                            result.Value.AsignaturaOfertada.TipoAsignatura.Nombre
                        },
                        AsignaturaPlan = new 
                        {
                            Asignatura = new 
                            {
                                result.Value.AsignaturaOfertada.AsignaturaPlan.Asignatura.Codigo,
                                result.Value.AsignaturaOfertada.AsignaturaPlan.Asignatura.Nombre,
                                result.Value.AsignaturaOfertada.AsignaturaPlan.Asignatura.Creditos,
                                result.Value.AsignaturaOfertada.AsignaturaPlan.Asignatura.DisplayName
                            }
                        },
                        PeriodoLectivo = new 
                        {
                            result.Value.AsignaturaOfertada.PeriodoLectivo.DisplayName,
                            result.Value.AsignaturaOfertada.PeriodoLectivo.Nombre,
                            FechaInicio = result.Value.AsignaturaOfertada.PeriodoLectivo.FechaInicio.ToString("dd/MM/yyyy"),
                            FechaFin = result.Value.AsignaturaOfertada.PeriodoLectivo.FechaFin.ToString("dd/MM/yyyy"),
                            DuracionPeriodoLectivo = new 
                            {
                                result.Value.AsignaturaOfertada.PeriodoLectivo.DuracionPeriodoLectivo.Nombre
                            }
                        },
                        PlanOfertado = new 
                        {
                            result.Value.AsignaturaOfertada.PlanOfertado.Id,
                            Plan = new 
                            {
                                result.Value.AsignaturaOfertada.PlanOfertado.Plan.DisplayName,
                                result.Value.AsignaturaOfertada.PlanOfertado.Plan.Codigo,
                                result.Value.AsignaturaOfertada.PlanOfertado.Plan.Nombre
                            },
                            PeriodoAcademico = new 
                            {
                                result.Value.AsignaturaOfertada.PlanOfertado.PeriodoAcademico.Id,
                                result.Value.AsignaturaOfertada.PlanOfertado.PeriodoAcademico.DisplayName,
                                result.Value.AsignaturaOfertada.PlanOfertado.PeriodoAcademico.Nombre,
                                FechaInicio = result.Value.AsignaturaOfertada.PlanOfertado.PeriodoAcademico.FechaInicio.ToString("dd/MM/yyyy"),
                                FechaFin = result.Value.AsignaturaOfertada.PlanOfertado.PeriodoAcademico.FechaFin.ToString("dd/MM/yyyy")

                            }
                        }
                    }
                };
                return Ok(element);
            }
            return ResultWithMessages(result);
        }
        [HttpPut]
        [Route("")]
        public IHttpActionResult New(SavePeriodoEstudioAsignaturaIntegracionParameters parameters)
        {
            var result = _periodoEstudioAsignaturaIntegracionServices.Crear(parameters);
            if (result.HasErrors)
            {
                return ResultWithMessages(result);
            }
            return Ok();
        }
        [HttpPost]
        [Route("{id}")]
        public IHttpActionResult Modify(SavePeriodoEstudioAsignaturaIntegracionParameters parameters)
        {
            var result = _periodoEstudioAsignaturaIntegracionServices.Modificar(parameters);
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
            var result = _periodoEstudioAsignaturaIntegracionServices.Eliminar(parameters.Ids);
            return ResultWithMessages(result);
        }
    }
}
