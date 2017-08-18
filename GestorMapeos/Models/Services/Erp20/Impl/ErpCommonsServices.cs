using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GestorMapeos.Models.Dto.Erp20;
using GestorMapeos.Models.Dto.Gestor;
using GestorMapeos.Models.Model.Erp20;
using GestorMapeos.Models.Model.Erp20.Entities;
using GestorMapeos.Models.Model.Gestor;
using GestorMapeos.Parameters;
using GestorMapeos.Parameters.ErpCommons;
using GestorMapeos.Parameters.EstudioIntegracion;
using GestorMapeos.Parameters.PlanOfertado;
using GestorMapeos.Parameters.PlantillaEstudioIntegracion;
using GestorMapeos.TransferStructs;

namespace GestorMapeos.Models.Services.Erp20.Impl
{
    public class ErpCommonsServices : IErpCommonsServices
    {
        private readonly ErpContext _erpContext;
        public ErpCommonsServices(ErpContext erpContext)
        {
            _erpContext = erpContext;
        }

        public ResultList<PlanDto> SimpleSearchPlan(SimpleListPlanParameters parameters)
        {
            var result = new ResultList<PlanDto>();
            var query = _erpContext.Plan.AsQueryable();
            if (parameters.IdPeriodoAcademico.HasValue)
                query = query.Where(p => p.PlanesOfertados
                    .Any(po => po.PeriodoAcademicoId == parameters.IdPeriodoAcademico.Value));
            if (!string.IsNullOrEmpty(parameters.Descripcion))
                query = query.Where(p => (p.Codigo + " - " + p.Nombre).Contains(parameters.Descripcion));

            query = query.OrderBy(r => r.Codigo + " - " + r.Nombre);

            var listado = query.Select(a => new PlanDto
            {
                Id = a.Id,
                Codigo = a.Codigo,
                Nombre = a.Nombre,
                Anyo = a.Anyo,
                EsOficial = a.EsOficial,
                Estudio = new EstudioDto
                {
                    CodigoRuct = a.Estudio.CodigoRuct,
                    Nombre = a.Estudio.Nombre,
                    TipoEstudio = new TipoEstudioDto
                    {
                        Nombre = a.Estudio.TipoEstudio.Nombre
                    },
                    RamaConocimiento = new RamaConocimientoDto
                    {
                        Nombre = a.Nombre
                    }
                },
                Titulo = new TituloDto
                {
                    CodigoMec = a.Titulo.CodigoMec,
                    Nombre = a.Titulo.Nombre
                }
            }).Skip((parameters.PageIndex - 1) * parameters.ItemsPerPage)
                .Take(parameters.ItemsPerPage)
                .ToList();

            result.Elements = listado;
            result.TotalElements = query.Count();
            result.PageCount = parameters.ItemsPerPage;
            return result;
        }
        public ResultList<TituloDto> SimpleSearchTitulos(SimpleListViewModel parameters)
        {
            var result = new ResultList<TituloDto>();
            var query = _erpContext.Titulo.AsQueryable();
            if (!string.IsNullOrEmpty(parameters.Descripcion))
                query = query.Where(p => (p.CodigoMec + " - " + p.Nombre).Contains(parameters.Descripcion));

            query = query.OrderBy(r => r.CodigoMec + " - " + r.Nombre);
            var listado = query.Select(r => new TituloDto
            {
                Id = r.Id,
                CodigoMec = r.CodigoMec,
                Nombre = r.Nombre
            }).Skip((parameters.PageIndex - 1) * parameters.ItemsPerPage)
                .Take(parameters.ItemsPerPage)
                .ToList();
            result.Elements = listado;
            result.TotalElements = query.Count();
            result.PageCount = parameters.ItemsPerPage;
            return result;
        }
        public ResultList<EspecializacionDto> SimpleSearchEspecializaciones(SimpleListEspecializacionViewModel parameters)
        {
            var result = new ResultList<EspecializacionDto>();
            var query = _erpContext.Especializacion.AsQueryable();
            if (parameters.FilterPlanEstudio.HasValue)
            {
                var plan = _erpContext.Plan.Find(parameters.FilterPlanEstudio.Value);
                query = query.Where(a => a.Titulo.Id == plan.TituloId);
            }
            if (parameters.FilterTitulo.HasValue)
            {
                query = query.Where(a => a.Titulo.Id == parameters.FilterTitulo.Value);
            }
            if (!string.IsNullOrEmpty(parameters.Descripcion))
                query = query.Where(p => p.Nombre.Contains(parameters.Descripcion));

            query = query.OrderBy(a => a.Nombre);

            var listado = query.Select(a => new EspecializacionDto
            {
                Id = a.Id,
                Nombre = a.Nombre
            }).Skip((parameters.PageIndex - 1) * parameters.ItemsPerPage)
                .Take(parameters.ItemsPerPage)
                .ToList();

            result.Elements = listado;
            result.TotalElements = query.Count();
            result.PageCount = parameters.ItemsPerPage;

            return result;
        }
        public ResultList<PeriodoAcademicoDto> SimpleSearchPeriodoAcademico(SimpleListViewModel parameters)
        {
            var result = new ResultList<PeriodoAcademicoDto>();

            var query = _erpContext.PeriodosAcademicos.AsQueryable();
            if (!string.IsNullOrEmpty(parameters.Descripcion))
                query = query.Where(p => p.Nombre.Contains(parameters.Descripcion));

            query = query.OrderBy(a => a.Nombre);

            var listado = query.Select(a => new PeriodoAcademicoDto
            {
                Id = a.Id,
                Nombre = a.Nombre,
                AnyoAcademico = new AnyoAcademicoDto
                {
                    AnyoInicio = a.AnyoAcademico.AnyoInicio,
                    AnyoFin = a.AnyoAcademico.AnyoFin
                },
                FechaInicio = a.FechaInicio,
                FechaFin = a.FechaFin,
                Nro = a.Nro
            }).Skip((parameters.PageIndex - 1) * parameters.ItemsPerPage)
                .Take(parameters.ItemsPerPage)
                .ToList();

            result.Elements = listado;
            result.TotalElements = query.Count();
            result.PageCount = parameters.ItemsPerPage;

            return result;
        }
        public ResultList<AsignaturaPlanDto> SimpleSearchAsignaturaPlan(SimpleListAsignaturaPlanParameters parameters)
        {
            var result = new ResultList<AsignaturaPlanDto>();
            var query = _erpContext.AsignaturaPlan.Where(ap => true);
            if (parameters.IdPlanEstudio.HasValue)
                query = query.Where(ap => ap.PlanId == parameters.IdPlanEstudio.Value);
            if (!string.IsNullOrEmpty(parameters.Descripcion))
                query = query.Where(ap => (ap.Asignatura.Codigo + " - " + ap.Asignatura.Nombre).Contains(parameters.Descripcion));

            query = query.OrderBy(ap => ap.Asignatura.Codigo + " - " + ap.Asignatura.Nombre);

            var listado = query.Select(ap => new AsignaturaPlanDto
            {
                Id = ap.Id,
                Asignatura = new AsignaturaDto
                {
                    Codigo = ap.Asignatura.Codigo,
                    Nombre = ap.Asignatura.Nombre,
                    Creditos = ap.Asignatura.Creditos,
                    TipoAsignatura = new TipoAsignaturaDto
                    {
                        Nombre = ap.Asignatura.TipoAsignatura.Nombre
                    }
                },
                Plan = new PlanDto
                {
                    Codigo = ap.Plan.Codigo,
                    Nombre = ap.Plan.Nombre
                },
                DuracionPeriodoLectivo = new DuracionPeriodoLectivoDto
                {
                    Nombre = ap.DuracionPeriodoLectivo.Nombre
                },
                UbicacionPeriodoLectivo = ap.UbicacionPeriodoLectivo,
                Curso = ap.Curso != null ? new CursoDto
                {
                    Numero = ap.Curso.Numero
                } : null
            }).Skip((parameters.PageIndex - 1) * parameters.ItemsPerPage)
                .Take(parameters.ItemsPerPage)
                .ToList();

            result.Elements = listado;
            result.TotalElements = query.Count();
            result.PageCount = parameters.ItemsPerPage;
            return result;
        }
        public ResultList<AsignaturaOfertadaDto> GetPageAsignaturaOfertada(SimpleListAsignaturaOfertadaParameters parameters)
        {
            var result = new ResultList<AsignaturaOfertadaDto>();
            var query = _erpContext.AsignaturaOfertada.AsQueryable();
            if (!string.IsNullOrEmpty(parameters.Descripcion))
                query = query.Where(a => (a.Codigo + " - " + a.Nombre).Contains(parameters.Descripcion));
            if (parameters.IdAsignaturaOfertada.HasValue)
                query = query.Where(ao => ao.Id == parameters.IdAsignaturaOfertada.Value);
            if (parameters.IdPeriodoAcademico.HasValue)
                query = query.Where(ao => ao.PlanOfertado.PeriodoAcademicoId == parameters.IdPeriodoAcademico.Value);
            if (parameters.IdPlanEstudio.HasValue)
                query = query.Where(ao => ao.PlanOfertado.PlanId == parameters.IdPlanEstudio.Value);
            if (parameters.FilterIdAsignaturaOfertada.HasValue)
                query = query.Where(ao => ao.Id == parameters.FilterIdAsignaturaOfertada.Value);

            var order = parameters.GetEnum(SimpleListAsignaturaOfertadaParameters.AsignaturaOfertadaOrderColumn.AsignaturaOfertada);
            switch (order)
            {
                case SimpleListAsignaturaOfertadaParameters.AsignaturaOfertadaOrderColumn.IdAsignaturaOfertada:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.Id)
                        : query.OrderByDescending(o => o.Id);
                    break;
                case SimpleListAsignaturaOfertadaParameters.AsignaturaOfertadaOrderColumn.AsignaturaOfertada:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.Codigo + " - " + o.Nombre)
                        : query.OrderByDescending(o => o.Codigo + " - " + o.Nombre);
                    break;
                case SimpleListAsignaturaOfertadaParameters.AsignaturaOfertadaOrderColumn.PeriodoLectivo:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.PeriodoLectivo.Nombre)
                        : query.OrderByDescending(o => o.PeriodoLectivo.Nombre);
                    break;
                case SimpleListAsignaturaOfertadaParameters.AsignaturaOfertadaOrderColumn.PlanEstudio:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.PlanOfertado.Plan.Codigo + " - " + o.PlanOfertado.Plan.Nombre)
                        : query.OrderByDescending(o => o.PlanOfertado.Plan.Codigo + " - " + o.PlanOfertado.Plan.Nombre);
                    break;
                case SimpleListAsignaturaOfertadaParameters.AsignaturaOfertadaOrderColumn.PeriodoAcademico:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.PlanOfertado.PeriodoAcademico.Nombre)
                        : query.OrderByDescending(o => o.PlanOfertado.PeriodoAcademico.Nombre);
                    break;
                case SimpleListAsignaturaOfertadaParameters.AsignaturaOfertadaOrderColumn.TipoAsignatura:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.TipoAsignatura.Nombre)
                        : query.OrderByDescending(o => o.TipoAsignatura.Nombre);
                    break;
                case SimpleListAsignaturaOfertadaParameters.AsignaturaOfertadaOrderColumn.Creditos:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.AsignaturaPlan.Asignatura.Creditos)
                        : query.OrderByDescending(o => o.AsignaturaPlan.Asignatura.Creditos);
                    break;
                case SimpleListAsignaturaOfertadaParameters.AsignaturaOfertadaOrderColumn.DuracionPeriodoLectivo:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.PeriodoLectivo.DuracionPeriodoLectivo.Nombre)
                        : query.OrderByDescending(o => o.PeriodoLectivo.DuracionPeriodoLectivo.Nombre);
                    break;
                case SimpleListAsignaturaOfertadaParameters.AsignaturaOfertadaOrderColumn.UbicacionPeriodoLectivo:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.UbicacionPeriodoLectivo)
                        : query.OrderByDescending(o => o.UbicacionPeriodoLectivo);
                    break;
                case SimpleListAsignaturaOfertadaParameters.AsignaturaOfertadaOrderColumn.Curso:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.Curso.Numero)
                        : query.OrderByDescending(o => o.Curso.Numero);
                    break;
            }
          
            var listado = query.Select(ao => new AsignaturaOfertadaDto
            {
                Id = ao.Id,
                Codigo = ao.Codigo,
                Nombre = ao.Nombre,
                UbicacionPeriodoLectivo = ao.UbicacionPeriodoLectivo,
                Curso = ao.Curso != null ? new CursoDto
                {
                    Numero = ao.Curso.Numero
                } : null,
                PeriodoLectivo = new PeriodoLectivoDto
                {
                    Nombre = ao.PeriodoLectivo.Nombre,
                    FechaInicio = ao.PeriodoLectivo.FechaInicio,
                    FechaFin = ao.PeriodoLectivo.FechaFin,
                    DuracionPeriodoLectivo = new DuracionPeriodoLectivoDto
                    {
                        Nombre = ao.PeriodoLectivo.DuracionPeriodoLectivo.Nombre
                    }
                },
                PlanOfertado = new PlanOfertadoDto
                {
                    Id = ao.PlanOfertado.Id,
                    Plan = new PlanDto
                    {
                        Codigo = ao.PlanOfertado.Plan.Codigo,
                        Nombre = ao.PlanOfertado.Plan.Nombre,
                    },
                    PeriodoAcademico = new PeriodoAcademicoDto
                    {
                        Nombre = ao.PlanOfertado.PeriodoAcademico.Nombre,
                        FechaInicio = ao.PlanOfertado.PeriodoAcademico.FechaInicio,
                        FechaFin = ao.PlanOfertado.PeriodoAcademico.FechaFin
                    },
                },
                TipoAsignatura = new TipoAsignaturaDto
                {
                    Nombre = ao.TipoAsignatura.Nombre
                },
                AsignaturaPlan = new AsignaturaPlanDto
                {
                    Asignatura = new AsignaturaDto
                    {
                        Creditos = ao.AsignaturaPlan.Asignatura.Creditos,
                        Nombre = ao.AsignaturaPlan.Asignatura.Nombre,
                        Codigo = ao.AsignaturaPlan.Asignatura.Codigo
                    },
                }
            }).Skip((parameters.PageIndex - 1) * parameters.ItemsPerPage)
                .Take(parameters.ItemsPerPage)
                .ToList();

            result.Elements = listado;
            result.TotalElements = query.Count();
            result.PageCount = parameters.ItemsPerPage;
            return result;
        }
        public ResultList<PlanOfertadoDto> GetPagePlanOfertado(SearchPlanOfertadoParameters parameters)
        {
            var result = new ResultList<PlanOfertadoDto>();
            var query = _erpContext.PlanOfertado.AsQueryable();
            if (parameters.IdPlanOfertado.HasValue)
            {
                query = query.Where(po => po.Id == parameters.IdPlanOfertado.Value);
            }
            if (parameters.IdPeriodoAcademico.HasValue)
            {
                query =
                    query.Where(po => po.PeriodoAcademicoId == parameters.IdPeriodoAcademico.Value);
            }
            if (parameters.IdPlanEstudio.HasValue)
            {
                query =
                    query.Where(po => po.PlanId == parameters.IdPlanEstudio.Value);
            }

            var order = parameters.GetEnum(SearchPlanOfertadoParameters.PlanOfertadoOrderColumn.IdPlanOfertado);
            switch (order)
            {
                case SearchPlanOfertadoParameters.PlanOfertadoOrderColumn.IdPlanOfertado:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.Id)
                        : query.OrderByDescending(o => o.Id);
                    break;
                case SearchPlanOfertadoParameters.PlanOfertadoOrderColumn.PlanEstudio:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.Plan.Codigo + " - " + o.Plan.Nombre)
                        : query.OrderByDescending(o => o.Plan.Codigo + " - " + o.Plan.Nombre);
                    break;
                case SearchPlanOfertadoParameters.PlanOfertadoOrderColumn.PeriodoAcademico:
                    query = parameters.Ascendente
                       ? query.OrderBy(o => o.PeriodoAcademico.Nombre)
                       : query.OrderByDescending(o => o.PeriodoAcademico.Nombre);
                    break;
            }

            var listado = query.Select(a => new PlanOfertadoDto
            {
                Id = a.Id,
                Plan = new PlanDto
                {
                    Codigo = a.Plan.Codigo,
                    Nombre = a.Plan.Nombre
                },
                PeriodoAcademico = new PeriodoAcademicoDto
                {
                    Nombre = a.PeriodoAcademico.Nombre,
                    FechaInicio = a.PeriodoAcademico.FechaInicio,
                    FechaFin = a.PeriodoAcademico.FechaFin
                }
            }).Skip((parameters.PageIndex - 1) * parameters.ItemsPerPage)
                .Take(parameters.ItemsPerPage)
                .ToList();
            result.Elements = listado;
            result.TotalElements = query.Count();
            result.PageCount = parameters.ItemsPerPage;
            return result;
        }
    }
}