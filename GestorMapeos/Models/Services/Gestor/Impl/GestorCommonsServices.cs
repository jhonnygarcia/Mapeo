using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using GestorMapeos.Models.Dto.Gestor;
using GestorMapeos.Models.Model.Gestor;
using GestorMapeos.Parameters;
using GestorMapeos.Parameters.AsignaturaOfertadaIntegracion;
using GestorMapeos.Parameters.GestorCommons;
using GestorMapeos.Parameters.PeriodoEstudioIntegracion;
using GestorMapeos.Parameters.PlantillaEstudioIntegracion;
using GestorMapeos.TransferStructs;

namespace GestorMapeos.Models.Services.Gestor.Impl
{
    public class GestorCommonsServices : IGestorCommonsServices
    {
        private readonly GestorContext _gestorContext;
        public GestorCommonsServices(GestorContext gestorContext)
        {
            _gestorContext = gestorContext;
        }

        public ResultList<PlantillaEstudioDto> SimpleSearchPlantillaEstudio(SimpleListViewModel parameters)
        {
            var result = new ResultList<PlantillaEstudioDto>();
            var query = _gestorContext.PlantillaEstudio.AsQueryable();
            if (!string.IsNullOrEmpty(parameters.Descripcion))
                query = query.Where(p => (p.Nombre + " - " + p.AnyoPlan).Contains(parameters.Descripcion));

            query = query.OrderBy(a => a.Nombre + " - " + a.AnyoPlan);
            var listado = query.Select(a => new PlantillaEstudioDto
            {
                Id = a.Id,
                Nombre = a.Nombre,
                AnyoPlan = a.AnyoPlan,
                TipoEstudio = new TipoEstudioUnirDto
                {
                    Nombre = a.TipoEstudio.Nombre
                },
                Rama = a.Rama != null ? new RamaUnirDto
                {
                    Nombre = a.Nombre
                } : null
            }).Skip((parameters.PageIndex - 1) * parameters.ItemsPerPage)
                .Take(parameters.ItemsPerPage)
                .ToList();

            result.Elements = listado;
            result.TotalElements = query.Count();
            result.PageCount = parameters.ItemsPerPage;

            return result;
        }

