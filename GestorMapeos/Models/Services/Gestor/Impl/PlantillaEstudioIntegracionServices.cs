using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using GestorMapeos.Parameters.PlantillaEstudioIntegracion;
using GestorMapeos.Globalization.Services;
using GestorMapeos.Models.Dto.Erp20;
using GestorMapeos.Models.Dto.Gestor;
using GestorMapeos.Models.Model.Erp20;
using GestorMapeos.Models.Model.Gestor;
using GestorMapeos.TransferStructs;
using GestorMapeos.Models.Model.Gestor.Entities;

namespace GestorMapeos.Models.Services.Gestor.Impl
{
    public class PlantillaEstudioIntegracionServices : IPlantillaEstudioIntegracionServices
    {
        private readonly GestorContext _gestorContext;
        private readonly ErpContext _erpContext;
        public PlantillaEstudioIntegracionServices(GestorContext gestorContext, ErpContext erpContext)
        {
            _gestorContext = gestorContext;
            _erpContext = erpContext;
        }

        public BaseResult Crear(SavePlantillaEstudioIntegracionParameters model)
        {
            var result = new BaseResult();
            var plantillaEstudio = _gestorContext.PlantillaEstudio.Find(model.IdPlantillaEstudio);
            var planEstudio = _erpContext.Plan.Find(model.IdPlan);
            if (plantillaEstudio == null)
                result.Errors.Add(PlantillaEstudioIntegracionStrings.ErrorPlantillaEstudioNoExiste);
            if (planEstudio == null)
                result.Errors.Add(PlantillaEstudioIntegracionStrings.ErrorPlanEstudioErpNoExiste);
            if (result.HasErrors)
                return result;

            if (_gestorContext.PlantillaEstudioIntegracion.Any(a => a.Id == model.IdPlan))
                result.Errors.Add(PlantillaEstudioIntegracionStrings.ErrorPlanEstudioYaMapeado);
            if (_gestorContext.PlantillaEstudioIntegracion.Any(a => a.PlantillaEstudioId == model.IdPlantillaEstudio))
                result.Errors.Add(PlantillaEstudioIntegracionStrings.ErrorPlantillaEstudioYaMapeada);
            if (result.HasErrors)
                return result;
            var planIntegracion = new PlantillaEstudioIntegracion
            {
                Id = model.IdPlan,
                PlantillaEstudioId = model.IdPlantillaEstudio
            };
            _gestorContext.PlantillaEstudioIntegracion.Add(planIntegracion);
            _gestorContext.SaveChanges();

            return result;
        }
        public BaseResult Modificar(SavePlantillaEstudioIntegracionParameters model)
        {
            var result = new BaseResult();
            var persisted = _gestorContext.PlantillaEstudioIntegracion.Find(model.IdPlan);
            var plantillaEstudio = _gestorContext.PlantillaEstudio.Find(model.IdPlantillaEstudio);
            var planEstudio = _erpContext.Plan.Find(model.IdPlan);
            if (persisted == null)
                result.Errors.Add(PlantillaEstudioIntegracionStrings.ErrorPlanIntegracionNoExiste);
            if (plantillaEstudio == null)
                result.Errors.Add(PlantillaEstudioIntegracionStrings.ErrorPlantillaEstudioNoExiste);
            if (planEstudio == null)
                result.Errors.Add(PlantillaEstudioIntegracionStrings.ErrorPlanEstudioErpNoExiste);
            if (result.HasErrors)
                return result;

            if (_gestorContext.PlantillaEstudioIntegracion.Any(p => p.PlantillaEstudioId == model.IdPlantillaEstudio && p.Id != model.IdPlan))
                result.Errors.Add(PlantillaEstudioIntegracionStrings.ErrorPlantillaEstudioYaMapeada);
            if (persisted.PlantillaEstudioId != model.IdPlantillaEstudio)
            {
                var dependencies = new List<string>();
                if (_gestorContext.EstudioIntegracion.Any(e => e.PlantillaEstudioIntegracionId == model.IdPlan))
                    dependencies.Add(PlantillaEstudioIntegracionStrings.LeyendaEstudioIntegracion);
                if (_gestorContext.PlantillaAsignaturaIntegracion.Any(ap => ap.PlanIntegracionId == model.IdPlan))
                    dependencies.Add(PlantillaEstudioIntegracionStrings.LeyendaAsignaturaPlanIntegracion);
                if (_gestorContext.PeriodoEstudioIntegracion.Any(pe => pe.PlantillaEstudioIntegracionId == model.IdPlan))
                    dependencies.Add(PlantillaEstudioIntegracionStrings.LeyendaPeriodoEstudioIntegracion);
                foreach (var dependency in dependencies)
                {
                    result.Errors.Add(string.Format(PlantillaEstudioIntegracionStrings.ErrorModificar, dependency));
                }
            }
            if (result.HasErrors)
                return result;

            persisted.Id = model.IdPlan;
            persisted.PlantillaEstudioId = model.IdPlantillaEstudio;
            _gestorContext.SaveChanges();
            
            return result;
        }
        public BaseResult Eliminar(int[] ids)
        {
            var result = new BaseResult();
            if (ids == null)
            {
                result.Errors.Add(PlantillaEstudioIntegracionStrings.ErrorDatosEliminar);
                return result;
            }
            var planesIntegracion = _gestorContext.PlantillaEstudioIntegracion.Where(a => ids.Contains(a.Id)).ToList();
            foreach (var planIntegracion in planesIntegracion)
            {
                //Dependiente EstudioIntegracion
                var dependencies = new List<string>();
                if (planIntegracion.EstudiosIntegracion.Any())
                    dependencies.Add(PlantillaEstudioIntegracionStrings.LeyendaEstudioIntegracion);
                //Dependiente AsignaturaIntegracion
                if (planIntegracion.AsignaturasIntegracion.Any())
                    dependencies.Add(PlantillaEstudioIntegracionStrings.LeyendaAsignaturaPlanIntegracion);
                //Dependiente PeriodoEstudioIntegracion
                if (planIntegracion.PeriodosEstudioIntegracion.Any())
                    dependencies.Add(PlantillaEstudioIntegracionStrings.LeyendaPeriodoEstudioIntegracion);
                if (dependencies.Any())
                {
                    result.Errors.Add(string.Format(PlantillaEstudioIntegracionStrings.ErrorEliminar,
                        planIntegracion.PlantillaEstudio.Nombre, string.Join(", ", dependencies)));
                }
                else
                {
                    _gestorContext.PlantillaEstudioIntegracion.Remove(planIntegracion);
                }
            }
            _gestorContext.SaveChanges();
            return result;
        }

