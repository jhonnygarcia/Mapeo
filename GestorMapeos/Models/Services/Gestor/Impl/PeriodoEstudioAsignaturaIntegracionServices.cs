using System.Linq;
using GestorMapeos.Parameters.PeriodoEstudioAsignaturaIntegracion;
using GestorMapeos.Globalization.Services;
using GestorMapeos.Models.Dto.Erp20;
using GestorMapeos.Models.Dto.Gestor;
using GestorMapeos.Models.Model.Erp20;
using GestorMapeos.Models.Model.Gestor;
using GestorMapeos.Models.Model.Gestor.Entities;
using GestorMapeos.Parameters.AsignaturaOfertadaIntegracion;
using GestorMapeos.TransferStructs;

namespace GestorMapeos.Models.Services.Gestor.Impl
{
    public class PeriodoEstudioAsignaturaIntegracionServices : IPeriodoEstudioAsignaturaIntegracionServices
    {
        private readonly GestorContext _gestorContext;
        private readonly ErpContext _erpContext;
        public PeriodoEstudioAsignaturaIntegracionServices(GestorContext gestorContext, ErpContext erpContext)
        {
            _gestorContext = gestorContext;
            _erpContext = erpContext;
        }

        public BaseResult Crear(SavePeriodoEstudioAsignaturaIntegracionParameters model)
        {
            var result = new BaseResult();
            //Validar Referencias
            var periodoEstudioAsignaturaUnir = _gestorContext.PeriodoEstudioAsignaturaUnir.Find(model.IdPeriodoEstudioAsignatura);
            if (periodoEstudioAsignaturaUnir == null)
                result.Errors.Add(PeriodoEstudioAsignaturaIntegracionStrings.ErrorPeriodoEstudioAsignaturaUnirNoExiste);

            var asignaturaOfertada = _erpContext.AsignaturaOfertada.Find(model.IdAsignaturaOfertada);
            if (asignaturaOfertada == null)
                result.Errors.Add(PeriodoEstudioAsignaturaIntegracionStrings.ErrorAsignaturaOfertadaErpNoExiste);

            var persisted = _gestorContext.PeriodoEstudioAsignaturaIntegracion.Find(model.IdPeriodoEstudioAsignatura);
            if (persisted != null)  //Validacion 5
                result.Errors.Add(PeriodoEstudioAsignaturaIntegracionStrings.ErrorPeriodoEstudioAsignaturaYaMapeada);

            if (result.HasErrors)
                return result;

            //Validacion 6
            if (!_gestorContext.AsignaturaIntegracion.Any(ae => ae.Id == periodoEstudioAsignaturaUnir.AsignaturaId && ae.AsignaturaPlanIntegracionId == asignaturaOfertada.AsignaturaPlanId))
                result.Errors.Add(PeriodoEstudioAsignaturaIntegracionStrings.ErrorAsignaturaEstudioNoMapeada);

            //Validacion 7
            var periodoEstudioIntegracion = _gestorContext.PeriodoEstudioIntegracion.Find(periodoEstudioAsignaturaUnir.PeriodoEstudioId);
            if (periodoEstudioIntegracion == null || periodoEstudioIntegracion.PlanOfertadoId != asignaturaOfertada.PlanOfertadoId)
                result.Errors.Add(PeriodoEstudioAsignaturaIntegracionStrings.ErrorPeriodoEstudioNoMapeado);

            if (result.HasErrors)
                return result;
            var periodoEstudioAsignaturaIntegracion = new PeriodoEstudioAsignaturaIntegracion
            {
                Id = periodoEstudioAsignaturaUnir.Id,
                AsignaturaOfertadaId = asignaturaOfertada.Id
            };
            _gestorContext.PeriodoEstudioAsignaturaIntegracion.Add(periodoEstudioAsignaturaIntegracion);
            _gestorContext.SaveChanges();
            return result;
        }