        public ResultList<PeriodoMatriculacionUnirDto> SimpleSearchPeriodoMatriculacion(SimpleListViewModel parameters)
        {
            var result = new ResultList<PeriodoMatriculacionUnirDto>();
            var query = _gestorContext.PeriodosMatriculacionesUnir.Where(p => p.Borrado.Equals("No"));
            if (!string.IsNullOrEmpty(parameters.Descripcion))
                query = query.Where(a => a.Nombre.Contains(parameters.Descripcion));

            query = query.OrderBy(a => a.Nombre);

            var listado = query.Select(a => new PeriodoMatriculacionUnirDto
            {
                Id = a.Id,
                Nombre = a.Nombre,
                AnyoAcademico = a.AnyoAcademico,
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

        public ResultList<PlantillaAsignaturaDto> SimpleSearchPlantillaAsignatura(SimpleListPlantillaAsignaturaParameters parameters)
        {
            var result = new ResultList<PlantillaAsignaturaDto>();
            var query = _gestorContext.PlantillaAsignatura.Where(pa => true);
            if (parameters.IdPlantillaEstudio.HasValue)
                query = query.Where(pa => pa.PlantillaEstudioId == parameters.IdPlantillaEstudio.Value);
            if (!string.IsNullOrEmpty(parameters.Descripcion))
                query = query.Where(pa => pa.NombreAsignatura.Contains(parameters.Descripcion));

            query = query.OrderBy(a => a.NombreAsignatura);

            var listado = query.Select(a => new PlantillaAsignaturaDto
            {
                Id = a.Id,
                NombreAsignatura = a.NombreAsignatura,
                Codigo = a.Codigo,
                TipoAsignatura = new TipoAsignaturaUnirDto
                {
                    Nombre = a.TipoAsignatura.Nombre
                },
                Creditos = a.Creditos,
                PlantillaEstudio = new PlantillaEstudioDto
                {
                    Nombre = a.PlantillaEstudio.Nombre,
                    AnyoPlan = a.PlantillaEstudio.AnyoPlan
                }
            }).Skip((parameters.PageIndex - 1) * parameters.ItemsPerPage)
                .Take(parameters.ItemsPerPage)
                .ToList();

            result.Elements = listado;
            result.TotalElements = query.Count();
            result.PageCount = parameters.ItemsPerPage;

            return result;
        }

        public ResultList<AsignaturaUnirDto> SimpleSearchAsignatura(SimpleListAsignaturaParameters parameters)
        {
            var result = new ResultList<AsignaturaUnirDto>();
            var query = _gestorContext.AsignaturaUnir.Where(pa => pa.Borrado.Equals("No"));
            if (parameters.IdPeriodoMatriculacion.HasValue)
                query = query.Where(a => a.EstudioUnir.Borrado.Equals("No") &&
                    a.EstudioUnir.PeriodosEstudioUnir.Any(p => p.Borrado.Equals("No") &&
                                                               p.PeriodoMatriculacionId ==
                                                               parameters.IdPeriodoMatriculacion.Value));
            if (parameters.IdEstudio.HasValue)
                query = query.Where(a => a.Borrado.Equals("No") && a.EstudioUnirId == parameters.IdEstudio.Value);
            if (!string.IsNullOrEmpty(parameters.Descripcion))
                query = query.Where(a => a.Nombre.Contains(parameters.Descripcion));

            query = query.OrderBy(a => a.Nombre);

            var listado = query.Select(a => new AsignaturaUnirDto
            {
                Id = a.Id,
                Nombre = a.Nombre,
                TipoAsignatura = a.TipoAsignatura,
                Creditos = a.Creditos,
                PeriodoLectivo = a.PeriodoLectivo,
                Curso = a.Curso,
                Activo = a.Activo,
                EstudioUnir = new EstudioUnirDto
                {
                    Nombre = a.EstudioUnir.Nombre
                }
            }).Skip((parameters.PageIndex - 1) * parameters.ItemsPerPage)
                .Take(parameters.ItemsPerPage)
                .ToList();
            result.Elements = listado;
            result.TotalElements = query.Count();
            result.PageCount = parameters.ItemsPerPage;

            return result;
        }

        public ResultList<EstudioUnirDto> SimpleSearchEstudio(SimpleListEstudioParameters parameters)
        {
            var result = new ResultList<EstudioUnirDto>();
            var query = _gestorContext.EstudioUnir.Where(p => p.Borrado.Equals("No"));
            if (parameters.IdPeriodoMatriculacion.HasValue)
                query = query.Where(e => e.PeriodosEstudioUnir.Any(pe => pe.Borrado.Equals("No")
                            && pe.PeriodoMatriculacionUnir.Borrado.Equals("No")
                            && pe.PeriodoMatriculacionId == parameters.IdPeriodoMatriculacion.Value));
            if (!string.IsNullOrEmpty(parameters.Descripcion))
                query = query.Where(e => e.Nombre.Contains(parameters.Descripcion));

            query = query.OrderBy(a => a.Nombre);

            var listado = query.Select(a => new EstudioUnirDto
            {
                Id = a.Id,
                Nombre = a.Nombre,
                PlanEstudio = a.PlanEstudio,
                Activo = a.Activo,
                TipoEstudio = new TipoEstudioSegunUnirDto
                {
                    Nombre = a.TipoEstudioSegunUnir.Nombre
                },
                RamaEstudio = a.RamaEstudio,
                Titulo = a.EstudioPrincipalUnir != null ? new EstudioPrincipalUnirDto
                {
                    Id = a.EstudioPrincipalUnir.Id,
                    Codigo = a.EstudioPrincipalUnir.Codigo,
                    Nombre = a.EstudioPrincipalUnir.Nombre
                } : null
            }).Skip((parameters.PageIndex - 1) * parameters.ItemsPerPage)
                .Take(parameters.ItemsPerPage)
                .ToList();

            result.Elements = listado;
            result.TotalElements = query.Count();
            result.PageCount = parameters.ItemsPerPage;

            return result;
        }

        public ResultList<PeriodoEstudioUnirDto> GetPagedPeriodoEstudio(SearchPeriodoEstudioParameters parameters)
        {
            var result = new ResultList<PeriodoEstudioUnirDto>();
            var query = _gestorContext.PeriodoEstudioUnir.Where(pe => pe.Borrado.Equals("No")
                       && pe.PeriodoMatriculacionUnir.Borrado.Equals("No")
                       && pe.EstudioUnir.Borrado.Equals("No"));
            if (parameters.IdPeriodoEstudio.HasValue)
                query = query.Where(pe => pe.Id == parameters.IdPeriodoEstudio.Value);
            if (parameters.IdPeriodoMatriculacion.HasValue)
                query = query.Where(pe => pe.PeriodoMatriculacionId == parameters.IdPeriodoMatriculacion.Value);
            if (parameters.IdEstudio.HasValue)
                query = query.Where(pe => pe.EstudioId == parameters.IdEstudio.Value);

            var order = parameters.GetEnum(
                    SearchPeriodoEstudioParameters.PeriodoEstudioOrderColumn.IdPeriodoEstudio);
            switch (order)
            {
                case SearchPeriodoEstudioParameters.PeriodoEstudioOrderColumn.IdPeriodoEstudio:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.Id)
                        : query.OrderByDescending(o => o.Id);
                    break;
                case SearchPeriodoEstudioParameters.PeriodoEstudioOrderColumn.Estudio:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.EstudioUnir.Nombre)
                        : query.OrderByDescending(o => o.EstudioUnir.Nombre);
                    break;
                case SearchPeriodoEstudioParameters.PeriodoEstudioOrderColumn.PeriodoMatriculacion
                    :
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.PeriodoMatriculacionUnir.Nombre)
                        : query.OrderByDescending(o => o.PeriodoMatriculacionUnir.Nombre);
                    break;
            }

            var listado = query.Select(a => new PeriodoEstudioUnirDto
            {
                Id = a.Id,
                EstudioUnir = new EstudioUnirDto
                {
                    Nombre = a.EstudioUnir.Nombre
                },
                PeriodoMatriculacionUnir = new PeriodoMatriculacionUnirDto
                {
                    Nombre = a.PeriodoMatriculacionUnir.Nombre
                }
            }).Skip((parameters.PageIndex - 1) * parameters.ItemsPerPage)
                .Take(parameters.ItemsPerPage)
                .ToList();
            result.Elements = listado;
            result.TotalElements = query.Count();
            result.PageCount = parameters.ItemsPerPage;
            return result;
        }

