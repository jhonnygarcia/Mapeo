using System.Collections.Generic;
using System.Linq;
using GestorMapeos.Parameters.PlantillaAsignaturaIntegracion;
using GestorMapeos.Globalization.Services;
using GestorMapeos.Models.Dto.Erp20;
using GestorMapeos.Models.Dto.Gestor;
using GestorMapeos.Models.Model.Erp20;
using GestorMapeos.Models.Model.Gestor;
using GestorMapeos.Models.Model.Gestor.Entities;
using GestorMapeos.TransferStructs;

namespace GestorMapeos.Models.Services.Gestor.Impl
{
    public class PlantillaAsignaturaIntegracionServices : IPlantillaAsignaturaIntegracionServices
    {
        private readonly GestorContext _gestorContext;
        private readonly ErpContext _erpContext;
        public PlantillaAsignaturaIntegracionServices(GestorContext gestorContext, ErpContext erpContext)
        {
            _gestorContext = gestorContext;
            _erpContext = erpContext;
        }

        public BaseResult Crear(SavePlantillaAsignaturaIntegracionParameters model)
        {
            var result = new BaseResult();
            var plantillaAsignatura = _gestorContext.PlantillaAsignatura.Find(model.IdPlantillaAsignatura);
            var asignaturaPlanErp = _erpContext.AsignaturaPlan.Find(model.IdAsignaturaPlan);
            if (plantillaAsignatura == null)
                result.Errors.Add(PlantillaAsignaturaIntegracionStrings.ErrorPlantillaAsignaturaNoExiste);
            if (asignaturaPlanErp == null)
                result.Errors.Add(PlantillaAsignaturaIntegracionStrings.ErrorAsignaturaPlanErpNoExiste);
            if (result.HasErrors)
                return result;

            var planIntegracion = _gestorContext.PlantillaEstudioIntegracion.FirstOrDefault(p => p.PlantillaEstudioId == plantillaAsignatura.PlantillaEstudioId);
            if (!_gestorContext.PlantillaEstudioIntegracion.Any(p => p.Id == asignaturaPlanErp.PlanId))
                result.Errors.Add(PlantillaAsignaturaIntegracionStrings.ErrorPlanIntegracionDesdeAsignaturaPlanNoExiste);
            if (planIntegracion == null)
                result.Errors.Add(PlantillaAsignaturaIntegracionStrings.ErrorPlanIntegracionDesdePlantillaAsignaturaNoExiste);
            else if (planIntegracion.Id != asignaturaPlanErp.PlanId)
                result.Errors.Add(PlantillaAsignaturaIntegracionStrings.ErrorPlantillaEstudioNoCoincide);
            if (_gestorContext.PlantillaAsignaturaIntegracion.Any(ap => ap.PlantillaAsignaturaId == model.IdPlantillaAsignatura))
                result.Errors.Add(PlantillaAsignaturaIntegracionStrings.ErrorPlantillaAsignaturaYaMapeada);
            if (_gestorContext.PlantillaAsignaturaIntegracion.Any(ap => ap.Id == model.IdAsignaturaPlan))
                result.Errors.Add(PlantillaAsignaturaIntegracionStrings.ErrorAsignaturaPlanYaMapeada);
            if (result.HasErrors)
                return result;
            var asignaturaPlanIntegracion = new PlantillaAsignaturaIntegracion
            {
                Id = asignaturaPlanErp.Id,
                PlantillaAsignaturaId = plantillaAsignatura.Id,
                PlanIntegracionId = planIntegracion.Id
            };
            _gestorContext.PlantillaAsignaturaIntegracion.Add(asignaturaPlanIntegracion);
            _gestorContext.SaveChanges();
            return result;
        }

