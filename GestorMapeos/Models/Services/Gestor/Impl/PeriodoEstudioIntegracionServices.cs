using GestorMapeos.Parameters.PeriodoEstudioIntegracion;
using System.Linq;
using GestorMapeos.Globalization.Services;
using GestorMapeos.Models.Dto.Erp20;
using GestorMapeos.Models.Dto.Gestor;
using GestorMapeos.Models.Model.Erp20;
using GestorMapeos.Models.Model.Gestor;
using GestorMapeos.Models.Model.Gestor.Entities;
using GestorMapeos.TransferStructs;

namespace GestorMapeos.Models.Services.Gestor.Impl
{
    public class PeriodoEstudioIntegracionServices : IPeriodoEstudioIntegracionServices
    {
        private readonly GestorContext _gestorContext;
        private readonly ErpContext _erpContext;
        public PeriodoEstudioIntegracionServices(GestorContext gestorContext, ErpContext erpContext)

        {
            _gestorContext = gestorContext;
            _erpContext = erpContext;
        }

        public ResultList<PeriodoEstudioIntegracionDto> GetPagedPeriodoEstudio(SearchPeriodoEstudioIntegracionParameters parameters)
        {
            var result = new ResultList<PeriodoEstudioIntegracionDto>();
            var query = _gestorContext.PeriodoEstudioIntegracion.AsQueryable();

            if (parameters.IdPeriodoEstudio.HasValue)
                query = query.Where(pe => pe.Id == parameters.IdPeriodoEstudio.Value);
            if (parameters.IdPeriodoMatriculacion.HasValue)
                query = query.Where(
                        pe => pe.PeriodoEstudioUnir.PeriodoMatriculacionId == parameters.IdPeriodoMatriculacion.Value);
            if (parameters.IdEstudio.HasValue)
                query =
                    query.Where(pe => pe.PeriodoEstudioUnir.EstudioId == parameters.IdEstudio.Value);
            if (parameters.IdPlanOfertado.HasValue)
                query =
                    query.Where(pe => pe.PlanOfertadoId == parameters.IdPlanOfertado.Value);
            if (parameters.IdPeriodoAcademico.HasValue)
            {
                var planOfertado = _erpContext.PlanOfertado.FirstOrDefault(p => p.PeriodoAcademicoId == parameters.IdPeriodoAcademico.Value);
                query = query.Where(pe => pe.PlanOfertadoId == planOfertado.Id);
            }

            if (parameters.IdPlanEstudio.HasValue)
                query = query.Where(pe => pe.PlantillaEstudioIntegracionId == parameters.IdPlanEstudio.Value);

            var order = parameters.GetEnum(
                    SearchPeriodoEstudioIntegracionParameters.PeriodoEstudioIntegracionOrderColumn.IdPeriodoEstudio);
            switch (order)
            {
                case SearchPeriodoEstudioIntegracionParameters.PeriodoEstudioIntegracionOrderColumn.IdPeriodoEstudio:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.PeriodoEstudioUnir.Id)
                        : query.OrderByDescending(o => o.PeriodoEstudioUnir.Id);
                    break;
                case SearchPeriodoEstudioIntegracionParameters.PeriodoEstudioIntegracionOrderColumn.Estudio:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.PeriodoEstudioUnir.EstudioUnir.Nombre)
                        : query.OrderByDescending(o => o.PeriodoEstudioUnir.EstudioUnir.Nombre);
                    break;
                case SearchPeriodoEstudioIntegracionParameters.PeriodoEstudioIntegracionOrderColumn.PeriodoMatriculacion
                    :
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.PeriodoEstudioUnir.PeriodoMatriculacionUnir.Nombre)
                        : query.OrderByDescending(o => o.PeriodoEstudioUnir.PeriodoMatriculacionUnir.Nombre);
                    break;
                case SearchPeriodoEstudioIntegracionParameters.PeriodoEstudioIntegracionOrderColumn.IdPlanOfertado:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.PlanOfertadoId)
                        : query.OrderByDescending(o => o.PlanOfertadoId);
                    break;
            }

            var listado = query.Select(a => new PeriodoEstudioIntegracionDto
            {
                Id = a.Id,
                PeriodoEstudioUnir = new PeriodoEstudioUnirDto
                {
                    Id = a.PeriodoEstudioUnir.Id,
                    EstudioUnir = new EstudioUnirDto
                    {
                        Nombre = a.PeriodoEstudioUnir.EstudioUnir.Nombre
                    },
                    PeriodoMatriculacionUnir = new PeriodoMatriculacionUnirDto
                    {
                        Nombre = a.PeriodoEstudioUnir.PeriodoMatriculacionUnir.Nombre
                    }
                },
                PlanOfertadoId = a.PlanOfertadoId,
                PlantillaEstudioIntegracionId = a.PlantillaEstudioIntegracionId
            }).Skip((parameters.PageIndex - 1) * parameters.ItemsPerPage)
                .Take(parameters.ItemsPerPage)
                .ToList();

            /*Obtener valores otros del ERP-20*/
            var idsPlanesOfertados = listado.Select(pe => pe.PlanOfertadoId).ToArray();
            var planesOfertados = _erpContext.PlanOfertado
                .Where(po => idsPlanesOfertados.Contains(po.Id))
                .Select(po => new
                {
                    po.Id,
                    Plan = new
                    {
                        po.Plan.Codigo,
                        po.Plan.Nombre,
                        DisplayName = po.Plan.Codigo + " - " + po.Plan.Nombre
                    },
                    PeriodoAcademico = new
                    {
                        po.PeriodoAcademico.Nombre,
                        po.PeriodoAcademico.FechaInicio,
                        po.PeriodoAcademico.FechaFin
                    }
                }).ToList();

            listado.ForEach((dto) =>
            {
                var planOfertado = planesOfertados.FirstOrDefault(a => a.Id == dto.PlanOfertadoId);
                if (planOfertado != null)
                {
                    dto.PlanOfertado = new PlanOfertadoDto
                    {
                        Id = planOfertado.Id,
                        Plan = new PlanDto
                        {
                            Nombre = planOfertado.Plan.Nombre,
                            Codigo = planOfertado.Plan.Codigo
                        },
                        PeriodoAcademico = new PeriodoAcademicoDto
                        {
                            Nombre = planOfertado.PeriodoAcademico.Nombre,
                            FechaInicio = planOfertado.PeriodoAcademico.FechaInicio,
                            FechaFin = planOfertado.PeriodoAcademico.FechaFin
                        }
                    };
                }
            });
            result.Elements = listado;
            result.TotalElements = query.Count();
            result.PageCount = parameters.ItemsPerPage;

            return result;
        }

        public ResultValue<PeriodoEstudioIntegracionDto> Get(int id)
        {
            var result = new ResultValue<PeriodoEstudioIntegracionDto>();
            var periodoEstudioIntegracion = _gestorContext.PeriodoEstudioIntegracion.Find(id);
            if (periodoEstudioIntegracion == null)
            {
                result.Errors.Add("Error de Datos: Periodo Estudio Inconsistente");
                result.Type = ResultType.ElementNotFound;
                return result;
            }
            var planOfertado = _erpContext.PlanOfertado.Find(periodoEstudioIntegracion.PlanOfertadoId);
            if (planOfertado == null)
            {
                result.Errors.Add("Error de Datos: Plan Ofertado Inconsistente");
            }
            if (result.HasErrors)
            {
                result.Type = ResultType.ElementNotFound;
                return result;
            }

            var element = new PeriodoEstudioIntegracionDto
            {
                Id = periodoEstudioIntegracion.Id,
                PlantillaEstudioIntegracionId = periodoEstudioIntegracion.PlantillaEstudioIntegracionId,
                PlanOfertado = new PlanOfertadoDto
                {
                    Id = planOfertado.Id, 
                    Plan = new PlanDto
                    {
                        Id = planOfertado.Plan.Id,
                        Codigo = planOfertado.Plan.Codigo,
                        Nombre = planOfertado.Plan.Nombre
                    },
                    PeriodoAcademico = new PeriodoAcademicoDto
                    {
                        Id = planOfertado.PeriodoAcademico.Id,
                        FechaInicio = planOfertado.PeriodoAcademico.FechaInicio,
                        FechaFin = planOfertado.PeriodoAcademico.FechaFin,
                        Nombre = planOfertado.PeriodoAcademico.Nombre
                    }
                },
                PeriodoEstudioUnir =  new PeriodoEstudioUnirDto
                {
                    Id = periodoEstudioIntegracion.PeriodoEstudioUnir.Id,
                    EstudioUnir = new EstudioUnirDto
                    {
                        Nombre = periodoEstudioIntegracion.PeriodoEstudioUnir.EstudioUnir.Nombre
                    },
                    PeriodoMatriculacionUnir = new PeriodoMatriculacionUnirDto
                    {
                        Nombre = periodoEstudioIntegracion.PeriodoEstudioUnir.PeriodoMatriculacionUnir.Nombre
                    }
                }  
            };
            result.Value = element;
            return result;
        }

        public BaseResult Crear(SavePeriodoEstudioIntegracionParameters model)
        {
            var result = new BaseResult();

            //Validaciones de Referencias

            var periodoEstudio = _gestorContext.PeriodoEstudioUnir.Find(model.IdPeriodoEstudio);
            if (periodoEstudio == null)
                result.Errors.Add(PeriodoEstudioIntegracionStrings.ErrorPeriodoEstudioNoExiste);

            var planOfertadoErp = _erpContext.PlanOfertado.Find(model.IdPlanOfertado);
            if (planOfertadoErp == null)
                result.Errors.Add(PeriodoEstudioIntegracionStrings.ErrorPlanOfertadoErpNoExiste);


            //Validacion Mapeo Existente
            if (_gestorContext.PeriodoEstudioIntegracion.Any(pe => pe.Id == model.IdPeriodoEstudio)) //Validacion 4
                result.Errors.Add(PeriodoEstudioIntegracionStrings.ErrorPeriodoEstudioYaMapeado);

            if (result.HasErrors)
                return result;

            var planIntegracion = _gestorContext.PlantillaEstudioIntegracion.Find(planOfertadoErp.PlanId);//Validacion 5
            if (planIntegracion == null)
            {
                result.Errors.Add(PeriodoEstudioIntegracionStrings.ErrorPlanNoExiste);
                return result;
            }

            var estudioIntegracion = _gestorContext.EstudioIntegracion.Find(periodoEstudio.EstudioId);
            if (estudioIntegracion == null || planIntegracion == null || estudioIntegracion.PlantillaEstudioIntegracionId != planIntegracion.Id) //Validacion 6
                result.Errors.Add(PeriodoEstudioIntegracionStrings.ErrorMapeoEstudioConPlanEstudio);

            if (!_gestorContext.PeriodosMatriculacionesIntegracion.Any(pm => pm.Id == periodoEstudio.PeriodoMatriculacionId && pm.PeriodoAcademicoId == planOfertadoErp.PeriodoAcademicoId))//Validacion 7
                result.Errors.Add(PeriodoEstudioIntegracionStrings.ErrorMapeoPeriodoMatriculacionConPeriodoAcademico);

            if (result.HasErrors)
                return result;

            var periodoEstudioIntegracion = new PeriodoEstudioIntegracion
            {
                Id = periodoEstudio.Id,
                PlantillaEstudioIntegracionId = planIntegracion.Id,
                PlanOfertadoId = planOfertadoErp.Id
            };
            _gestorContext.PeriodoEstudioIntegracion.Add(periodoEstudioIntegracion);
            _gestorContext.SaveChanges();
            return result;
        }

        public BaseResult Modificar(SavePeriodoEstudioIntegracionParameters model)
        {
            var result = new BaseResult();
            //Validar Referencias
            var periodoEstudio = _gestorContext.PeriodoEstudioUnir.Find(model.IdPeriodoEstudio);
            if (periodoEstudio == null)
                result.Errors.Add(PeriodoEstudioIntegracionStrings.ErrorPeriodoEstudioNoExiste);

            var planOfertadoErp = _erpContext.PlanOfertado.Find(model.IdPlanOfertado);
            if (planOfertadoErp == null)
                result.Errors.Add(PeriodoEstudioIntegracionStrings.ErrorPlanOfertadoErpNoExiste);

            //Validacion Existencialismo
            var persisted = _gestorContext.PeriodoEstudioIntegracion.Find(model.IdPeriodoEstudio);
            if (persisted == null)
                result.Errors.Add(PeriodoEstudioIntegracionStrings.ErrorPeriodoEstudioNoExiste);

            if (result.HasErrors)
                return result;

            var planIntegracion = _gestorContext.PlantillaEstudioIntegracion.Find(planOfertadoErp.PlanId);//Validacion 5
            if (planIntegracion == null)
            {
                result.Errors.Add(PeriodoEstudioIntegracionStrings.ErrorPlanNoExiste);
                return result;
            }

            var estudioIntegracion = _gestorContext.EstudioIntegracion.Find(periodoEstudio.EstudioId);
            if (estudioIntegracion == null || planIntegracion == null || estudioIntegracion.PlantillaEstudioIntegracionId != planIntegracion.Id) //Validacion 6
                result.Errors.Add(PeriodoEstudioIntegracionStrings.ErrorMapeoEstudioConPlanEstudio);

            if (!_gestorContext.PeriodosMatriculacionesIntegracion.Any(pm => pm.Id == periodoEstudio.PeriodoMatriculacionId && pm.PeriodoAcademicoId == planOfertadoErp.PeriodoAcademicoId))//Validacion 7
                result.Errors.Add(PeriodoEstudioIntegracionStrings.ErrorMapeoPeriodoMatriculacionConPeriodoAcademico);

            if (result.HasErrors)
                return result;

            persisted.Id = model.IdPeriodoEstudio;
            persisted.PlanOfertadoId = model.IdPlanOfertado;
            persisted.PlantillaEstudioIntegracionId = planIntegracion.Id;

            _gestorContext.SaveChanges();

            return result;
        }

        public BaseResult Eliminar(int[] ids)
        {
            var result = new BaseResult();
            if (ids == null)
            {
                result.Errors.Add("Error de datos: los datos enviados no son validos");
                return result;
            }
            var periodosoEstudios = _gestorContext.PeriodoEstudioIntegracion.Where(a => ids.Contains(a.Id)).ToList();
            foreach (var periodoEstudioIntegracion in periodosoEstudios)
            {
                _gestorContext.PeriodoEstudioIntegracion.Remove(periodoEstudioIntegracion);
            }
            _gestorContext.SaveChanges();
            return result;
        }
    }
}