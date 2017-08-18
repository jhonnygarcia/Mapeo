using System.Linq;
using GestorMapeos.Parameters.EstudioIntegracion;
using GestorMapeos.Globalization.Services;
using GestorMapeos.Models.Dto.Erp20;
using GestorMapeos.Models.Dto.Gestor;
using GestorMapeos.Models.Model.Erp20;
using GestorMapeos.Models.Model.Erp20.Entities;
using GestorMapeos.Models.Model.Gestor;
using GestorMapeos.TransferStructs;
using GestorMapeos.Models.Model.Gestor.Entities;

namespace GestorMapeos.Models.Services.Gestor.Impl
{
    public class EstudioIntegracionServices : IEstudioIntegracionServices
    {
        private readonly GestorContext _gestorContext;
        private readonly ErpContext _erpContext;
        public EstudioIntegracionServices(GestorContext gestorContext, ErpContext erpContext)
        {
            _gestorContext = gestorContext;
            _erpContext = erpContext;
        }

        public ResultValue<EstudioIntegracionDto> Get(int id)
        {
            var result = new ResultValue<EstudioIntegracionDto>();
            var estudioIntegracion = _gestorContext.EstudioIntegracion.Find(id);
            if (estudioIntegracion == null)
                result.Errors.Add("Error de Datos: Estudio Integracion Inconsistente");
            var estudioGestor = _gestorContext.EstudioUnir.Find(id);
            if (estudioGestor == null)
                result.Errors.Add("Error de Datos: Estudio Unir Inconsistente");

            if (result.HasErrors)
            {
                result.Type = ResultType.ElementNotFound;
                return result;
            }

            var erpPlan = _erpContext.Plan.Find(estudioIntegracion.PlantillaEstudioIntegracionId);
            Especializacion especializacion = null;
            if (estudioIntegracion.EspecializacionId.HasValue)
            {
                especializacion = _erpContext.Especializacion.Find(estudioIntegracion.EspecializacionId.Value);
            }
            var element = new EstudioIntegracionDto
            {
                EstudioUnir = new EstudioUnirDto
                {
                    Id = estudioGestor.Id,
                    Nombre = estudioGestor.Nombre,
                    PlanEstudio = estudioGestor.PlanEstudio,
                    Activo = estudioGestor.Activo,
                    TipoEstudio = new TipoEstudioSegunUnirDto
                    {
                        Id = estudioGestor.TipoEstudioSegunUnir.Id,
                        Nombre = estudioGestor.TipoEstudioSegunUnir.Nombre
                    },
                    RamaEstudio = estudioGestor.RamaEstudio,
                    Titulo = estudioGestor.EstudioPrincipalUnirId.HasValue ? new EstudioPrincipalUnirDto
                    {
                        Id = estudioGestor.EstudioPrincipalUnir.Id,
                        Codigo = estudioGestor.EstudioPrincipalUnir.Codigo,
                        Nombre = estudioGestor.EstudioPrincipalUnir.Nombre
                    } : null
                },
                PlantillaEstudioIntegracion = new PlantillaEstudioIntegracionDto
                {
                    Id = erpPlan.Id,
                    Plan = new PlanDto
                    {
                        Id = erpPlan.Id,
                        Nombre = erpPlan.Nombre,
                        Codigo = erpPlan.Codigo,
                        Anyo = erpPlan.Anyo,
                        EsOficial = erpPlan.EsOficial,
                        Estudio = new EstudioDto
                        {
                            CodigoRuct = erpPlan.Estudio.CodigoRuct,
                            Nombre = erpPlan.Estudio.Nombre,
                            TipoEstudio = new TipoEstudioDto
                            {
                                Nombre = erpPlan.Estudio.TipoEstudio.Nombre
                            },
                            RamaConocimiento = new RamaConocimientoDto
                            {
                                Nombre = erpPlan.Estudio.RamaConocimiento.Nombre
                            }
                        },
                        Titulo = new TituloDto
                        {
                            CodigoMec = erpPlan.Titulo.CodigoMec,
                            Nombre = erpPlan.Titulo.Nombre,
                        }
                    },
                },
                Especializacion = especializacion != null ? new EspecializacionDto
                {
                    Id = especializacion.Id,
                    Nombre = especializacion.Nombre
                } : null
            };
            result.Value = element;
            return result;
        }
        public ResultList<EstudioIntegracionDto> GetPagedEstudios(SearchEstudioIntegracionParameters parameters)
        {
            var result = new ResultList<EstudioIntegracionDto>();
            var query = _gestorContext.EstudioIntegracion.AsQueryable();
            if (parameters.IdEstudio.HasValue)//Gestor
            {
                query = query.Where(e => e.Id == parameters.IdEstudio.Value);
            }
            if (parameters.FilterEstudio.HasValue)//Gestor
            {
                query = query.Where(e => e.Id == parameters.FilterEstudio.Value);
            }
            if (parameters.IdPlanEstudio.HasValue)//Erp
            {
                query = query.Where(e => e.PlantillaEstudioIntegracion.Id == parameters.IdPlanEstudio.Value);
            }
            if (parameters.FilterPlanEstudio.HasValue)//Erp
            {
                query = query.Where(e => e.PlantillaEstudioIntegracion.Id == parameters.FilterPlanEstudio.Value);
            }
            if (parameters.IdEspecializacion.HasValue)//Erp
            {
                query = query.Where(e =>
                    e.EspecializacionId == parameters.IdEspecializacion);
            }

            var order = parameters.GetEnum(SearchEstudioIntegracionParameters.EstudioIntegracionOrderColumn.IdEstudio);
            switch (order)
            {
                case SearchEstudioIntegracionParameters.EstudioIntegracionOrderColumn.IdEstudio:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.EstudioUnir.Id)
                        : query.OrderByDescending(o => o.EstudioUnir.Id);
                    break;
                case SearchEstudioIntegracionParameters.EstudioIntegracionOrderColumn.Estudio:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.EstudioUnir.Nombre)
                        : query.OrderByDescending(o => o.EstudioUnir.Nombre);
                    break;
                case SearchEstudioIntegracionParameters.EstudioIntegracionOrderColumn.IdPlan:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.PlantillaEstudioIntegracion.Id)
                        : query.OrderByDescending(o => o.PlantillaEstudioIntegracion.Id);
                    break;
            }

            var listado = query.Select(a => new EstudioIntegracionDto
            {
                Id = a.Id,
                EstudioUnir = new EstudioUnirDto
                {
                    Id = a.Id,
                    Nombre = a.EstudioUnir.Nombre
                },
                PlantillaEstudioIntegracion = new PlantillaEstudioIntegracionDto
                {
                    Id = a.PlantillaEstudioIntegracionId
                },
                EspecializacionId = a.EspecializacionId,
                PlantillaEstudioIntegracionId = a.PlantillaEstudioIntegracionId
            }).Skip((parameters.PageIndex - 1) * parameters.ItemsPerPage)
                .Take(parameters.ItemsPerPage)
                .ToList();

            /*Obtener valor otros del ERP-20*/
            var idsPlanes = listado.Select(a => a.PlantillaEstudioIntegracion.Id).ToArray();
            var idsEspecializaciones = listado.Where(a => a.EspecializacionId.HasValue).Select(a => a.EspecializacionId.Value).ToArray();
            var planes = _erpContext.Plan.Where(p => idsPlanes.Contains(p.Id)).ToList();
            var especializaciones = _erpContext.Especializacion.Where(a => idsEspecializaciones.Contains(a.Id)).ToList();
            listado.ForEach((dto) =>
            {
                var plan = planes.FirstOrDefault(p => p.Id == dto.PlantillaEstudioIntegracion.Id);
                var especializacion = especializaciones.FirstOrDefault(a => dto.EspecializacionId == a.Id);
                if (plan != null)
                {
                    dto.PlantillaEstudioIntegracion = new PlantillaEstudioIntegracionDto
                    {
                        Plan = new PlanDto
                        {
                            Id = plan.Id,
                            Codigo = plan.Codigo,
                            Nombre = plan.Nombre,
                            Anyo = plan.Anyo
                        }
                    };
                }
                if (especializacion != null)
                {
                    dto.Especializacion = new EspecializacionDto
                    {
                        Id = especializacion.Id,
                        Nombre = especializacion.Nombre
                    };
                }
            });
            result.Elements = listado;
            result.TotalElements = query.Count();
            result.PageCount = parameters.ItemsPerPage;
            return result;

        }

        public ResultList<EstudioIntegracionDto> GetFilteredList(int? idEstudioGestor, int? idPlanErp,int? idEspecializacionErp)
        {
            var result = new ResultList<EstudioIntegracionDto>();
            var query = _gestorContext.EstudioIntegracion.AsQueryable();

            if (!idEstudioGestor.HasValue && !idPlanErp.HasValue && !idEspecializacionErp.HasValue)
            {
                result.Type = ResultType.ValidationError;
                result.Errors.Add("Error de Datos: Error de Datos de Gestor");
                return result;
            }

            if (idEstudioGestor.HasValue)//Gestor
            {
                query = query.Where(e => e.Id == idEstudioGestor);
            }

            if (idPlanErp.HasValue)//Gestor
            {
                query = query.Where(e => e.PlantillaEstudioIntegracionId == idPlanErp);
            }

            if (idEspecializacionErp.HasValue)//Erp
            {
                query = query.Where(e => e.EspecializacionId == idEspecializacionErp);
            }

            var listado = query.Select(a => new EstudioIntegracionDto
            {
                Id = a.Id,
                PlantillaEstudioIntegracion = new PlantillaEstudioIntegracionDto
                {
                    Id = a.PlantillaEstudioIntegracionId
                },
                EspecializacionId = a.EspecializacionId
            }).ToList();
          
            result.Elements = listado;
            return result;

        }

        public BaseResult Crear(SaveEstudioIntegracionParameters model)
        {
            var result = new BaseResult();

            //Validar Referencias

            var estudioUnir = _gestorContext.EstudioUnir.Find(model.IdEstudioGestor);
            if (estudioUnir == null)
                result.Errors.Add(EstudioIntegracionStrings.ErrorNoEstudioUnir);

            var planIntegracion = _gestorContext.PlantillaEstudioIntegracion.FirstOrDefault(p => p.Id == model.IdRefPlanErp);
            if (planIntegracion == null)//Validacion 4
                result.Errors.Add(EstudioIntegracionStrings.ErrorMapeoPlantillaEstudio);

            var estudioIntegracion = _gestorContext.EstudioIntegracion.FirstOrDefault(e => e.Id == model.IdEstudioGestor);
            if (estudioIntegracion != null)//Validacion 6
                result.Errors.Add(EstudioIntegracionStrings.ErrorMapeoExistente);

            if (result.HasErrors)
                return result;

            var plantilla = _gestorContext.PlantillaEstudio.Find(planIntegracion.PlantillaEstudioId);
            var plantillaEstudio_Estudio =
                _gestorContext.EstudioUnir.Find(model.IdEstudioGestor)
                    .PlantillasEstudio.FirstOrDefault(p => p.Id == plantilla.Id);
            if (plantillaEstudio_Estudio == null)//Validacion 5
            {
                result.Errors.Add(EstudioIntegracionStrings.ErrorMapeoPlanNoAsociadoConPlantilla);
                return result;
            }

            var mapeoIntegracionSinEspecializacion =
                _gestorContext.EstudioIntegracion.FirstOrDefault(
                    e =>
                        e.PlantillaEstudioIntegracionId == model.IdRefPlanErp && !e.EspecializacionId.HasValue &&
                        !model.IdRefEspecializacion.HasValue);
            if (mapeoIntegracionSinEspecializacion != null) //Validacion 7.1
            {
                result.Errors.Add(EstudioIntegracionStrings.ErrorMapeoPlanExistenteSinEspecializacion);
                return result;
            }
            var mapeoIntegracionConEspecializacion =
                _gestorContext.EstudioIntegracion.FirstOrDefault(
                    e =>
                        e.PlantillaEstudioIntegracionId == model.IdRefPlanErp && e.EspecializacionId.HasValue &&
                        model.IdRefEspecializacion.HasValue && e.EspecializacionId == model.IdRefEspecializacion);
            if (mapeoIntegracionConEspecializacion != null) //Validacion 7.2
            {
                result.Errors.Add(EstudioIntegracionStrings.ErrorMapeoPlanExistenteConEspecialización);
                return result;
            }
            if (model.IdRefEspecializacion.HasValue)
            {
                var especializacion =
                    _erpContext.Especializacion.Where(
                        e => e.Hitos.Any(p => p.Hito.Nodos.Any(n => n.Plan.Id == model.IdRefPlanErp)))
                        .FirstOrDefault(p => p.Id == model.IdRefEspecializacion);
                if (especializacion == null)//Validacion 8
                {
                    result.Errors.Add(EstudioIntegracionStrings.ErrorMapeoEspecializacionNoExistente);
                    return result;
                }
            }
            if (result.HasErrors)
                return result;

            var mapeoEstudio = new EstudioIntegracion()
            {
                Id = model.IdEstudioGestor,
                PlantillaEstudioIntegracionId = model.IdRefPlanErp,
                EspecializacionId = model.IdRefEspecializacion
            };
            _gestorContext.EstudioIntegracion.Add(mapeoEstudio);
            _gestorContext.SaveChanges();

            return result;
        }
        public BaseResult Modificar(SaveEstudioIntegracionParameters model)
        {
            var result = new BaseResult();
            //Validar Referencias

            var estudioUnir = _gestorContext.EstudioUnir.Find(model.IdEstudioGestor);
            if (estudioUnir == null)
                result.Errors.Add(EstudioIntegracionStrings.ErrorNoEstudioUnir);

            var planIntegracion = _gestorContext.PlantillaEstudioIntegracion.FirstOrDefault(p => p.Id == model.IdRefPlanErp);
            if (planIntegracion == null)//Validacion 4
                result.Errors.Add(EstudioIntegracionStrings.ErrorMapeoPlantillaEstudio);


            //Validacion Existencialidad
            var persisted = _gestorContext.EstudioIntegracion.Find(model.IdEstudioGestor);
            if (persisted == null)
                result.Errors.Add(EstudioIntegracionStrings.ErrorNoEstudioIntegracion);

            if (result.HasErrors)
                return result;

            //Refencias de Mapeo    

            var plantilla = _gestorContext.PlantillaEstudio.Find(planIntegracion.PlantillaEstudioId);
            var plantillaEstudio_Estudio =
                _gestorContext.EstudioUnir.Find(model.IdEstudioGestor).PlantillasEstudio.FirstOrDefault(p => p.Id == plantilla.Id);
            if (plantillaEstudio_Estudio == null)//Validacion 5
            {
                result.Errors.Add(EstudioIntegracionStrings.ErrorMapeoPlanNoAsociadoConPlantilla);
                return result;
            }

            var mapeoIntegracionSinEspecializacion =
                _gestorContext.EstudioIntegracion.FirstOrDefault(
                    e =>
                        e.Id != model.IdEstudioGestor && e.PlantillaEstudioIntegracionId == model.IdRefPlanErp &&
                        !e.EspecializacionId.HasValue && !model.IdRefEspecializacion.HasValue);
            if (mapeoIntegracionSinEspecializacion != null) //Validacion 6.1
            {
                result.Errors.Add(EstudioIntegracionStrings.ErrorMapeoPlanExistenteSinEspecializacion);
                return result;
            }
            var mapeoIntegracionConEspecializacion =
                _gestorContext.EstudioIntegracion.FirstOrDefault(
                    e =>
                        e.Id != model.IdEstudioGestor && e.PlantillaEstudioIntegracionId == model.IdRefPlanErp &&
                        e.EspecializacionId.HasValue && model.IdRefEspecializacion.HasValue &&
                        e.EspecializacionId == model.IdRefEspecializacion);
            if (mapeoIntegracionConEspecializacion != null) //Validacion 6.2
            {
                result.Errors.Add(EstudioIntegracionStrings.ErrorMapeoPlanExistenteConEspecialización);
                return result;
            }
            if (model.IdRefEspecializacion.HasValue)
            {
                var especializacion = _erpContext.Especializacion.Where(e => e.Hitos.Any(p => p.Hito.Nodos.Any(n => n.Plan.Id == model.IdRefPlanErp))).FirstOrDefault(p => p.Id == model.IdRefEspecializacion);
                if (especializacion == null)//Validacion 7
                {
                    result.Errors.Add(EstudioIntegracionStrings.ErrorMapeoEspecializacionNoExistente);
                    return result;
                }
            }
            if (result.HasErrors)
                return result;

            //Pediente a ser implementado en el webService
            persisted.Id = model.IdEstudioGestor;
            persisted.PlantillaEstudioIntegracionId = model.IdRefPlanErp;
            persisted.EspecializacionId = model.IdRefEspecializacion;
            _gestorContext.SaveChanges();

            return result;
        }
        public BaseResult Eliminar(int[] ids)
        {
            var result = new BaseResult();
            if (ids == null)
            {
                result.Errors.Add(EstudioIntegracionStrings.ErrorIdsNoEnviados);
                return result;
            }
            var estudiosIntegracion = _gestorContext.EstudioIntegracion.Where(a => ids.Contains(a.Id)).ToList();
            if (!estudiosIntegracion.Any())
            {
                result.Errors.Add(EstudioIntegracionStrings.ErrorMapeosNoExistentes);
                return result;
            }
            foreach (var estudioIntegracion in estudiosIntegracion)
            {
                _gestorContext.EstudioIntegracion.Remove(estudioIntegracion);
            }
            _gestorContext.SaveChanges();
            return result;
        }
    }
}