        public BaseResult Modificar(SavePeriodoEstudioAsignaturaIntegracionParameters model)
        {
            var result = new BaseResult();
            var persisted = _gestorContext.PeriodoEstudioAsignaturaIntegracion.Find(model.IdPeriodoEstudioAsignatura);
            var periodoEstudioAsignaturaUnir = _gestorContext.PeriodoEstudioAsignaturaUnir.Find(model.IdPeriodoEstudioAsignatura);
            var asignaturaOfertada = _erpContext.AsignaturaOfertada.Find(model.IdAsignaturaOfertada);
            if (persisted == null)
                result.Errors.Add(PeriodoEstudioAsignaturaIntegracionStrings.ErrorPeriodoEstudioAsignaturaNoExiste);
            if (periodoEstudioAsignaturaUnir == null)
                result.Errors.Add(PeriodoEstudioAsignaturaIntegracionStrings.ErrorPeriodoEstudioAsignaturaUnirNoExiste);
            if (asignaturaOfertada == null)
                result.Errors.Add(PeriodoEstudioAsignaturaIntegracionStrings.ErrorAsignaturaOfertadaErpNoExiste);
            if (result.HasErrors)
                return result;

            if (_gestorContext.PeriodoEstudioAsignaturaIntegracion.Any(pea => pea.Id == model.IdPeriodoEstudioAsignatura && pea.AsignaturaOfertadaId != model.IdAsignaturaOfertada))
                result.Errors.Add(PeriodoEstudioAsignaturaIntegracionStrings.ErrorPeriodoEstudioAsignaturaYaMapeada);
            if (!_gestorContext.AsignaturaIntegracion.Any(ae => ae.Id == periodoEstudioAsignaturaUnir.AsignaturaId && ae.AsignaturaPlanIntegracionId == asignaturaOfertada.AsignaturaPlanId))
                result.Errors.Add(PeriodoEstudioAsignaturaIntegracionStrings.ErrorAsignaturaEstudioNoMapeada);
            var periodoEstudioIntegracion = _gestorContext.PeriodoEstudioIntegracion.Find(periodoEstudioAsignaturaUnir.PeriodoEstudioId);
            if (periodoEstudioIntegracion == null || periodoEstudioIntegracion.PlanOfertadoId != asignaturaOfertada.PlanOfertadoId)
                result.Errors.Add(PeriodoEstudioAsignaturaIntegracionStrings.ErrorPeriodoEstudioNoMapeado);
            if (result.HasErrors)
                return result;

            persisted.Id = model.IdPeriodoEstudioAsignatura;
            persisted.AsignaturaOfertadaId = model.IdAsignaturaOfertada;
            _gestorContext.SaveChanges();

            return result;
        }

        public BaseResult Eliminar(int[] ids)
        {
            var result = new BaseResult();
            if (ids == null)
            {
                result.Errors.Add("Error de datos: los datos enviados no son válidos");
                return result;
            }
            var periodosEstudiosAsignaturas = _gestorContext.PeriodoEstudioAsignaturaIntegracion.Where(a => ids.Contains(a.Id)).ToList();
            if (!periodosEstudiosAsignaturas.Any())
            {
                result.Errors.Add("Error de datos: Los datos proporcionados no son validos");
                return result;
            }
            foreach (var periodoEstudioAsignaturaIntegracion in periodosEstudiosAsignaturas)
            {
                _gestorContext.PeriodoEstudioAsignaturaIntegracion.Remove(periodoEstudioAsignaturaIntegracion);
            }
            _gestorContext.SaveChanges();
            return result;
        }