        public ResultValue<PlantillaEstudioIntegracionDto> Get(int id)
        {
            var result = new ResultValue<PlantillaEstudioIntegracionDto>();
            var planGestor = _gestorContext.PlantillaEstudioIntegracion.Find(id);
            var planErp = _erpContext.Plan.Find(id);
            if (planErp == null)
                result.Errors.Add("Error de Datos: Error de Datos de Erp");
            if (planGestor == null)
                result.Errors.Add("Error de Datos: Error de Datos de Gestor");

            if (result.HasErrors)
            {
                result.Type = ResultType.ElementNotFound;
                return result;
            }
            var element = new PlantillaEstudioIntegracionDto
            {
                Id = planErp.Id,
                PlantillaEstudio = new PlantillaEstudioDto
                {
                    Id = planGestor.PlantillaEstudio.Id,
                    Nombre = planGestor.PlantillaEstudio.Nombre,
                    AnyoPlan = planGestor.PlantillaEstudio.AnyoPlan,
                    TipoEstudio = new TipoEstudioUnirDto
                    {
                        Id = planGestor.PlantillaEstudio.TipoEstudioId,
                        Nombre =  planGestor.PlantillaEstudio.TipoEstudio.Nombre
                    },
                    Rama = planGestor.PlantillaEstudio.RamaId.HasValue ? new RamaUnirDto
                    {
                        Id = planGestor.PlantillaEstudio.Rama.Id,
                        Nombre = planGestor.PlantillaEstudio.Rama.Nombre,
                    } : null
                },
                Plan = new PlanDto
                {
                    Id = planErp.Id,
                    Codigo = planErp.Codigo,
                    Nombre = planErp.Nombre,
                    Anyo = planErp.Anyo,
                    EsOficial = planErp.EsOficial,
                    Estudio = new EstudioDto
                    {
                        CodigoRuct = planErp.Estudio.CodigoRuct,
                        Nombre = planErp.Estudio.Nombre,
                        TipoEstudio = new TipoEstudioDto
                        {
                            Id = planErp.Estudio.TipoEstudioId,
                            Nombre = planErp.Estudio.TipoEstudio.Nombre
                        },
                        RamaConocimiento = new RamaConocimientoDto
                        {
                            Nombre = planErp.Estudio.RamaConocimiento.Nombre
                        }
                    },
                    Titulo = new TituloDto
                    {
                        Nombre = planErp.Titulo.Nombre,
                        CodigoMec = planErp.Titulo.CodigoMec
                    }
                }
            };
            result.Value = element;
            return result;
        }

