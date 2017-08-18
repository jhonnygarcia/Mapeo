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
using GestorMapeos.Parameters.GestorCommons;
using GestorMapeos.Parameters.PeriodoEstudioIntegracion;
using GestorMapeos.Parameters.PlantillaEstudioIntegracion;
using GestorMapeos.TransferStructs;

namespace GestorMapeos.ApiControllers
{
    [RoutePrefix("api/v1/gestor-commons")]
    public class GestorCommonsController : ApiControllerBase
    {
        private readonly IGestorCommonsServices _commonsServices;
        public GestorCommonsController(IGestorCommonsServices commonsServices)
        {
            _commonsServices = commonsServices;
        }
        [HttpPost]
        [Route("search-plantilla-estudio")]
        public IHttpActionResult SimpleSearchPlantillaEstudio(SimpleListViewModel parameters)
        {
            var result = _commonsServices.SimpleSearchPlantillaEstudio(parameters);
            if (!result.HasErrors)
            {
                var elements = result.Elements.Select(r => new
                {
                    r.Id,
                    Descripcion = r.Nombre + " - " + r.AnyoPlan,
                    TipoEstudio = r.TipoEstudio.Nombre,
                    Rama = r.Rama != null ? r.Rama.Nombre : ""
                }).ToList();

                return OkPagedList(elements, result.TotalElements, result.TotalPages);
            }
            return ResultWithMessages(result);
        }
        [HttpPost]
        [Route("search-periodo-matriculacion")]
        public IHttpActionResult SimpleSearchPeriodoMatriculacion(SimpleListViewModel parameters)
        {
            var result = _commonsServices.SimpleSearchPeriodoMatriculacion(parameters);
            if (!result.HasErrors)
            {
                var elements = result.Elements.Select(a => new
                {
                    a.Id,
                    Descripcion = a.Nombre,
                    a.AnyoAcademico,
                    FechaInicio = a.FechaInicio?.ToString("dd/MM/yyyy"),
                    FechaFin = a.FechaFin?.ToString("dd/MM/yyyy"),
                    a.Nro
                }).ToList();

                return OkPagedList(elements, result.TotalElements, result.TotalPages);
            }
            return ResultWithMessages(result);
        }
        [HttpPost]
        [Route("search-plantiila-asignatura")]
        public IHttpActionResult SimpleSearchPlantillaAsignatura(SimpleListPlantillaAsignaturaParameters parameters)
        {
            var result = _commonsServices.SimpleSearchPlantillaAsignatura(parameters);
            if (!result.HasErrors)
            {
                var elements = result.Elements.Select(pa => new
                {
                    pa.Id,
                    Descripcion = pa.NombreAsignatura,
                    pa.Codigo,
                    TipoAsignatura = pa.TipoAsignatura.Nombre,
                    pa.Creditos,
                    PlantillaEstudio = pa.PlantillaEstudio.Nombre + " - " + pa.PlantillaEstudio.AnyoPlan
                }).ToList();

                return OkPagedList(elements, result.TotalElements, result.TotalPages);
            }
            return ResultWithMessages(result);
        }
        [HttpPost]
        [Route("search-asignatura")]
        public IHttpActionResult SimpleSearchAsignatura(SimpleListAsignaturaParameters parameters)
        {
            var result = _commonsServices.SimpleSearchAsignatura(parameters);
            if (!result.HasErrors)
            {
                var elements = result.Elements.Select(a => new
                {
                    a.Id,
                    Descripcion = a.Nombre,
                    a.TipoAsignatura,
                    a.Creditos,
                    a.PeriodoLectivo,
                    a.Curso,
                    a.Activo,
                    Estudio = a.EstudioUnir.Nombre
                }).ToList();

                return OkPagedList(elements, result.TotalElements, result.TotalPages);
            }
            return ResultWithMessages(result);
        }
        [HttpPost]
        [Route("search-estudio")]
        public IHttpActionResult SimpleSearchEstudio(SimpleListEstudioParameters parameters)
        {
            var result = _commonsServices.SimpleSearchEstudio(parameters);
            if (!result.HasErrors)
            {
                var elements = result.Elements.Select(r => new
                {
                    r.Id,
                    Descripcion = r.Nombre,
                    Plan = r.PlanEstudio,
                    r.Activo,
                    TipoEstudio = new
                    {
                        r.TipoEstudio.Nombre
                    },
                    Rama = r.RamaEstudio,
                    Titulo = r.Titulo != null ? new
                    {
                        r.Titulo.Id,
                        r.Titulo.Codigo,
                        r.Titulo.Nombre,
                        DisplayName = r.Titulo.Codigo + " - " + r.Titulo.Nombre
                    } : null
                }).ToList();

                return OkPagedList(elements, result.TotalElements, result.TotalPages);
            }
            return ResultWithMessages(result);
        }

        [HttpPost]
        [Route("search-periodo-estudio")]
        public IHttpActionResult SearchPeriodoEstudio(SearchPeriodoEstudioParameters parameters)
        {
            var result = _commonsServices.GetPagedPeriodoEstudio(parameters);
            if (!result.HasErrors)
            {
                var elementos = result.Elements.Select(pe => new
                {
                    pe.Id,
                    Estudio = new
                    {
                        pe.EstudioUnir.Nombre
                    },
                    PeriodoMatriculacion = new
                    {
                        pe.PeriodoMatriculacionUnir.Nombre
                    }
                }).ToList();

                return ResultWithMessages(result, new { Elements = elementos, result.TotalPages, result.TotalElements });
            }
            return ResultWithMessages(result);
        }
        [HttpPost]
        [Route("search-periodo-estudio-asignatura")]
        public IHttpActionResult SearchPeriodoEstudioAsignatura(SearchPeriodoEstudioAsignaturaParameters parameters)
        {
            var result = _commonsServices.SearchPeriodoEstudioAsignatura(parameters);
            if (!result.HasErrors)
            {
                var elementos = result.Elements.Select(pe => new
                {
                    pe.Id,
                    Asignatura = new
                    {
                        pe.AsignaturaUnir.Id,
                        pe.AsignaturaUnir.Nombre,
                        pe.AsignaturaUnir.TipoAsignatura,
                        pe.AsignaturaUnir.Creditos,
                        pe.AsignaturaUnir.PeriodoLectivo,
                        pe.AsignaturaUnir.Curso,
                        pe.AsignaturaUnir.Activo,
                        Estudio = new
                        {
                            pe.AsignaturaUnir.EstudioUnir.Id,
                            pe.AsignaturaUnir.EstudioUnir.Nombre
                        }
                    },
                    PeriodoEstudio = new
                    {
                        pe.PeriodoEstudioUnir.Id,
                        PeriodoMatriculacion = new
                        {
                            pe.PeriodoEstudioUnir.PeriodoMatriculacionUnir.Id,
                            pe.PeriodoEstudioUnir.PeriodoMatriculacionUnir.Nombre
                        }
                    }
                }).ToList();

                return ResultWithMessages(result, new { Elements = elementos, result.TotalPages, result.TotalElements });
            }
            return ResultWithMessages(result);
        }
    }
}