        public ResultList<PeriodoEstudioAsignaturaIntegracionDto> GetPagedPeriodoEstudioAsignatura(SearchPeriodoEstudioAsignasturaIntegracionParameters parameters)
        {
            var result = new ResultList<PeriodoEstudioAsignaturaIntegracionDto>();
            var query = _gestorContext.PeriodoEstudioAsignaturaIntegracion.Where(a => true);

            if (parameters.IdPeriodoEstudioAsignatura.HasValue)
                query = query.Where(a => a.Id == parameters.IdPeriodoEstudioAsignatura.Value);
            if (parameters.IdPeriodoMatriculacion.HasValue)
                query = query.Where(a => a.PeriodoEstudioAsignaturaUnir.PeriodoEstudioUnir.PeriodoMatriculacionId
                                         == parameters.IdPeriodoMatriculacion.Value);
            if (parameters.IdEstudio.HasValue)
                query = query.Where(a => a.PeriodoEstudioAsignaturaUnir.AsignaturaUnir.EstudioUnirId
                                                 == parameters.IdEstudio.Value);
            if (parameters.IdAsignatura.HasValue)
                query = query.Where(a => a.PeriodoEstudioAsignaturaUnir.AsignaturaId == parameters.IdAsignatura.Value);

            if (parameters.IdAsignaturaOfertada.HasValue)
                query = query.Where(a => a.AsignaturaOfertadaId == parameters.IdAsignaturaOfertada.Value);

            if (parameters.IdPeriodoAcademico.HasValue)
                query = query.Where(a => a.PeriodoEstudioAsignaturaUnir
                    .PeriodoEstudioUnir.PeriodoMatriculacionUnir.PeriodoMatriculacionIntegracion.PeriodoAcademicoId ==
                                                 parameters.IdPeriodoAcademico.Value);
            if (parameters.IdPlanEstudio.HasValue)
                query = query.Where(a => a.PeriodoEstudioAsignaturaUnir
                    .PeriodoEstudioUnir.PeriodoEstudioIntegracion.PlantillaEstudioIntegracionId == parameters.IdPlanEstudio.Value);
            if (parameters.FilterIdAsignaturaOfertada.HasValue)
                query = query.Where(a => a.AsignaturaOfertadaId == parameters.FilterIdAsignaturaOfertada.Value);


            var order = parameters.GetEnum(SearchPeriodoEstudioAsignasturaIntegracionParameters.AsignaturaOfertadaIntegracionOrderColumn.Id);
            switch (order)
            {
                case SearchPeriodoEstudioAsignasturaIntegracionParameters.AsignaturaOfertadaIntegracionOrderColumn.Id:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.Id)
                        : query.OrderByDescending(o => o.Id);
                    break;
                case SearchPeriodoEstudioAsignasturaIntegracionParameters.AsignaturaOfertadaIntegracionOrderColumn.Asignatura:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.PeriodoEstudioAsignaturaUnir.AsignaturaUnir.Nombre)
                        : query.OrderByDescending(o => o.PeriodoEstudioAsignaturaUnir.AsignaturaUnir.Nombre);
                    break;
                case SearchPeriodoEstudioAsignasturaIntegracionParameters.AsignaturaOfertadaIntegracionOrderColumn.Estudio:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.PeriodoEstudioAsignaturaUnir.AsignaturaUnir.EstudioUnir.Nombre)
                        : query.OrderByDescending(o => o.PeriodoEstudioAsignaturaUnir.AsignaturaUnir.EstudioUnir.Nombre);
                    break;
                case SearchPeriodoEstudioAsignasturaIntegracionParameters.AsignaturaOfertadaIntegracionOrderColumn.IdAsignaturaOfertada:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.AsignaturaOfertadaId)
                        : query.OrderByDescending(o => o.AsignaturaOfertadaId);
                    break;
                case SearchPeriodoEstudioAsignasturaIntegracionParameters.AsignaturaOfertadaIntegracionOrderColumn.PeriodoMatriculacion:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.PeriodoEstudioAsignaturaUnir.PeriodoEstudioUnir.PeriodoMatriculacionUnir.Nombre)
                        : query.OrderByDescending(o => o.PeriodoEstudioAsignaturaUnir.PeriodoEstudioUnir.PeriodoMatriculacionUnir.Nombre);
                    break;
            }

            var listado = query.Select(a => new PeriodoEstudioAsignaturaIntegracionDto
            {
                Id = a.Id,
                PeriodoEstudioAsignaturaUnir = new PeriodoEstudioAsignaturaUnirDto
                {
                    Id = a.PeriodoEstudioAsignaturaUnir.Id,
                    AsignaturaUnir = new AsignaturaUnirDto
                    {
                        Nombre = a.PeriodoEstudioAsignaturaUnir.AsignaturaUnir.Nombre,
                        EstudioUnir = new EstudioUnirDto
                        {
                            Nombre = a.PeriodoEstudioAsignaturaUnir.AsignaturaUnir.EstudioUnir.Nombre
                        }
                    },
                    PeriodoEstudioUnir = new PeriodoEstudioUnirDto
                    {
                        PeriodoMatriculacionUnir = new PeriodoMatriculacionUnirDto
                        {
                            Nombre = a.PeriodoEstudioAsignaturaUnir.PeriodoEstudioUnir.PeriodoMatriculacionUnir.Nombre
                        }
                    },
                },
                AsignaturaOfertadaId = a.AsignaturaOfertadaId
            }).Skip((parameters.PageIndex - 1) * parameters.ItemsPerPage)
                .Take(parameters.ItemsPerPage)
                .ToList();

            /*Obtener valores otros del ERP-20*/
            var ids = listado.Select(a => a.AsignaturaOfertadaId).ToArray();
            var asignaturasOfertadas = _erpContext.AsignaturaOfertada.Where(p => ids.Contains(p.Id)).Select(a => new
            {
                a.Id,
                a.Codigo,
                a.Nombre,
                PeriodoLectivo = new
                {
                    a.PeriodoLectivo.Nombre,
                    a.PeriodoLectivo.FechaInicio,
                    a.PeriodoLectivo.FechaFin
                },
                Plan = new
                {
                    a.PlanOfertado.Plan.Codigo,
                    a.PlanOfertado.Plan.Nombre
                },
                PeriodoAcademico = new
                {
                    a.PlanOfertado.PeriodoAcademico.Nombre,
                    a.PlanOfertado.PeriodoAcademico.FechaInicio,
                    a.PlanOfertado.PeriodoAcademico.FechaFin
                }
            }).ToList();

            listado.ForEach((dto) =>
            {
                var asignaturaOfertada = asignaturasOfertadas.FirstOrDefault(a => a.Id == dto.AsignaturaOfertadaId);
                if (asignaturaOfertada != null)
                {
                    dto.AsignaturaOfertada = new AsignaturaOfertadaDto
                    {
                        Id = asignaturaOfertada.Id,
                        Codigo = asignaturaOfertada.Codigo,
                        Nombre = asignaturaOfertada.Nombre,
                        PeriodoLectivo = new PeriodoLectivoDto
                        {
                            Nombre = asignaturaOfertada.PeriodoLectivo.Nombre,
                            FechaInicio = asignaturaOfertada.PeriodoLectivo.FechaInicio,
                            FechaFin = asignaturaOfertada.PeriodoLectivo.FechaFin
                        },
                        PlanOfertado = new PlanOfertadoDto
                        {
                            Plan = new PlanDto
                            {
                                Codigo = asignaturaOfertada.Plan.Codigo,
                                Nombre = asignaturaOfertada.Plan.Nombre
                            },
                            PeriodoAcademico = new PeriodoAcademicoDto
                            {
                                Nombre = asignaturaOfertada.PeriodoAcademico.Nombre,
                                FechaInicio = asignaturaOfertada.PeriodoAcademico.FechaInicio,
                                FechaFin = asignaturaOfertada.PeriodoAcademico.FechaFin
                            }
                        }
                    };
                }
            });

            result.Elements = listado;
            result.TotalElements = query.Count();
            result.PageCount = parameters.ItemsPerPage;
            return result;
        }

        public ResultValue<PeriodoEstudioAsignaturaIntegracionDto> Get(int id)
        {
            var result = new ResultValue<PeriodoEstudioAsignaturaIntegracionDto>();
            var periodoEstudioAsignaturaIntegracion = _gestorContext.PeriodoEstudioAsignaturaIntegracion.Find(id);
            if (periodoEstudioAsignaturaIntegracion == null)
                result.Errors.Add(PeriodoEstudioAsignaturaIntegracionStrings.ErrorDatosGestor);
            if (result.HasErrors)
                return result;

            var asignaturaOfertada = _erpContext.AsignaturaOfertada.Find(periodoEstudioAsignaturaIntegracion.AsignaturaOfertadaId);
            if (asignaturaOfertada == null)
                result.Errors.Add(PeriodoEstudioAsignaturaIntegracionStrings.ErrorDatosErp);
            if (result.HasErrors)
            {
                result.Type = ResultType.ElementNotFound;
                return result;
            }

            var element = new PeriodoEstudioAsignaturaIntegracionDto
            {
                Id = periodoEstudioAsignaturaIntegracion.Id,
                PeriodoEstudioAsignaturaUnir = new PeriodoEstudioAsignaturaUnirDto
                {
                    Id = periodoEstudioAsignaturaIntegracion.PeriodoEstudioAsignaturaUnir.Id,
                    AsignaturaUnir = new AsignaturaUnirDto
                    {
                        Nombre = periodoEstudioAsignaturaIntegracion.PeriodoEstudioAsignaturaUnir.AsignaturaUnir.Nombre,
                        TipoAsignatura = periodoEstudioAsignaturaIntegracion.PeriodoEstudioAsignaturaUnir.AsignaturaUnir.TipoAsignatura,
                        Creditos = periodoEstudioAsignaturaIntegracion.PeriodoEstudioAsignaturaUnir.AsignaturaUnir.Creditos,
                        PeriodoLectivo = periodoEstudioAsignaturaIntegracion.PeriodoEstudioAsignaturaUnir.AsignaturaUnir.PeriodoLectivo,
                        Curso = periodoEstudioAsignaturaIntegracion.PeriodoEstudioAsignaturaUnir.AsignaturaUnir.Curso,
                        Activo = periodoEstudioAsignaturaIntegracion.PeriodoEstudioAsignaturaUnir.AsignaturaUnir.Activo
                    },
                    PeriodoEstudioUnir = new PeriodoEstudioUnirDto
                    {
                        Id = periodoEstudioAsignaturaIntegracion.PeriodoEstudioAsignaturaUnir.PeriodoEstudioId,
                        EstudioUnir = new EstudioUnirDto
                        {
                            Nombre = periodoEstudioAsignaturaIntegracion.PeriodoEstudioAsignaturaUnir.PeriodoEstudioUnir.EstudioUnir.Nombre
                        },
                        PeriodoMatriculacionUnir = new PeriodoMatriculacionUnirDto
                        {
                            Nombre = periodoEstudioAsignaturaIntegracion.PeriodoEstudioAsignaturaUnir.PeriodoEstudioUnir.PeriodoMatriculacionUnir.Nombre
                        }
                    }
                },
                AsignaturaOfertada = new AsignaturaOfertadaDto
                {
                    Id = asignaturaOfertada.Id,
                    Codigo = asignaturaOfertada.Codigo,
                    Nombre = asignaturaOfertada.Nombre,
                    UbicacionPeriodoLectivo = asignaturaOfertada.UbicacionPeriodoLectivo,
                    Curso = asignaturaOfertada.CursoId.HasValue ? new CursoDto
                    {
                        Numero = asignaturaOfertada.Curso.Numero
                    } : null,
                    TipoAsignatura = new TipoAsignaturaDto
                    {
                        Nombre = asignaturaOfertada.TipoAsignatura.Nombre
                    },
                    AsignaturaPlan = new AsignaturaPlanDto
                    {
                        Asignatura = new AsignaturaDto
                        {
                            Nombre = asignaturaOfertada.AsignaturaPlan.Asignatura.Nombre,
                            Creditos = asignaturaOfertada.AsignaturaPlan.Asignatura.Creditos,
                            Codigo = asignaturaOfertada.AsignaturaPlan.Asignatura.Codigo,
                        }
                    },
                    PeriodoLectivo = new PeriodoLectivoDto
                    {
                        Nombre = asignaturaOfertada.PeriodoLectivo.Nombre,
                        FechaInicio = asignaturaOfertada.PeriodoLectivo.FechaInicio,
                        FechaFin = asignaturaOfertada.PeriodoLectivo.FechaFin,
                        DuracionPeriodoLectivo = new DuracionPeriodoLectivoDto
                        {
                            Nombre = asignaturaOfertada.PeriodoLectivo.DuracionPeriodoLectivo.Nombre
                        }
                    },
                    PlanOfertado = new PlanOfertadoDto
                    {
                        Id = asignaturaOfertada.PlanOfertadoId,
                        Plan = new PlanDto
                        {
                            Codigo = asignaturaOfertada.PlanOfertado.Plan.Codigo,
                            Nombre = asignaturaOfertada.PlanOfertado.Plan.Nombre
                        },
                        PeriodoAcademico = new PeriodoAcademicoDto
                        {
                            Nombre = asignaturaOfertada.PlanOfertado.PeriodoAcademico.Nombre,
                            FechaInicio = asignaturaOfertada.PlanOfertado.PeriodoAcademico.FechaInicio,
                            FechaFin = asignaturaOfertada.PlanOfertado.PeriodoAcademico.FechaInicio

                        }
                    }
                }
            };
            result.Value = element;
            return result;
        }
    }
}