        public ResultList<PeriodoEstudioAsignaturaUnirDto> SearchPeriodoEstudioAsignatura(SearchPeriodoEstudioAsignaturaParameters parameters)
        {
            var result = new ResultList<PeriodoEstudioAsignaturaUnirDto>();
            var query = _gestorContext.PeriodoEstudioAsignaturaUnir.Where(pe => pe.Borrado.Equals(false)
                                                                        && pe.PeriodoEstudioUnir.Borrado.Equals("No")
                                                                        && pe.AsignaturaUnir.Borrado.Equals("No"));
            if (parameters.IdPeriodoEstudioAsignatura.HasValue)
                query = query.Where(pe => pe.Id == parameters.IdPeriodoEstudioAsignatura.Value);
            if (parameters.IdPeriodoMatriculacion.HasValue)
                query = query.Where(pe => pe.PeriodoEstudioUnir.PeriodoMatriculacionUnir.Id == parameters.IdPeriodoMatriculacion.Value);
            if (parameters.IdEstudio.HasValue)
                query = query.Where(pe => pe.PeriodoEstudioUnir.EstudioUnir.Id == parameters.IdEstudio.Value);
            if (parameters.IdAsignatura.HasValue)
                query = query.Where(pe => pe.AsignaturaUnir.Id == parameters.IdAsignatura.Value);


            var order =
                parameters.GetEnum(
                    SearchPeriodoEstudioAsignaturaParameters.PeriodoEstudioAsignaturaOrderColumn
                        .IdPeriodoEstudioAsignatura);
            switch (order)
            {
                case SearchPeriodoEstudioAsignaturaParameters.PeriodoEstudioAsignaturaOrderColumn.IdPeriodoEstudioAsignatura:
                    query = parameters.Ascendente
                       ? query.OrderBy(o => o.Id)
                       : query.OrderByDescending(o => o.Id);
                    break;
                case SearchPeriodoEstudioAsignaturaParameters.PeriodoEstudioAsignaturaOrderColumn.Asignatura:
                    query = parameters.Ascendente
                      ? query.OrderBy(o => o.AsignaturaUnir.Nombre)
                      : query.OrderByDescending(o => o.AsignaturaUnir.Nombre);
                    break;
                case SearchPeriodoEstudioAsignaturaParameters.PeriodoEstudioAsignaturaOrderColumn.Estudio:
                    query = parameters.Ascendente
                      ? query.OrderBy(o => o.AsignaturaUnir.EstudioUnir.Nombre)
                      : query.OrderByDescending(o => o.AsignaturaUnir.EstudioUnir.Nombre);
                    break;
                case SearchPeriodoEstudioAsignaturaParameters.PeriodoEstudioAsignaturaOrderColumn.PeriodoMatriculacion:
                    query = parameters.Ascendente
                      ? query.OrderBy(o => o.PeriodoEstudioUnir.PeriodoMatriculacionUnir.Nombre)
                      : query.OrderByDescending(o => o.PeriodoEstudioUnir.PeriodoMatriculacionUnir.Nombre);
                    break;
                case SearchPeriodoEstudioAsignaturaParameters.PeriodoEstudioAsignaturaOrderColumn.TipoAsignatura:
                    query = parameters.Ascendente
                      ? query.OrderBy(o => o.AsignaturaUnir.TipoAsignatura)
                      : query.OrderByDescending(o => o.AsignaturaUnir.TipoAsignatura);
                    break;
                case SearchPeriodoEstudioAsignaturaParameters.PeriodoEstudioAsignaturaOrderColumn.Creditos:
                    query = parameters.Ascendente
                      ? query.OrderBy(o => o.AsignaturaUnir.Creditos)
                      : query.OrderByDescending(o => o.AsignaturaUnir.Creditos);
                    break;
                case SearchPeriodoEstudioAsignaturaParameters.PeriodoEstudioAsignaturaOrderColumn.PeriodoLectivo:
                    query = parameters.Ascendente
                      ? query.OrderBy(o => o.AsignaturaUnir.PeriodoLectivo)
                      : query.OrderByDescending(o => o.AsignaturaUnir.PeriodoLectivo);
                    break;
                case SearchPeriodoEstudioAsignaturaParameters.PeriodoEstudioAsignaturaOrderColumn.Curso:
                    query = parameters.Ascendente
                      ? query.OrderBy(o => o.AsignaturaUnir.Curso)
                      : query.OrderByDescending(o => o.AsignaturaUnir.Curso);
                    break;
                case SearchPeriodoEstudioAsignaturaParameters.PeriodoEstudioAsignaturaOrderColumn.Activa:
                    query = parameters.Ascendente
                      ? query.OrderBy(o => o.AsignaturaUnir.Activo)
                      : query.OrderByDescending(o => o.AsignaturaUnir.Activo);
                    break;
            }

            var listado = query.Select(a => new PeriodoEstudioAsignaturaUnirDto
            {
                Id = a.Id,
                AsignaturaUnir = new AsignaturaUnirDto
                {
                    Id = a.Id,
                    Nombre = a.AsignaturaUnir.Nombre,
                    TipoAsignatura = a.AsignaturaUnir.TipoAsignatura,
                    Creditos = a.AsignaturaUnir.Creditos,
                    PeriodoLectivo = a.AsignaturaUnir.PeriodoLectivo,
                    Curso = a.AsignaturaUnir.Curso,
                    Activo = a.AsignaturaUnir.Activo,
                    EstudioUnir = new EstudioUnirDto
                    {
                        Id = a.AsignaturaUnir.EstudioUnir.Id,
                        Nombre = a.AsignaturaUnir.EstudioUnir.Nombre
                    }
                },
                PeriodoEstudioUnir = new PeriodoEstudioUnirDto
                {
                    Id = a.PeriodoEstudioUnir.Id,
                    PeriodoMatriculacionUnir = new PeriodoMatriculacionUnirDto
                    {
                        Id = a.PeriodoEstudioUnir.PeriodoMatriculacionUnir.Id,
                        Nombre = a.PeriodoEstudioUnir.PeriodoMatriculacionUnir.Nombre
                    }
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