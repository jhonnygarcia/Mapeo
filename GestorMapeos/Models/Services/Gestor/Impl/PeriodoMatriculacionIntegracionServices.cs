using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using GestorMapeos.Parameters.PeriodoMatriculacionIntegracion;
using GestorMapeos.Globalization.Services;
using GestorMapeos.Models.Dto.Erp20;
using GestorMapeos.Models.Dto.Gestor;
using GestorMapeos.Models.Model.Erp20;
using GestorMapeos.Models.Model.Gestor;
using GestorMapeos.Models.Model.Gestor.Entities;
using GestorMapeos.TransferStructs;

namespace GestorMapeos.Models.Services.Gestor.Impl
{
    public class PeriodoMatriculacionIntegracionServices: IPeriodoMatriculacionIntegracionServices
    {
        private readonly GestorContext _gestorContext;
        private readonly ErpContext _erpContext;
        public PeriodoMatriculacionIntegracionServices(GestorContext gestorContext, ErpContext erpContext)
        {
            _gestorContext = gestorContext;
            _erpContext = erpContext;
        }

        public ResultList<PeriodoMatriculacionIntegracionDto> GetPagedPeriodoMatriculacion(
            SearchPeriodoMatriculacionIntegracionParameters parameters)
        {
            var result = new ResultList<PeriodoMatriculacionIntegracionDto>();
            var query = _gestorContext.PeriodosMatriculacionesIntegracion.AsQueryable();
            if (parameters.FilterIdPeriodoMatriculacion.HasValue)
                query = query.Where(a => a.Id == parameters.FilterIdPeriodoMatriculacion.Value);
            if (parameters.FilterIdPeriodoAcademico.HasValue)
                query = query.Where(a => a.PeriodoAcademicoId == parameters.FilterIdPeriodoAcademico.Value);

            if (parameters.IdPeriodoMatriculacion.HasValue)
                query = query.Where(a => a.Id == parameters.IdPeriodoMatriculacion.Value);
            if (parameters.IdPeriodoAcademico.HasValue)
                query = query.Where(a => a.PeriodoAcademicoId == parameters.IdPeriodoAcademico.Value);


            var order = parameters.GetEnum(SearchPeriodoMatriculacionIntegracionParameters
                .PeriodoMatriculacionIntegracionOrderColumn.IdPeriodoMatriculacion);
            switch (order)
            {
                case SearchPeriodoMatriculacionIntegracionParameters.PeriodoMatriculacionIntegracionOrderColumn.IdPeriodoMatriculacion:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.Id)
                        : query.OrderByDescending(o => o.Id);
                    break;
                case SearchPeriodoMatriculacionIntegracionParameters.PeriodoMatriculacionIntegracionOrderColumn.PeriodoMatriculacion:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.PeriodoMatriculacion.Nombre)
                        : query.OrderByDescending(o => o.PeriodoMatriculacion.Nombre);
                    break;
                case SearchPeriodoMatriculacionIntegracionParameters.PeriodoMatriculacionIntegracionOrderColumn.FechaInicio:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.PeriodoMatriculacion.FechaInicio)
                        : query.OrderByDescending(o => o.PeriodoMatriculacion.FechaInicio);
                    break;
                case SearchPeriodoMatriculacionIntegracionParameters.PeriodoMatriculacionIntegracionOrderColumn.FechaFin:
                    query = parameters.Ascendente
                        ? query.OrderBy(o => o.PeriodoMatriculacion.FechaFin)
                        : query.OrderByDescending(o => o.PeriodoMatriculacion.FechaFin);
                    break;
            }
            var listado = query.Select(a => new PeriodoMatriculacionIntegracionDto
            {
                Id = a.Id,
                PeriodoMatriculacion = new PeriodoMatriculacionUnirDto
                {
                    Nombre = a.PeriodoMatriculacion.Nombre,
                    Id = a.PeriodoMatriculacion.Id,
                    FechaInicio = a.PeriodoMatriculacion.FechaInicio,
                    FechaFin = a.PeriodoMatriculacion.FechaFin,
                    Nro = a.PeriodoMatriculacion.Nro
                },
                PeriodoAcademicoId = a.PeriodoAcademicoId
            }).Skip((parameters.PageIndex - 1)*parameters.ItemsPerPage)
                .Take(parameters.ItemsPerPage)
                .ToList();

            /*Obtener valores otros del ERP-20*/
            var idsPeriodosAcademicos = listado.Select(a => a.PeriodoAcademicoId).ToArray();
            var periodoAcademicos = _erpContext.PeriodosAcademicos.Where(p => idsPeriodosAcademicos.Contains(p.Id)).ToList();

            listado.ForEach((dto) =>
            {
                var periodoAcademico = periodoAcademicos.FirstOrDefault(a => a.Id == dto.PeriodoAcademicoId);
                if (periodoAcademico != null)
                {
                    dto.PeriodoAcademico = new PeriodoAcademicoDto
                    {
                        Id = periodoAcademico.Id,
                        Nombre = periodoAcademico.Nombre,
                        FechaInicio =  periodoAcademico.FechaInicio,
                        FechaFin = periodoAcademico.FechaFin,
                        Nro = periodoAcademico.Nro
                    };
                }
            });

            result.Elements = listado;
            result.TotalElements = query.Count();
            result.PageCount = parameters.ItemsPerPage;

            return result;
        }

        public ResultValue<PeriodoMatriculacionIntegracionDto> Get(int id)
        {
            var result = new ResultValue<PeriodoMatriculacionIntegracionDto>();
            var periodoMatriculacionGestor = _gestorContext.PeriodosMatriculacionesUnir.Find(id);
            if (periodoMatriculacionGestor == null)
            {
                result.Errors.Add(PeriodoMatriculacionIntegracionStrings.ErrorPeriodoMatriculacionUnirInconsistente);
            }
            var periodoMatriculacionIntegracion = _gestorContext.PeriodosMatriculacionesIntegracion.Find(id);
            if (periodoMatriculacionIntegracion == null)
            {
                result.Errors.Add(PeriodoMatriculacionIntegracionStrings.ErrorPeriodoMatriculacionIntegracionInconsistente);
            }
            var periodoAcademico = _erpContext.PeriodosAcademicos.FirstOrDefault(
                    pa => pa.Id == periodoMatriculacionIntegracion.PeriodoAcademicoId);
            if (periodoAcademico == null)
            {
                result.Errors.Add(PeriodoMatriculacionIntegracionStrings.ErrorPeriodoAcademicoInconsistente);
            }
            if (result.HasErrors)
            {
                result.Type = ResultType.ElementNotFound;
                return result;
            }
            var element = new PeriodoMatriculacionIntegracionDto
            {
                PeriodoMatriculacion = new PeriodoMatriculacionUnirDto
                {
                    Id = periodoMatriculacionGestor.Id,
                    Nombre = periodoMatriculacionGestor.Nombre,
                    AnyoAcademico = periodoMatriculacionGestor.AnyoAcademico,
                    FechaInicio = periodoMatriculacionGestor.FechaInicio,
                    FechaFin = periodoMatriculacionGestor.FechaFin,
                    Nro = periodoMatriculacionGestor.Nro
                },
                PeriodoAcademico = periodoAcademico != null ? new PeriodoAcademicoDto
                {
                    Id = periodoAcademico.Id,
                    Nombre = periodoAcademico.Nombre,
                    AnyoAcademico = new AnyoAcademicoDto
                    {
                        AnyoInicio = periodoAcademico.AnyoAcademico.AnyoInicio,
                        AnyoFin = periodoAcademico.AnyoAcademico.AnyoFin
                    },
                    FechaInicio = periodoAcademico.FechaInicio,
                    FechaFin = periodoAcademico.FechaFin,
                    Nro = periodoAcademico.Nro
                } : null
            };
            result.Value = element;
            return result;
        }

        public BaseResult Crear(SavePeriodoMatriculacionIntegracionParameters model)
        {
            var result = new BaseResult();
            //Verificacion Existencialismo Periodo Matriculacion Unir
            var periodoMatriculaicon = _gestorContext.PeriodosMatriculacionesUnir.Find(model.IdPeriodoMatriculacionGestor);
            if (periodoMatriculaicon == null)
            {
                result.Errors.Add(PeriodoMatriculacionIntegracionStrings.ErrorPeriodoMatriculacionUnirInconsistente);
            }
            //Verificacion Existencialismo Periodo Academico
            var periodoAcademico = _erpContext.PeriodosAcademicos.Find(model.IdRefPeriodoAcademicoErp);
            if (periodoAcademico == null)
            {
                result.Errors.Add(PeriodoMatriculacionIntegracionStrings.ErrorPeriodoAcademicoInconsistente);
                return result;
            }
            //Verificacion Existencialismo Integracion
            var persisted = _gestorContext.PeriodosMatriculacionesIntegracion.Find(model.IdPeriodoMatriculacionGestor);
            if (persisted != null)
            {
                result.Errors.Add(PeriodoMatriculacionIntegracionStrings.ErrorMapeoPeriodoMatriculacionIntegracionExistente);
                return result;
            }
            var periodoMatriculacion = new PeriodoMatriculacionIntegracion
            {
                Id = periodoMatriculaicon.Id,
                PeriodoAcademicoId = periodoAcademico.Id
            };
            _gestorContext.PeriodosMatriculacionesIntegracion.Add(periodoMatriculacion);
            _gestorContext.SaveChanges();
            
            return result;
        }

        public BaseResult Modificar(SavePeriodoMatriculacionIntegracionParameters model)
        {
            var result = new BaseResult();
            //Verificacion Existencialismo Periodo Matriculacion Unir
            var periodoMatriculaicon = _gestorContext.PeriodosMatriculacionesUnir.Find(model.IdPeriodoMatriculacionGestor);
            if (periodoMatriculaicon == null)
            {
                result.Errors.Add(PeriodoMatriculacionIntegracionStrings.ErrorPeriodoMatriculacionUnirInconsistente);
            }
            //Verificacion Existencialismo Periodo Academico
            var periodoAcademico = _erpContext.PeriodosAcademicos.Find(model.IdRefPeriodoAcademicoErp);
            if (periodoAcademico == null)
            {
                result.Errors.Add(PeriodoMatriculacionIntegracionStrings.ErrorPeriodoAcademicoInconsistente);
                return result;
            }
            //Verificacion Existencialismo Integracion
            var persisted = _gestorContext.PeriodosMatriculacionesIntegracion.Find(model.IdPeriodoMatriculacionGestor);
            if (persisted == null)
            {
                result.Errors.Add(PeriodoMatriculacionIntegracionStrings.ErrorPeriodoMatriculacionIntegracionInconsistente);
                return result;
            }

            persisted.PeriodoAcademicoId = model.IdRefPeriodoAcademicoErp;
            _gestorContext.SaveChanges();
             
            return result;
        }

        public BaseResult Eliminar(int[] ids)
        {
            var result = new BaseResult();
            if (ids == null)
            {
                result.Errors.Add(PeriodoMatriculacionIntegracionStrings.ErrorDatosEliminar);
                return result;
            }
            var periodosoMatriculaciones = _gestorContext.PeriodosMatriculacionesIntegracion.Where(a => ids.Contains(a.Id)).ToList();
            foreach (var periodoMatriculacion in periodosoMatriculaciones)
            {
                _gestorContext.PeriodosMatriculacionesIntegracion.Remove(periodoMatriculacion);
            }
            _gestorContext.SaveChanges();
            return result;
        }
    }
}