        public BaseResult Modificar(SavePlantillaAsignaturaIntegracionParameters model)
        {
            var result = new BaseResult();
            var persisted = _gestorContext.PlantillaAsignaturaIntegracion.Find(model.IdAsignaturaPlan);
            var plantillaAsignatura = _gestorContext.PlantillaAsignatura.Find(model.IdPlantillaAsignatura);
            var asignaturaPlanErp = _erpContext.AsignaturaPlan.Find(model.IdAsignaturaPlan);

            if (persisted == null)
                result.Errors.Add(PlantillaAsignaturaIntegracionStrings.ErrorAsignaturaPlanNoExiste);
            if (plantillaAsignatura == null)
                result.Errors.Add(PlantillaAsignaturaIntegracionStrings.ErrorPlantillaAsignaturaNoExiste);
            if (asignaturaPlanErp == null)
                result.Errors.Add(PlantillaAsignaturaIntegracionStrings.ErrorAsignaturaPlanErpNoExiste);
            if (result.HasErrors)
                return result;

            var planIntegracion = _gestorContext.PlantillaEstudioIntegracion.FirstOrDefault(p => p.PlantillaEstudioId == plantillaAsignatura.PlantillaEstudioId);
            if (!_gestorContext.PlantillaEstudioIntegracion.Any(p => p.Id == asignaturaPlanErp.PlanId))
                result.Errors.Add(PlantillaAsignaturaIntegracionStrings.ErrorPlanIntegracionDesdeAsignaturaPlanNoExiste);
            if (planIntegracion == null)
                result.Errors.Add(PlantillaAsignaturaIntegracionStrings.ErrorPlanIntegracionDesdePlantillaAsignaturaNoExiste);
            else if (planIntegracion.Id != asignaturaPlanErp.PlanId)
                result.Errors.Add(PlantillaAsignaturaIntegracionStrings.ErrorPlantillaEstudioNoCoincide);
            if (_gestorContext.PlantillaAsignaturaIntegracion.Any(ap => ap.PlantillaAsignaturaId == model.IdPlantillaAsignatura && ap.Id != model.IdAsignaturaPlan))
                result.Errors.Add(PlantillaAsignaturaIntegracionStrings.ErrorPlantillaAsignaturaYaMapeada);
            if (persisted.PlantillaAsignaturaId != model.IdPlantillaAsignatura)
            {
                if (_gestorContext.AsignaturaIntegracion.Any(ae => ae.AsignaturaPlanIntegracionId == model.IdAsignaturaPlan))
                    result.Errors.Add(string.Format(PlantillaAsignaturaIntegracionStrings.ErrorModificarPorAsignaturaEstudioIntegracion));
            }
            if (result.HasErrors)
                return result;

            if (result.HasErrors)
                return result;

            //Pendiente a ser implementado en el webService
            persisted.Id = model.IdAsignaturaPlan;
            persisted.PlantillaAsignaturaId = model.IdPlantillaAsignatura;
            persisted.PlanIntegracionId = planIntegracion.Id;
            _gestorContext.SaveChanges();

            return result;
        }

        public BaseResult Eliminar(int[] ids)
        {
            var result = new BaseResult();
            if (ids == null)
            {
                result.Errors.Add(PlantillaAsignaturaIntegracionStrings.ErrorDatosEliminar);
                return result;
            }

            var asignaturasPlanIntegracion = _gestorContext.PlantillaAsignaturaIntegracion.Where(ap => ids.Contains(ap.Id)).ToList();
            foreach (var asignaturaPlanIntegracion in asignaturasPlanIntegracion)
            {
                //Dependiente AsignaturaEstudioIntegracion
                var dependencies = new List<string>();
                if (asignaturaPlanIntegracion.AsignaturasEstudioIntegracion != null && asignaturaPlanIntegracion.AsignaturasEstudioIntegracion.Any())
                    dependencies.Add(PlantillaAsignaturaIntegracionStrings.LeyendaAsignaturaEstudioIntegracion);
                if (dependencies.Any())
                {
                    result.Errors.Add(string.Format(PlantillaAsignaturaIntegracionStrings.ErrorEliminar,
                        asignaturaPlanIntegracion.PlantillaAsignatura.NombreAsignatura, string.Join(", ", dependencies)));
                }
                else
                {
                    _gestorContext.PlantillaAsignaturaIntegracion.Remove(asignaturaPlanIntegracion);
                }
            }
            _gestorContext.SaveChanges();
            return result;
        }