        public ResultList<PlantillaEstudioIntegracionDto> GetFilteredList(int? idRefPlan, int? idPlantillaEstudio)
        {
            var result = new ResultList<PlantillaEstudioIntegracionDto>();

            if (!idRefPlan.HasValue && !idPlantillaEstudio.HasValue)
            {
                result.Type = ResultType.ValidationError;
                result.Errors.Add("Error de Datos: Error de Datos de Gestor");
                return result;
            }

            var query = _gestorContext.PlantillaEstudioIntegracion.AsQueryable();

            if(idRefPlan.HasValue)
                query = query.Where(a => a.Id == idRefPlan);

            if(idPlantillaEstudio.HasValue)
                query = query.Where(a => a.PlantillaEstudio.Id == idPlantillaEstudio);
            
            var listado = query.Select(p=> new PlantillaEstudioIntegracionDto
            {
                Id =p.Id,
                PlantillaEstudio = new PlantillaEstudioDto
                {
                    Id = p.PlantillaEstudio.Id
                }
            }).ToList();

            result.Type = ResultType.Ok;
            result.Elements = listado;
            return result;
        }

        public ResultList<PlantillaEstudioIntegracionDto> GetPagedPlantillasEstudio(SearchPlantillaEstudioIntegracionParameters parameters)
        {
            var result = new ResultList<PlantillaEstudioIntegracionDto>();

            var query = _gestorContext.PlantillaEstudioIntegracion.AsQueryable();

            if (parameters.FilterIdPlantillaEstudio.HasValue)
                query = query.Where(a => a.PlantillaEstudioId == parameters.FilterIdPlantillaEstudio.Value);
            if (parameters.FilterIdPlan.HasValue)
                query = query.Where(a => a.Id == parameters.FilterIdPlan.Value);
            if (parameters.IdPlantillaEstudio.HasValue)
                query = query.Where(a => a.PlantillaEstudioId == parameters.IdPlantillaEstudio.Value);
            if (parameters.IdPlan.HasValue)
                query = query.Where(a => a.Id == parameters.IdPlan.Value);

            var order = parameters.GetEnum(SearchPlantillaEstudioIntegracionParameters.PlanIntegracionOrderColumn.IdPlantilla);
            switch (order)
            {
                case SearchPlantillaEstudioIntegracionParameters.PlanIntegracionOrderColumn.IdPlantilla:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.PlantillaEstudio.Id)
                        : query.OrderByDescending(o => o.PlantillaEstudio.Id);
                    break;
                case SearchPlantillaEstudioIntegracionParameters.PlanIntegracionOrderColumn.PlantillaEstudio:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.PlantillaEstudio.Nombre + " - " + o.PlantillaEstudio.AnyoPlan)
                        : query.OrderByDescending(o => o.PlantillaEstudio.Nombre + " - " + o.PlantillaEstudio.AnyoPlan);
                    break;
                case SearchPlantillaEstudioIntegracionParameters.PlanIntegracionOrderColumn.IdPlan:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.PlantillaEstudio.Id)
                        : query.OrderByDescending(o => o.PlantillaEstudio.Id);
                    break;
            }

            var listado = query.Select(a => new PlantillaEstudioIntegracionDto
            {
                Id = a.Id,
                PlantillaEstudio = new PlantillaEstudioDto
                {
                    Id = a.PlantillaEstudio.Id,
                    Nombre = a.PlantillaEstudio.Nombre,
                    AnyoPlan = a.PlantillaEstudio.AnyoPlan
                }
            }).Skip((parameters.PageIndex - 1)*parameters.ItemsPerPage)
                .Take(parameters.ItemsPerPage)
                .ToList();

            /*Obtener valores otros del ERP-20*/
            var idsPlanes = listado.Select(a => a.Id).ToArray();
            var planes = _erpContext.Plan.Where(p => idsPlanes.Contains(p.Id)).ToList();
            listado.ForEach((dto) =>
            {
                var plan = planes.FirstOrDefault(p => p.Id == dto.Id);
                if (plan != null)
                {
                    dto.Plan = new PlanDto
                    {
                        Id = plan.Id,
                        Codigo = plan.Codigo,
                        Nombre = plan.Nombre,
                        Anyo = plan.Anyo
                    };
                }
            });
            result.Elements = listado;
            result.TotalElements = query.Count();
            result.PageCount = parameters.ItemsPerPage;

            return result;
        }

      
    }
}