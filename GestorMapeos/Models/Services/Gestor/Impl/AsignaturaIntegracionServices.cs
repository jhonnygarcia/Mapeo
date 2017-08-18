using System.Linq;
using GestorMapeos.Parameters.AsignaturaIntegracion;
using GestorMapeos.Globalization.Services;
using GestorMapeos.Models.Dto.Erp20;
using GestorMapeos.Models.Dto.Gestor;
using GestorMapeos.Models.Model.Erp20;
using GestorMapeos.Models.Model.Gestor;
using GestorMapeos.Models.Model.Gestor.Entities;
using GestorMapeos.TransferStructs;

namespace GestorMapeos.Models.Services.Gestor.Impl
{
    public class AsignaturaIntegracionServices : IAsignaturaIntegracionServices
    {
        private readonly GestorContext _gestorContext;
        private readonly ErpContext _erpContext;
        public AsignaturaIntegracionServices(GestorContext gestorContext, ErpContext erpContext)
        {
            _gestorContext = gestorContext;
            _erpContext = erpContext;
        }

        public ResultList<AsignaturaIntegracionDto> GetPagedAsignatura(SearchAsignaturaIntegracionParameters parameters)
        {
            var result = new ResultList<AsignaturaIntegracionDto>();
            var query = _gestorContext.AsignaturaIntegracion.AsQueryable();
            if (parameters.IdEstudio.HasValue)
                query = query.Where(ae => ae.AsignaturaUnir.EstudioUnirId == parameters.IdEstudio.Value);
            if (parameters.IdAsignatura.HasValue)
                query = query.Where(ae => ae.Id == parameters.IdAsignatura.Value);
            if (parameters.IdPlanEstudio.HasValue)
                query = query.Where(ae => ae.AsignaturaPlanIntegracion.PlanIntegracionId == parameters.IdPlanEstudio.Value);
            if (parameters.IdAsignaturaPlan.HasValue)
                query = query.Where(ae => ae.AsignaturaPlanIntegracionId == parameters.IdAsignaturaPlan.Value);

            var order = parameters.GetEnum(SearchAsignaturaIntegracionParameters.AsignaturaEstudioIntegracionOrderColumn.IdAsignatura);
            switch (order)
            {
                case SearchAsignaturaIntegracionParameters.AsignaturaEstudioIntegracionOrderColumn.IdAsignatura:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.Id)
                        : query.OrderByDescending(o => o.Id);
                    break;
                case SearchAsignaturaIntegracionParameters.AsignaturaEstudioIntegracionOrderColumn.Asignatura:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.AsignaturaUnir.Nombre)
                        : query.OrderByDescending(o => o.AsignaturaUnir.Nombre);
                    break;
                case SearchAsignaturaIntegracionParameters.AsignaturaEstudioIntegracionOrderColumn.Estudio:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.AsignaturaUnir.EstudioUnir.Nombre)
                        : query.OrderByDescending(o => o.AsignaturaUnir.EstudioUnir.Nombre);
                    break;
                case SearchAsignaturaIntegracionParameters.AsignaturaEstudioIntegracionOrderColumn.Tipo:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.AsignaturaUnir.TipoAsignatura)
                        : query.OrderByDescending(o => o.AsignaturaUnir.TipoAsignatura);
                    break;
                case SearchAsignaturaIntegracionParameters.AsignaturaEstudioIntegracionOrderColumn.Creditos:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.AsignaturaUnir.Creditos)
                        : query.OrderByDescending(o => o.AsignaturaUnir.Creditos);
                    break;
                case SearchAsignaturaIntegracionParameters.AsignaturaEstudioIntegracionOrderColumn.IdAsignaturaPlan:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.AsignaturaPlanIntegracionId)
                        : query.OrderByDescending(o => o.AsignaturaPlanIntegracionId);
                    break;
            }
  
            var listado = query.Select(a => new AsignaturaIntegracionDto
            {
                Id = a.Id,
                AsignaturaUnir = new AsignaturaUnirDto
                {
                    Id = a.Id,
                    Nombre = a.AsignaturaUnir.Nombre,
                    TipoAsignatura = a.AsignaturaUnir.TipoAsignatura,
                    Creditos = a.AsignaturaUnir.Creditos,
                    EstudioUnir = new EstudioUnirDto
                    {
                        Nombre = a.AsignaturaUnir.EstudioUnir.Nombre
                    }
                },
                AsignaturaPlanIntegracionId = a.AsignaturaPlanIntegracionId
            }).Skip((parameters.PageIndex - 1) * parameters.ItemsPerPage)
                .Take(parameters.ItemsPerPage)
                .ToList();

            /*Obtener valores otros del ERP-20*/
            var idsAsignaturasPlanes = listado.Select(a => a.AsignaturaPlanIntegracionId).ToArray();
            var asignaturasPlanes = _erpContext.AsignaturaPlan
                .Where(ap => idsAsignaturasPlanes.Contains(ap.Id))
                .Select(ap => new
                {
                    ap.Id,
                    Asignatura = new
                    {
                        ap.Asignatura.Codigo,
                        ap.Asignatura.Nombre,
                        TipoAsignatura = ap.Asignatura.TipoAsignatura.Nombre,
                        ap.Asignatura.Creditos
                    },
                    Plan = new
                    {
                        ap.Plan.Codigo,
                        ap.Plan.Nombre
                    }
                })
                .ToList();

            listado.ForEach((dto) =>
            {
                var asignaturaPlan = asignaturasPlanes.FirstOrDefault(a => a.Id == dto.AsignaturaPlanIntegracionId);
                if (asignaturaPlan != null)
                {
                    dto.AsignaturaPlan = new AsignaturaPlanDto
                    {
                        Id = asignaturaPlan.Id,
                        Asignatura = new AsignaturaDto
                        {
                            Codigo = asignaturaPlan.Asignatura.Codigo,
                            Nombre = asignaturaPlan.Asignatura.Nombre,
                            TipoAsignatura = new TipoAsignaturaDto
                            {
                                Nombre = asignaturaPlan.Asignatura.TipoAsignatura
                            },
                            Creditos = asignaturaPlan.Asignatura.Creditos
                        },
                        Plan = new PlanDto
                        {
                            Codigo = asignaturaPlan.Plan.Codigo,
                            Nombre = asignaturaPlan.Plan.Nombre
                        }
                    };
                }
            });

            result.Elements = listado;
            result.TotalElements = query.Count();
            result.PageCount = parameters.ItemsPerPage;
            
            return result;
        }

        public ResultList<AsignaturaIntegracionDto> GetFilteredList(int? idEstudioGestor, int? idAsignaturaEstudioGestor, int? idPlanErp, int? idAsignaturaPlanErp)
        {
            var result = new ResultList<AsignaturaIntegracionDto>();
            var query = _gestorContext.AsignaturaIntegracion.AsQueryable();

            if (!idEstudioGestor.HasValue && !idAsignaturaEstudioGestor.HasValue && !idPlanErp.HasValue &&
                !idAsignaturaPlanErp.HasValue)
            {
                result.Type = ResultType.ValidationError;
                result.Errors.Add("Error de Datos: Error de Datos de Gestor");
                return result;
            }

            if (idEstudioGestor.HasValue)
                query = query.Where(ae => ae.AsignaturaUnir.EstudioUnir.Id == idEstudioGestor);
            if (idAsignaturaEstudioGestor.HasValue)
                query = query.Where(ae => ae.Id == idAsignaturaEstudioGestor);
            if (idPlanErp.HasValue)
                query = query.Where(ae => ae.AsignaturaPlanIntegracion.PlanIntegracion.Id == idPlanErp);
            if (idAsignaturaPlanErp.HasValue)
                query = query.Where(ae => ae.AsignaturaPlanIntegracion.Id == idAsignaturaPlanErp);
            
            var listado = query.Select(a => new AsignaturaIntegracionDto
            {
                Id = a.Id,
                AsignaturaUnir = new AsignaturaUnirDto
                {
                    EstudioUnir = new EstudioUnirDto
                    {
                        Id = a.AsignaturaUnir.EstudioUnir.Id
                    }
                },
               AsignaturaPlanIntegracionId = a.AsignaturaPlanIntegracion.Id,
               PlantillaAsignaturaIntegracion = new PlantillaAsignaturaIntegracionDto
               {
                   PlanIntegracion = new PlantillaEstudioIntegracionDto
                   {
                       Id = a.AsignaturaPlanIntegracion.PlanIntegracion.Id
                   }
               }
                
            }).ToList();
            
            result.Elements = listado;
            return result;
        }

        public ResultValue<AsignaturaIntegracionDto> Get(int id)
        {
            var result = new ResultValue<AsignaturaIntegracionDto>();
            var asignaturaIntegracion = _gestorContext.AsignaturaIntegracion.Find(id);
            if (asignaturaIntegracion == null)
                result.Errors.Add(AsignaturaIntegracionStrings.ErrorDatosGestor);
            if (result.HasErrors)
                return result;

            var asignaturaPlanErp = _erpContext.AsignaturaPlan.Find(asignaturaIntegracion.AsignaturaPlanIntegracionId);
            if (asignaturaPlanErp == null)
                result.Errors.Add(AsignaturaIntegracionStrings.ErrorDatosErp);
            if (result.HasErrors)
            {
                result.Type = ResultType.ElementNotFound;
                return result;
            }
                

            var element  = new AsignaturaIntegracionDto
            {
                Id = asignaturaIntegracion.Id,
                AsignaturaUnir = new AsignaturaUnirDto
                {
                    Id = asignaturaIntegracion.AsignaturaUnir.Id,
                    Nombre =  asignaturaIntegracion.AsignaturaUnir.Nombre,
                    TipoAsignatura = asignaturaIntegracion.AsignaturaUnir.TipoAsignatura,
                    Creditos = asignaturaIntegracion.AsignaturaUnir.Creditos,
                    PeriodoLectivo = asignaturaIntegracion.AsignaturaUnir.PeriodoLectivo,
                    Curso = asignaturaIntegracion.AsignaturaUnir.Curso,
                    Activo = asignaturaIntegracion.AsignaturaUnir.Activo,
                    EstudioUnir = new EstudioUnirDto
                    {
                        Id = asignaturaIntegracion.AsignaturaUnir.EstudioUnir.Id,
                        Nombre = asignaturaIntegracion.AsignaturaUnir.EstudioUnir.Nombre
                    }
                },
                AsignaturaPlan = new AsignaturaPlanDto
                {
                    Id = asignaturaPlanErp.Id,
                    Asignatura = new AsignaturaDto
                    {
                        Codigo = asignaturaPlanErp.Asignatura.Codigo,
                        Nombre = asignaturaPlanErp.Asignatura.Nombre,
                        Creditos = asignaturaPlanErp.Asignatura.Creditos,
                        TipoAsignatura = new TipoAsignaturaDto
                        {
                            Nombre = asignaturaPlanErp.Asignatura.TipoAsignatura.Nombre
                        }
                    },
                    UbicacionPeriodoLectivo = asignaturaPlanErp.UbicacionPeriodoLectivo,
                    Curso = asignaturaPlanErp.CursoId.HasValue ? new CursoDto
                    {
                        Numero = asignaturaPlanErp.Curso.Numero
                    } : null,
                    DuracionPeriodoLectivo = new DuracionPeriodoLectivoDto
                    {
                        Nombre = asignaturaPlanErp.DuracionPeriodoLectivo.Nombre
                    },
                    Plan = new PlanDto
                    {
                        Id = asignaturaPlanErp.Plan.Id,
                        Codigo = asignaturaPlanErp.Plan.Codigo,
                        Nombre = asignaturaPlanErp.Plan.Nombre
                    }
                }
            };
            result.Value = element;
            return result;
        }
        public BaseResult Crear(SaveAsignaturaIntegracionParameters model)
        {
            var result = new BaseResult();
            var asignatura = _gestorContext.AsignaturaUnir.Find(model.IdAsignatura);
            var asignaturaPlanErp = _erpContext.AsignaturaPlan.Find(model.IdAsignaturaPlan);
            if (asignatura == null)
                result.Errors.Add(AsignaturaIntegracionStrings.ErrorAsignaturaNoExiste);
            if (asignaturaPlanErp == null)
                result.Errors.Add(AsignaturaIntegracionStrings.ErrorAsignaturaPlanErpNoExiste);
            if (result.HasErrors)
                return result;

            var asignaturaPlanIntegracion = _gestorContext.PlantillaAsignaturaIntegracion.Find(asignaturaPlanErp.Id);
            if (asignaturaPlanIntegracion == null)
                result.Errors.Add(AsignaturaIntegracionStrings.ErrorAsignaturaPlanNoExiste);
            else if (!asignatura.PlantillasAsignaturas.Any(pa => pa.Id == asignaturaPlanIntegracion.PlantillaAsignaturaId))
                result.Errors.Add(AsignaturaIntegracionStrings.ErrorPlantillaAsignaturaNoCoincide);
            if (_gestorContext.AsignaturaIntegracion.Any(ae => ae.Id == model.IdAsignatura))
                result.Errors.Add(AsignaturaIntegracionStrings.ErrorAsignaturaYaMapeada);
            if (result.HasErrors)
                return result;

            var asignaturaEstudioIntegracion = new AsignaturaIntegracion
            {
                Id = asignatura.Id,
                AsignaturaPlanIntegracionId = asignaturaPlanIntegracion.Id
            };
            _gestorContext.AsignaturaIntegracion.Add(asignaturaEstudioIntegracion);
            _gestorContext.SaveChanges();
            return result;
        }
        public BaseResult Modificar(SaveAsignaturaIntegracionParameters model)
        {
            var result = new BaseResult();
            var persisted = _gestorContext.AsignaturaIntegracion.Find(model.IdAsignatura);
            var asignatura = _gestorContext.AsignaturaUnir.Find(model.IdAsignatura);
            var asignaturaPlanErp = _erpContext.AsignaturaPlan.Find(model.IdAsignaturaPlan);

            if (persisted == null)
                result.Errors.Add(AsignaturaIntegracionStrings.ErrorAsignaturaEstudioNoExiste);
            if (asignatura == null)
                result.Errors.Add(AsignaturaIntegracionStrings.ErrorAsignaturaNoExiste);
            if (asignaturaPlanErp == null)
                result.Errors.Add(AsignaturaIntegracionStrings.ErrorAsignaturaPlanErpNoExiste);
            if (result.HasErrors)
                return result;

            var asignaturaPlanIntegracion = _gestorContext.PlantillaAsignaturaIntegracion.Find(asignaturaPlanErp.Id);
            if (asignaturaPlanIntegracion == null)
                result.Errors.Add(AsignaturaIntegracionStrings.ErrorAsignaturaPlanNoExiste);
            else if (!asignatura.PlantillasAsignaturas.Any(pa => pa.Id == asignaturaPlanIntegracion.PlantillaAsignaturaId))
                result.Errors.Add(AsignaturaIntegracionStrings.ErrorPlantillaAsignaturaNoCoincide);
            //if (_gestorContext.AsignaturaEstudioIntegracion.Any(ae => ae.Id == model.IdAsignatura))
            //    result.Errors.Add(AsignaturaIntegracionStrings.ErrorAsignaturaYaMapeada);
            if (result.HasErrors)
                return result;

            //Pendiente a implementacion en el webService
            persisted.Id = model.IdAsignatura;
            persisted.AsignaturaPlanIntegracionId = model.IdAsignaturaPlan;
            _gestorContext.SaveChanges();

            return result;
        }
        public BaseResult Eliminar(int[] ids)
        {
            var result = new BaseResult();
            if (ids == null)
            {
                result.Errors.Add(AsignaturaIntegracionStrings.ErrorDatosEliminar);
                return result;
            }

            var asignaturasEstudioIntegracion = _gestorContext.AsignaturaIntegracion.Where(ap => ids.Contains(ap.Id)).ToList();
            foreach (var asignaturaEstudioIntegracion in asignaturasEstudioIntegracion)
            {
                _gestorContext.AsignaturaIntegracion.Remove(asignaturaEstudioIntegracion);
            }
            _gestorContext.SaveChanges();
            return result;
        }
    }
}