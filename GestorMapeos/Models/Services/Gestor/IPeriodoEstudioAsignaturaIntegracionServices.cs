using System;
using GestorMapeos.Models.Dto.Gestor;
using GestorMapeos.Parameters.AsignaturaOfertadaIntegracion;
using GestorMapeos.Parameters.PeriodoEstudioAsignaturaIntegracion;
using GestorMapeos.TransferStructs;

namespace GestorMapeos.Models.Services.Gestor
{
    public interface IPeriodoEstudioAsignaturaIntegracionServices
    {
        BaseResult Crear(SavePeriodoEstudioAsignaturaIntegracionParameters model);
        BaseResult Modificar(SavePeriodoEstudioAsignaturaIntegracionParameters model);
        BaseResult Eliminar(int[] ids);
        ResultList<PeriodoEstudioAsignaturaIntegracionDto> GetPagedPeriodoEstudioAsignatura(
            SearchPeriodoEstudioAsignasturaIntegracionParameters parameters);
        ResultValue<PeriodoEstudioAsignaturaIntegracionDto> Get(int id);
    }
}