        public ResultList<PlantillaAsignaturaIntegracionDto> GetPagedPlantillaAsignatura(SearchPlantillaAsignaturaIntegracionParameters parameters)
        {
            var result = new ResultList<PlantillaAsignaturaIntegracionDto>();
            var query = _gestorContext.PlantillaAsignaturaIntegracion.AsQueryable();

            if (parameters.FilterIdPlantillaEstudio.HasValue)
                query = query.Where(ap => ap.PlantillaAsignatura.PlantillaEstudioId == parameters.FilterIdPlantillaEstudio.Value);
            if (parameters.FilterIdPlantillaAsignatura.HasValue)
                query = query.Where(ap => ap.PlantillaAsignaturaId == parameters.FilterIdPlantillaAsignatura.Value);
            if (parameters.FilterIdPlanEstudio.HasValue)
                query = query.Where(ap => ap.PlanIntegracionId == parameters.FilterIdPlanEstudio.Value);
            if (parameters.FilterIdAsignaturaPlan.HasValue)
                query = query.Where(ap => ap.Id == parameters.FilterIdAsignaturaPlan.Value);

            if (parameters.IdPlantillaEstudio.HasValue)
                query = query.Where(ap => ap.PlantillaAsignatura.PlantillaEstudioId == parameters.IdPlantillaEstudio.Value);
            if (parameters.IdPlantillaAsignatura.HasValue)
                query = query.Where(ap => ap.PlantillaAsignaturaId == parameters.IdPlantillaAsignatura.Value);
            if (parameters.IdPlanEstudio.HasValue)
                query = query.Where(ap => ap.PlanIntegracionId == parameters.IdPlanEstudio.Value);
            if (parameters.IdAsignaturaPlan.HasValue)
                query = query.Where(ap => ap.Id == parameters.IdAsignaturaPlan.Value);

            var order = parameters.GetEnum(SearchPlantillaAsignaturaIntegracionParameters.AsignaturaPlanIntegracionOrderColumn.IdPlantilla);
            switch (order)
            {
                case SearchPlantillaAsignaturaIntegracionParameters.AsignaturaPlanIntegracionOrderColumn.IdPlantilla:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.PlantillaAsignatura.Id)
                        : query.OrderByDescending(o => o.PlantillaAsignatura.Id);
                    break;
                case SearchPlantillaAsignaturaIntegracionParameters.AsignaturaPlanIntegracionOrderColumn.PlantillaAsignatura:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.PlantillaAsignatura.NombreAsignatura)
                        : query.OrderByDescending(o => o.PlantillaAsignatura.NombreAsignatura);
                    break;
                case SearchPlantillaAsignaturaIntegracionParameters.AsignaturaPlanIntegracionOrderColumn.PlantillaEstudio:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.PlantillaAsignatura.PlantillaEstudio.Nombre + " - " + o.PlantillaAsignatura.PlantillaEstudio.AnyoPlan)
                        : query.OrderByDescending(o => o.PlantillaAsignatura.PlantillaEstudio.Nombre + " - " + o.PlantillaAsignatura.PlantillaEstudio.AnyoPlan);
                    break;
                case SearchPlantillaAsignaturaIntegracionParameters.AsignaturaPlanIntegracionOrderColumn.Tipo:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.PlantillaAsignatura.TipoAsignatura.Nombre)
                        : query.OrderByDescending(o => o.PlantillaAsignatura.TipoAsignatura.Nombre);
                    break;
                case SearchPlantillaAsignaturaIntegracionParameters.AsignaturaPlanIntegracionOrderColumn.Creditos:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.PlantillaAsignatura.Creditos)
                        : query.OrderByDescending(o => o.PlantillaAsignatura.Creditos);
                    break;
                case SearchPlantillaAsignaturaIntegracionParameters.AsignaturaPlanIntegracionOrderColumn.IdAsignaturaPlan:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.Id)
                        : query.OrderByDescending(o => o.Id);
                    break;
            }

            var listado = query.Select(a => new PlantillaAsignaturaIntegracionDto
            {
                Id = a.Id,
                PlantillaAsignatura = new PlantillaAsignaturaDto
                {
                    Id = a.PlantillaAsignaturaId,
                    NombreAsignatura = a.PlantillaAsignatura.NombreAsignatura,
                    TipoAsignatura = new TipoAsignaturaUnirDto
                    {
                        Id = a.PlantillaAsignatura.TipoAsignatura.Id,
                        Nombre = a.PlantillaAsignatura.TipoAsignatura.Nombre
                    },
                    Creditos = a.PlantillaAsignatura.Creditos,
                    PlantillaEstudio = new PlantillaEstudioDto
                    {
                        Nombre = a.PlantillaAsignatura.PlantillaEstudio.Nombre,
                        AnyoPlan = a.PlantillaAsignatura.PlantillaEstudio.AnyoPlan
                    }
                }
            }).Skip((parameters.PageIndex - 1) * parameters.ItemsPerPage)
                .Take(parameters.ItemsPerPage)
                .ToList();

            /*Obtener valores otros del ERP-20*/
            var idsAsignaturasPlanes = listado.Select(a => a.Id).ToArray();
            var asignaturasPlanes = _erpContext.AsignaturaPlan
                .Where(ap => idsAsignaturasPlanes.Contains(ap.Id))
                .Select(ap => new
                {
                    ap.Id,
                    Asignatura = new
                    {
                        ap.Asignatura.Codigo,
                        ap.Asignatura.Nombre,
                        TipoAsignatura = new
                        {
                            ap.Asignatura.TipoAsignatura.Nombre
                        },
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
                var asignaturaPlan = asignaturasPlanes.FirstOrDefault(a => a.Id == dto.Id);
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
                                Nombre = asignaturaPlan.Asignatura.TipoAsignatura.Nombre
                            },
                            Creditos = asignaturaPlan.Asignatura.Creditos
                        },
                        Plan = new PlanDto
                        {
                            Codigo = asignaturaPlan.Plan.Codigo,
                            Nombre = asignaturaPlan.Plan.Nombre,
                        }
                    };
                }
            });

            result.Elements = listado;
            result.TotalElements = query.Count();
            result.PageCount = parameters.ItemsPerPage;
            return result;
        }

        public ResultValue<PlantillaAsignaturaIntegracionDto> Get(int id)
        {
            var result = new ResultValue<PlantillaAsignaturaIntegracionDto>();
            var asignaturaPlanGestor = _gestorContext.PlantillaAsignaturaIntegracion.Find(id);
            var asignaturaPlanErp = _erpContext.AsignaturaPlan.Find(id);
            if (asignaturaPlanErp == null)
                result.Errors.Add(PlantillaAsignaturaIntegracionStrings.ErrorDatosErp);
            if (asignaturaPlanGestor == null)
                result.Errors.Add(PlantillaAsignaturaIntegracionStrings.ErrorDatosGestor);

            if (result.HasErrors)
            {
                result.Type = ResultType.ElementNotFound;
                return result;
            }
            var element = new PlantillaAsignaturaIntegracionDto
            {
                PlantillaAsignatura = new PlantillaAsignaturaDto
                {
                    Id = asignaturaPlanGestor.PlantillaAsignatura.Id,
                    NombreAsignatura = asignaturaPlanGestor.PlantillaAsignatura.NombreAsignatura,
                    Codigo = asignaturaPlanGestor.PlantillaAsignatura.Codigo,
                    Creditos = asignaturaPlanGestor.PlantillaAsignatura.Creditos,
                    TipoAsignatura = new TipoAsignaturaUnirDto
                    {
                        Nombre = asignaturaPlanGestor.PlantillaAsignatura.TipoAsignatura.Nombre
                    },
                    PlantillaEstudio = new PlantillaEstudioDto
                    {
                        Id = asignaturaPlanGestor.PlantillaAsignatura.PlantillaEstudio.Id,
                        Nombre = asignaturaPlanGestor.PlantillaAsignatura.PlantillaEstudio.Nombre,
                        AnyoPlan = asignaturaPlanGestor.PlantillaAsignatura.PlantillaEstudio.AnyoPlan
                    }
                },
                AsignaturaPlan = new AsignaturaPlanDto
                {
                    Id = asignaturaPlanErp.Id,
                    Asignatura = new AsignaturaDto
                    {
                        Creditos = asignaturaPlanErp.Asignatura.Creditos,
                        Nombre = asignaturaPlanErp.Asignatura.Nombre,
                        Codigo = asignaturaPlanErp.Asignatura.Codigo,
                        TipoAsignatura = new TipoAsignaturaDto
                        {
                            Nombre = asignaturaPlanErp.Asignatura.TipoAsignatura.Nombre
                        }
                    },
                    Plan = new PlanDto
                    {
                        Codigo = asignaturaPlanErp.Plan.Codigo,
                        Nombre = asignaturaPlanErp.Plan.Nombre
                    }
                }
            };
            result.Value = element;
            return result;
        }

        public ResultList<PlantillaAsignaturaIntegracionDto> GetFilteredList(int? idPlantillaEstudio, int? idPlantillaAsignatura, int? idPlanErp, int? idAsignaturaPlanErp)
        {
            var result = new ResultList<PlantillaAsignaturaIntegracionDto>();

            if (!idPlantillaEstudio.HasValue && !idPlantillaAsignatura.HasValue && !idPlanErp.HasValue && !idAsignaturaPlanErp.HasValue)
            {
                result.Type = ResultType.ValidationError;
                result.Errors.Add("Error de Datos: Error de Datos de Gestor");
                return result;
            }
            var query = _gestorContext.PlantillaAsignaturaIntegracion.AsQueryable();

            if (idPlantillaEstudio.HasValue)
                query = query.Where(a => a.PlantillaAsignatura.PlantillaEstudio.Id == idPlantillaEstudio);

            if (idPlantillaAsignatura.HasValue)
                query = query.Where(a => a.PlantillaAsignatura.Id == idPlantillaAsignatura);

            if (idPlanErp.HasValue)
                query = query.Where(a => a.PlanIntegracion.Id == idPlanErp);

            if (idAsignaturaPlanErp.HasValue)
                query = query.Where(a => a.Id == idAsignaturaPlanErp);


            var listado = query.Select(p => new PlantillaAsignaturaIntegracionDto
            {
                Id = p.Id,
                PlantillaAsignatura = new PlantillaAsignaturaDto
                {
                    Id = p.PlantillaAsignatura.Id,
                    PlantillaEstudio = new PlantillaEstudioDto
                    {
                        Id = p.PlantillaAsignatura.PlantillaEstudio.Id
                    }
                },
                PlanIntegracion = new PlantillaEstudioIntegracionDto
                {
                   Id = p.PlanIntegracion.Id
                }
            }).ToList();

            result.Type = ResultType.Ok;
            result.Elements = listado;
            return result;
        }
    }
}